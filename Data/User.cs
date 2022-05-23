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
        /// Prints the <see cref="User"/> to the console as a formatted string-JSON element 
        /// </summary>
        public void Print()
        {
            string s = "   ";

            Arcade.ConsoleColors.Set("cyan");
            Console.Write($"{{\n{s}\"id\": ");         Arcade.ConsoleColors.Set("magenta");
            Console.Write(this.id);                    Arcade.ConsoleColors.Set("cyan");
            Console.Write($"\n{s}\"username\": \"");   Arcade.ConsoleColors.Set("magenta");
            Console.Write(this.name);                  Arcade.ConsoleColors.Set("cyan");
            Console.Write($"\"\n{s}\"password\": \""); Arcade.ConsoleColors.Set("magenta");
            Console.Write(this.password);              Arcade.ConsoleColors.Set("cyan");
            Console.Write($"\"\n{s}\"balance\": ");    Arcade.ConsoleColors.Set("magenta");
            Console.Write($"${this.balance}");               Arcade.ConsoleColors.Set("cyan");
            Console.WriteLine("\n}");
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

        /// <summary>
        /// Prompt the user to either Sign-In to an existing account or Sign-Up for an <see cref="Arcade"/> account<br></br><br></br>
        /// The user will then either sign-in or sign-up, and the <see cref="User"/> they made/signed-in to will be returned
        /// </summary>
        /// <returns>the <see cref="User"/> they made/signed-in to</returns>
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
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                    first = false;

                    Arcade.ConsoleColors.Set("cyan"); Console.WriteLine($"{dl}\n\n{space}Sign In");

                    Arcade.ConsoleColors.Set("cyan"); Console.Write($"\n{dl}\n\nUsername: ");
                    Arcade.ConsoleColors.Set("magenta");
                    string username = Console.ReadLine().Trim(' ');

                    Arcade.ConsoleColors.Set("cyan"); Console.Write("Password: ");
                    Arcade.ConsoleColors.Set("magenta");
                    string password = Console.ReadLine().Trim(' ');

                    Console.Write("\n");

                    DotAnimation dotAnimationChecking = new DotAnimation(message: "Looking for your account", messageConsoleColor: "cyan") ;
                    id = await Arcade.Database.DoesUserExist(username, password);

                    if (id == 0)
                    {
                        dotAnimationChecking.End(endMessage: "This account does not exist", endMessageConsoleColor: "darkred");
                        Arcade.ConsoleColors.Set("cyan");
                        Console.WriteLine($"\n{dl}");
                    }
                    else
                    {
                        dotAnimationChecking.End(endMessage: "Found it!", endMessageConsoleColor: "magenta");
                        Arcade.ConsoleColors.Set("cyan");
                    }
                } while (id == 0);

                DotAnimation dotAnimationGetting = new DotAnimation(message: "Signing in", endMessage: "Done!\n", messageConsoleColor: "cyan", endMessageConsoleColor: "magenta");
                User user = await Arcade.Database.GetUser(id);
                
                Thread.Sleep(2000);
                dotAnimationGetting.End();
                Thread.Sleep(500);
                
                Arcade.ConsoleColors.Set("cyan"); Console.WriteLine(dl); Arcade.ConsoleColors.Reset();
                Console.Clear();
                return user;
            }
            else // option == 2
            {
                bool first = true;
                string username;
                string password;
                string confirm_password;
                while (true)
                {
                    Arcade.ConsoleColors.Set("cyan");
                    if (!first)
                    {
                        Thread.Sleep(1000);
                        Console.Clear();
                    }

                    first = false;
                    Console.WriteLine($"{dl}\n\n{space}Sign Up\n");
                    Console.Write($"{dl}\n\nCreate Your Username: ");
                    Arcade.ConsoleColors.Set("magenta");
                    username = Console.ReadLine().Trim(' ');

                    string response;
                    while (true)
                    {
                        Console.Clear(); Arcade.ConsoleColors.Set("cyan");

                        // rewriting the prompt and the response to the screen after clearing the console
                        Console.WriteLine($"{dl}\n\n{space}Sign Up\n");
                        Console.Write($"{dl}\n\nCreate Your Username: ");
                        Arcade.ConsoleColors.Set("magenta");
                        Console.WriteLine(username);

                        Arcade.ConsoleColors.Set("cyan");
                        Console.Write($"\n{dl}\n\nDo you want to choose a different username? (y/n): ");
                        Arcade.ConsoleColors.Set("magenta");
                        response = Console.ReadLine().Trim(' ');
                        if (new string[] { "yes", "y", "n", "no" }.Contains(response))
                            break;
                    }

                    if (response == "y" || response == "yes")
                    {
                        first = true;
                        Console.Clear();
                        continue;
                    }

                    Console.Write("\n\n");
                    DotAnimation dotAnimation = new DotAnimation(message: "Checking Username Availability", messageConsoleColor: "cyan");
                    bool available = await Database.CheckUsernameAvailability(username);
                    if (!available)
                    {
                        Thread.Sleep(500);
                        dotAnimation.End(endMessage: "Not Available", endMessageConsoleColor: "darkred");
                        Console.WriteLine($"\n{dl}\n");
                        Thread.Sleep(500);
                        continue;
                    }
                    else
                    {
                        Thread.Sleep(500);
                        dotAnimation.End(endMessage: "Available", endMessageConsoleColor: "magenta");
                        Console.WriteLine($"\n{dl}\n");
                        Thread.Sleep(500);
                    }

                    Arcade.ConsoleColors.Set("cyan");
                    first = true;
                    Thread.Sleep(1000);
                    Console.Clear();

                    while (true)
                    {

                        if (!first)
                        {
                            Thread.Sleep(1000);
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
                User user = await Arcade.Database.CreateUser(username, password, 375);
                return user;
            }
        }

        /// <summary>
        /// Ask the user if they want to sign-in to an existing account, or sign-up for an Arcade account
        /// </summary>
        /// <returns>
        /// <see cref="int"/> 1 or 2.<br></br>
        /// 1 if the user wants to sign-in to an existing account
        /// <br></br>2 if the user wants to sign-up for an Arcade account
        /// </returns>
        private static int GetSignInOrSignUpOption()
        {
            /* Message that will be displayed:
             * You must either SIGN IN or SIGN UP to play this game!

             * Type 1 to Sign-In to an already existing account.
             * Type 2 to Sign-Up for an Arcade account. Choose an option:
            */

            bool passed;
            int option;

            do
            {
                Thread.Sleep(15);
                Console.Clear();

                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine(dl + "\n");
                
                Console.Write("You must either ");                                     Arcade.ConsoleColors.Set("magenta");
                Console.Write("SIGN IN ");                                             Arcade.ConsoleColors.Set("cyan");
                Console.Write("or ");                                                  Arcade.ConsoleColors.Set("magenta");
                Console.Write("SIGN UP ");                                             Arcade.ConsoleColors.Set("cyan");
                Console.Write("to play this game!\n\nType ");                          Arcade.ConsoleColors.Set("magenta");
                Console.Write("1 ");                                                   Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine("to Sign-In to an already existing account.");
                Console.Write("Type ");                                                Arcade.ConsoleColors.Set("magenta");
                Console.Write("2 ");                                                   Arcade.ConsoleColors.Set("cyan");
                Console.Write("to Sign-Up for an Arcade account. Choose an option: "); Arcade.ConsoleColors.Set("magenta");

                string response = Console.ReadLine().Trim(' ');
                passed = int.TryParse(response, out option);
                Arcade.ConsoleColors.Set("cyan");
                if (!passed) Console.WriteLine("\n" + dl + "\n");

            } while (!passed || (option != 1 && option != 2));

            Console.Clear();
            return option;
        }

        /// <summary>
        /// Ask the user to place a bet
        /// </summary>
        /// <returns>the user's bet</returns>
        public async Task<double> GetBet()
        {
            string s = "";
            for (int i = 0; i < 51; i++) s += ' ';

            int bet;
            bool first = true;
            while (true)
            {
                string error;
                ConsoleColors.Set("cyan"); 
                if (!first)
                    Thread.Sleep(1000); Console.Clear();
                first = false;
                Console.WriteLine($"{dl}\n\n{s}Place Your Bet!\n\n{dl}\n");

                Console.Write("Your current balance is: "); Arcade.ConsoleColors.Set("magenta");
                Console.WriteLine($"${this.balance}"); Arcade.ConsoleColors.Set("cyan");
                Console.Write($"Place Your Bet: "); Arcade.ConsoleColors.Set("magenta");
                string response = Console.ReadLine();
                bool valid = int.TryParse(response, out bet);
                if (valid)
                {
                    if (bet > this.balance)
                    {
                        error = "You can't afford this bet!";
                    }
                    else if (bet == 0)
                    {
                        error = "Your bet must be greater than $0!";
                    }
                    else
                    {
                        ConsoleColors.Set("cyan"); Console.WriteLine($"\n{dl}\n");
                        Thread.Sleep(1000); Console.Clear();
                        break;
                    }
                }
                else
                {
                    error = "Your bet must be a number less than 9,007,199,254,740,992";
                }

                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine($"\n{dl}\n");
                Arcade.ConsoleColors.Set("darkred");
                Console.WriteLine(error);
                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine($"\n{dl}\n");
            }

            Console.Write($"{dl}\n\nThe bet you placed was: ");
            Arcade.ConsoleColors.Set("magenta");
            Console.WriteLine($"{bet}\n");

            DotAnimation dotAnimation = new DotAnimation(message: "Updating Balance", endMessage: $"Done!", messageConsoleColor: "cyan", endMessageConsoleColor: "magenta");
            await this.RemoveBalance(bet);
            Thread.Sleep(550);
            dotAnimation.End();
            Console.Write("\nYour balance is now: ");
            Arcade.ConsoleColors.Set("magenta");
            Console.Write($"${this.balance}");
            Arcade.ConsoleColors.Set("cyan");
            Console.WriteLine($"\n\n{dl}\n");
            Thread.Sleep(1000);
            return bet;
        }
    }
}

