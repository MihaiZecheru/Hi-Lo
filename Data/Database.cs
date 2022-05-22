// todo: make sure all games start with the user signing in

using Newtonsoft.Json;

/*
 * This class will handle the backend of the Arcade Console Application and Arcade Discord Bot
 */

namespace Arcade
{
    public class Database
    {
        private static string api_key = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhcGlfa2V5IjoiMDAxYTJmMGItMTY0ZC00MjMzLWIzYTYtNmMzZTYwMTE2ODgzIiwidGVuYW50X2lkIjo1NzgsImp0aV9rZXkiOiIwMzQ5MWEyYS1kOTYxLTExZWMtYWFlMy0wYTU4YTlmZWFjMDIifQ.LgLl0SOLMBZECNfpMjIJgZm4t4Z1-dIwR3XxWR8pDz4";
        private static string api_id = "466";
        private static string api_url = "https://arcadeapp.fireapis.com/userdata/";

        /// <summary>
        /// Get's all userdata under the /userdata endpoint
        /// </summary>
        /// <returns>The JSON response as a <see cref="List{T}"/> of <seealso cref="Dictionary{TKey, TValue}"/></returns>
        public static async Task<List<Dictionary<string, object>>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-ID", api_id);
                client.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", api_key);

                using (HttpResponseMessage response = await client.GetAsync(api_url + "all?page_size=200000&page=1"))
                {
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    using (HttpContent responseContent = response.Content)
                    {
                        string content = await responseContent.ReadAsStringAsync();
                        Dictionary<string, object> _db = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                        Newtonsoft.Json.Linq.JArray _data = (Newtonsoft.Json.Linq.JArray)_db["data"];
                        return _data.ToObject<List<Dictionary<string, object>>>();
                    }
                }
            }
        }

        /// <summary>
        /// Get's the userdata corresponding to the given <paramref name="userid"/>
        /// </summary>
        /// <param name="userid"></param>
        /// <returns><see cref="Arcade.User"/></returns>
        public static async Task<User> GetUser(int userid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-ID", api_id);
                client.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", api_key);

                using (HttpResponseMessage response = await client.GetAsync(api_url + userid))
                {
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    using (HttpContent responseContent = response.Content)
                    {
                        string content = await responseContent.ReadAsStringAsync();
                        Dictionary<string, object> user = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                        return new User(Convert.ToInt32(user["id"]), (string)user["username"], (string)user["password"], Convert.ToDouble(user["balance"]));
                    }
                }
            }
        }

        /// <summary>
        /// Find the next available ID in the database
        /// </summary>
        /// <returns><see cref="int"/> ID</returns>
        private static async Task<int> GetNextId()
        {
            List<Dictionary<string, object>> users = await GetAll();

            int greatest_id = 0;
            foreach (Dictionary<string, object> user in users)
            {
                if ((Int64)user["id"] > greatest_id)
                    greatest_id = Convert.ToInt32(user["id"]);
            }

            return greatest_id + 1;
        }

        /// <summary>
        /// Creates a new <see cref="Arcade.User"/>, then posts it to the database<br></br><br></br>
        /// If a password is not provided, the <see cref="User"/> will be prompted to create one
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">user password</param>
        /// <param name="balance">user balance</param>
        /// <returns><see cref="Arcade.User"/></returns>
        public static async Task<User> CreateUser(string username, string password, double balance)
        {
            DotAnimation dotAnimation = new DotAnimation("Creating Your Account");
            dotAnimation.ENDMESSAGE = "Done!";

            User user = new User(await GetNextId(), username, password, balance);
            HttpContent body = new FormUrlEncodedContent(user.GetAsKVpairs());

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-ID", api_id);
                client.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", api_key);

                using (HttpResponseMessage response = await client.PostAsync(api_url, body))
                {
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    using (HttpContent responseContent = response.Content)
                    {
                        string content = await responseContent.ReadAsStringAsync();
                    }

                    dotAnimation.End();
                    return user;
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="Arcade.User"/>, then posts it to the database<br></br><br></br>
        /// If a password is not provided, the <see cref="User"/> will be prompted to create one
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="balance">user balance</param>
        /// <returns><see cref="Arcade.User"/></returns>
        public static async Task<User> CreateUser(string username, double balance)
        {
            // Ask user to create a password 
            string password = CreatePassword();

            return await CreateUser(username, password, balance);
        }

        /// <summary>
        /// Updates a <see cref="Arcade.User"/> in the database
        /// </summary>
        /// <param name="user"><see cref="Arcade.User"/></param>
        /// <returns><see cref="Arcade.User"/></returns>
        public static async Task<User> UpdateUser(User user)
        {
            HttpContent body = new FormUrlEncodedContent(user.GetAsKVpairs());

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-ID", api_id);
                client.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", api_key);

                using (HttpResponseMessage response = await client.PutAsync(api_url + user.id, body))
                {
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    using (HttpContent responseContent = response.Content)
                    {
                        string content = await responseContent.ReadAsStringAsync();
                    }
                    return user;
                }
            }
        }

        private static string CreatePassword()
        {
            bool firstTime = true;
            string dl = "-------------------------------------------------------------";
            string password = "placeholder1";
            string confirm_password = "placeholder2";

            do
            {
                if (firstTime)
                {
                    Console.WriteLine($"{dl}\n\nCreate a password\n\n{dl}\n");
                    firstTime = false;
                }
                else if (password != confirm_password)
                    Console.Write($"{dl}\n\nPasswords must match\n\n{dl}\n\n");
                else if (password == "")
                    Console.Write($"{dl}\n\nYour password cannot be \"\"\n\n{dl}\n\n");
                else if (password.Length < 8)
                    Console.Write($"{dl}\n\nYour password must be 8 characters or longer\n\n{dl}\n\n");

                Console.Write("Password: ");
                password = Console.ReadLine().Trim(' ');
                Console.Write("\nConfirm: ");
                confirm_password = Console.ReadLine().Trim(' ');
                Console.Write("\n");
            } while (password != confirm_password || password == "" || password.Length < 8);

            return password;
        }

        public static async Task<int> DoesUserExist(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-ID", api_id);
                client.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", api_key);
                using (HttpResponseMessage response = await client.GetAsync(api_url + $"?username={username}&password={password}&page_size=1&page=1"))
                {
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    using (HttpContent responseContent = response.Content)
                    {
                        string content = await responseContent.ReadAsStringAsync();
                        Dictionary<string, object> _db = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                        Newtonsoft.Json.Linq.JArray _data = (Newtonsoft.Json.Linq.JArray)_db["data"];
                        List<Dictionary<string, object>> users = _data.ToObject<List<Dictionary<string, object>>>();

                        if (users.Count == 0) return 0;                        

                        Dictionary<string, object> user = users[0];
                        return Convert.ToInt32(user["id"]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Loading animation with dots ...
    /// </summary>
    internal class DotAnimation
    {
        private bool END { get; set; }
        private string MESSAGE { get; set; }
        public string ENDMESSAGE { get; set; }

        /// <summary>
        /// Takes the given <paramref name="message"/> and appends periods (.) to the end to create a loading animation
        /// </summary>
        /// <param name="message">The initial <paramref name="message"/> to animate</param>
        /// <param name="endMessage">The message to display when <see cref="DotAnimation.End"/> is called</param>
        /// <param name="consoleColor">The color of <paramref name="endMessage"/> in the animation</param>
        public DotAnimation(string message, string endMessage = null, string consoleColor = null)
        {
            this.ENDMESSAGE = endMessage;
            this.MESSAGE = message;
            this.END = false;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                if (consoleColor != null)
                    Arcade.ConsoleColors.Set(consoleColor);

                Console.Write(this.MESSAGE);
                while (true)
                {
                    Console.Write($"\r{this.MESSAGE} .");
                    if (END) return;
                    Thread.Sleep(350);
                    if (END) return;
                    Console.Write($"\r{this.MESSAGE} ..");
                    if (END) return;
                    Thread.Sleep(350);
                    if (END) return;
                    Console.Write($"\r{this.MESSAGE} ...");
                    if (END) return;
                    Thread.Sleep(350);
                }
            }).Start();
        }

        /// <summary>
        /// Ends this <see cref="DotAnimation"/>
        /// </summary>
        /// <param name="endMessage">The phrase to append to the end of the <paramref name="message"/> when <see cref="DotAnimation.End"/> is called</param>
        /// <param name="consoleColor">The color of the <paramref name="endMessage"/> in the animation</param>
        public void End(string endMessage, string consoleColor = null)
        {
            this.END = true;

            Console.Write($"\r{this.MESSAGE} ... ");
            
            if (consoleColor != null)
                Arcade.ConsoleColors.Set(consoleColor);

            Console.WriteLine(endMessage);
            Arcade.ConsoleColors.SetToPrevious();
        }

        /// <summary>
        /// Ends this <see cref="DotAnimation"/>
        /// </summary>
        /// <param name="consoleColor">The color of the <see cref="DotAnimation.ENDMESSAGE"/> in the animation</param>
        public void End(string consoleColor = null)
        {
            this.END = true;
            if (this.ENDMESSAGE == null)
                throw new Exception("No <endMessage> given. Call End() with an <endMessage>, or manually change the <endMessage> property with myDotAnimation.ENDMESSAGE = \"my end message\"");

            Console.Write($"\r{this.MESSAGE} ... ");

            if (consoleColor != null)
                Arcade.ConsoleColors.Set(consoleColor);

            Console.WriteLine(this.ENDMESSAGE);
            Arcade.ConsoleColors.SetToPrevious();
        }

    }
}
