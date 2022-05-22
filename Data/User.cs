using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade
{
    public class User
    {
        private static string dl = "------------------------------------------------------------------------------------------------------------------------";

        public int id { get; }
        public string name { get; set; }
        public string password { get; set; }
        public double balance { get; set; }

        /// <summary>
        /// Initializes the user
        /// </summary>
        /// <param name="id">
        /// Database Identifier, MUST MATCH WITH THE DATABASE.<br></br>
        /// Use <see cref="Arcade.Database.GetNextId"/> to get the proper id
        /// </param>
        /// <param name="name"><see cref="User"/>'s name</param>
        /// <param name="password"><see cref="User"/>'s password</param>
        /// <param name="balance"><see cref="User"/>'s starting balance</param>
        public User(int id, string name, string password, double balance)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.balance = balance;
        }

        /// <summary>
        /// Packages the user into a formatted string-JSON element
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            string s = "    ";
            return $"{{\n{s}\"id\": {this.id}\n{s}\"username\": \"{this.name}\"\n{s}\"password\": \"{this.password}\"\n{s}\"balance\": {this.balance}\n}}";
        }

        /// <summary>
        /// Prints the <see cref="User"/> to the console as a formatted string-JSON element 
        /// </summary>
        public void Print()
        {
            Console.WriteLine(this.ToString());
        }

        /// <summary>
        /// Get the <see cref="User"/> as a <see cref="List{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/><br></br>
        /// Used for packaging the <see cref="User"/> for updating the database
        /// </summary>
        /// <returns>The packaged <see cref="User"/> as a <see cref="List{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/></returns>
        public IEnumerable<KeyValuePair<string, string>> GetAsKVpairs()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", this.name),
                new KeyValuePair<string, string>("password", this.password),
                new KeyValuePair<string, string>("balance", this.balance.ToString()),
            };
        }

        /// <summary>
        /// Adds <paramref name="amount"/> to the user's balance, then updates the user in the database.
        /// </summary>
        /// <param name="amount">Add <paramref name="amount"/> to user</param>
        /// <returns><see cref="User"/></returns>
        public async Task<User> AddBalance(double amount)
        {
            this.balance += amount;
            return await Arcade.Database.UpdateUser(this);
        }

        /// <summary>
        /// Removes <paramref name="amount"/> from the user's balance, then updates the user in the database.
        /// </summary>
        /// <param name="amount">Remove <paramref name="amount"/> from user</param>
        /// <returns><see cref="User"/></returns>
        public async Task<User> RemoveBalance(double amount)
        {
            this.balance -= amount;
            return await Arcade.Database.UpdateUser(this);
        }

        public static async Task<User> SignIn()
        {
            string space = "";
            for (int i = 0; i < 55; i++) space += ' ';

            /* ask for sign-in or sign-up;
             * 1 is sign-in
             * 2 is sign-up
            */
            int option = GetSignInOrSignUpOption();

            if (option == 1)
            {
                bool first = true;
                int id;
                do
                {
                    if (!first)
                    {
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    first = false;

                    Arcade.ConsoleColors.Set("cyan");
                    Console.WriteLine($"{dl}\n\n{space}Sign In");

                    Arcade.ConsoleColors.Set("cyan");
                    Console.Write($"\n{dl}\n\nUsername: ");
                    Arcade.ConsoleColors.Set("magenta");
                    string username = Console.ReadLine();
                    Arcade.ConsoleColors.Set("cyan");
                    Console.Write("Password: ");
                    Arcade.ConsoleColors.Set("magenta");
                    string password = Console.ReadLine();

                    Console.Write("\n");

                    Arcade.ConsoleColors.Set("cyan");
                    DotAnimation dotAnimationChecking = new DotAnimation("Looking for your account");
                    id = await Arcade.Database.DoesUserExist(username, password);
                    if (id == 0)
                    {
                        dotAnimationChecking.End("This account does not exist", "darkred");
                        Arcade.ConsoleColors.Set("cyan");
                        Console.WriteLine('\n' + dl);
                    }
                    else
                    {
                        dotAnimationChecking.End("Found it!", "magenta");
                        Arcade.ConsoleColors.Set("cyan");
                    }
                } while (id == 0);

                DotAnimation dotAnimationGetting = new DotAnimation("Signing in", "Done!\n");
                User user = await Arcade.Database.GetUser(id);
                Thread.Sleep(2000);
                dotAnimationGetting.End(consoleColor: "magenta");
                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine(dl);
                Arcade.ConsoleColors.Reset();
                Thread.Sleep(1250);
                Console.Clear();
                return user;
            }
            else // option == 2
            {
                bool first = true;
                string username;
                string password = "placeholder";
                string confirm_password = "placeholder";
                while (true)
                {
                    Arcade.ConsoleColors.Set("cyan");
                    if (!first)
                    {
                        Thread.Sleep(2000);
                        Console.Clear();
                    }

                    first = false;
                    Console.WriteLine($"{dl}\n\n{space}Sign Up\n");
                    Console.Write($"{dl}\n\nCreate Your Username: ");
                    Arcade.ConsoleColors.Set("magenta");
                    username = Console.ReadLine();

                    Arcade.ConsoleColors.Set("cyan");
                    Console.Write($"\n{dl}\n\nDo you want to choose a different username? (y/n): ");
                    Arcade.ConsoleColors.Set("magenta");
                    string response = Console.ReadLine();
                    if (response == "y" || response == "yes")
                        continue;

                    Arcade.ConsoleColors.Set("cyan");
                    first = true;
                    Thread.Sleep(1000);
                    Console.Clear();

                    while (true)
                    {

                        if (!first)
                        {
                            Thread.Sleep(1500);
                            Console.Clear();
                        }

                        first = false;

                        Console.WriteLine($"{dl}\n\n{space}Sign Up\n");
                        Arcade.ConsoleColors.Set("cyan");
                        Console.Write($"{dl}\n\nCreate Your Password: ");
                        Arcade.ConsoleColors.Set("magenta");
                        password = Console.ReadLine().Trim(' ');

                        Arcade.ConsoleColors.Set("cyan");
                        Console.Write("\nConfirm Your Password: ");
                        Arcade.ConsoleColors.Set("magenta");
                        confirm_password = Console.ReadLine().Trim(' ');
                        Arcade.ConsoleColors.Set("cyan");

                        string error = "";
                        if (password != confirm_password)
                            error = "Passwords must match";
                        else if (password == "")
                            error = "Your password cannot be \"\"";
                        else if (password.Length < 8)
                            error = "Your password must be 8 characters or longer";

                        if (error != "")
                        {
                            Console.Write($"{dl}\n\n");
                            Arcade.ConsoleColors.Set("darkred");
                            Console.Write(error);
                            Arcade.ConsoleColors.Set("cyan");
                            Console.Write($"\n\n{dl}\n\n");
                        }

                        if (password == confirm_password && password.Length >= 8 && password != "") break;
                    }
                    break;
                }
                Console.WriteLine($"\n{dl}\n");

                ConsoleColors.Reset();
                User user = await Arcade.Database.GetUser(8);
                return user;
            }
        }

        private static int GetSignInOrSignUpOption()
        {
            /* Message that will be displayed:
             * You must either SIGN IN or SIGN UP to play this game!

             * Type 1 to Sign-In to an already existing account.
             * Type 2 to Sign-Up for an Arcade account. Choose an option:
            */

            bool passed;
            int option;

            bool first = true;
            do
            {
                if (!first)
                {
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                first = false;

                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine(dl + "\n");
                Console.Write("You must either ");
                Arcade.ConsoleColors.Set("magenta");
                Console.Write("SIGN IN ");
                Arcade.ConsoleColors.Set("cyan");
                Console.Write("or ");
                Arcade.ConsoleColors.Set("magenta");
                Console.Write("SIGN UP ");
                Arcade.ConsoleColors.Set("cyan");
                Console.Write("to play this game!\n\nType ");
                Arcade.ConsoleColors.Set("magenta");
                Console.Write("1 ");
                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine("to Sign-In to an already existing account.");
                Console.Write("Type ");
                Arcade.ConsoleColors.Set("magenta");
                Console.Write("2 ");
                Arcade.ConsoleColors.Set("cyan");
                Console.Write("to Sign-Up for an Arcade account. Choose an option: ");
                Arcade.ConsoleColors.Set("magenta");

                string response = Console.ReadLine();
                passed = int.TryParse(response, out option);
                Arcade.ConsoleColors.Set("cyan");
                if (!passed) Console.WriteLine("\n" + dl + "\n");

            } while (!passed || (option != 1 && option != 2));

            Console.Clear();
            return option;
        }
    }
}
