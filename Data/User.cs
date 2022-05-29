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
        /// Use <see cref="Arcade.Backend.GetNextId"/> to get the proper id
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
        public override string ToString()
        {
            string s = "   ";
            return $"{{\n{s}\"id\": {this.id}\n{s}\"username\": \"{this.name}\"\n{s}\"password\": \"{this.password}\"\n{s}\"balance\": {this.balance}\n}}";
        }

        /// <summary>
        /// Prints the <see cref="User"/> to the console as a formatted string-JSON element with syntax highlighting
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
            Console.Write($"${this.balance}");         Arcade.ConsoleColors.Set("cyan");
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
            return await Arcade.Backend.UpdateUser(this);
        }

        /// <summary>
        /// Removes <paramref name="amount"/> from the user's balance, then updates the user in the database.
        /// </summary>
        /// <param name="amount">Remove <paramref name="amount"/> from user</param>
        /// <returns><see cref="User"/></returns>
        public async Task<User> RemoveBalance(double amount)
        {
            this.balance -= amount;
            return await Arcade.Backend.UpdateUser(this);
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

                    (string username, string password) = GetUsernameAndPassword(space);

                    Console.Write("\n");

                    DotAnimation dotAnimationChecking = new DotAnimation(message: "Looking for your account", messageConsoleColor: "cyan") ;
                    id = await Arcade.Backend.DoesUserExist(username, password);

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
                User user = await Arcade.Backend.GetUser(id);
                
                Thread.Sleep(2000);
                dotAnimationGetting.End();
                Thread.Sleep(500);
                
                Arcade.ConsoleColors.Set("cyan"); Console.WriteLine(dl); Arcade.ConsoleColors.Reset();
                Console.Clear();
                return user;
            }
            else // option == 2
            {
                (string username, string password) = await CreateUsernameAndPassword(space);
                
                User user = await Arcade.Backend.CreateUser(username, password, 375);
                return user;
            }
        }

        /// <summary>
        /// Prompt user to enter their username and password
        /// </summary>
        /// <param name="space">Distance from <see cref="Console.CursorLeft"/> (centers the title)</param>
        /// <returns><see cref="Tuple{T1, T2}"/> T1 = username, T2 = password</returns>
        private static Tuple<string, string> GetUsernameAndPassword(string space)
        {
            string baseScreen = $"{dl}\n\n{space}Sign In\n\n{ dl}\n\nUsername: \n\nPassword: \n\n{dl}";
            Arcade.ConsoleColors.Set("cyan");
            Console.WriteLine(baseScreen);
            Console.SetCursorPosition(10, 6);
            Arcade.ConsoleColors.Set("magenta");

            // 0 is the username field, 1 is the password field
            int loc = 0;

            // vertical, horizontal
            int vt, hz;

            // username line length, password line length
            int ul = 0, pl = 0;

            List<char> username = new List<char>(), password = new List<char>();

            while (true)
            {
                ConsoleKeyInfo Key = Console.ReadKey();
                if (loc == 0) username.Add(Key.KeyChar);
                else password.Add(Key.KeyChar);

                ConsoleKey key = Key.Key;

                (hz, vt) = Console.GetCursorPosition();

                if (key == ConsoleKey.Enter && loc == 1)
                {
                    if (ul > 0 && pl > 0)
                        break;
                    else if (ul <= 0)
                    {
                        Console.SetCursorPosition(10, 6);
                        loc = 0;
                        continue;
                    }
                    else
                        Console.SetCursorPosition(10, 8);
                }

                switch (key)
                {
                    case ConsoleKey.Spacebar:
                        if (hz >= (10 + (loc == 0 ? ul : pl)))
                        {
                            Console.SetCursorPosition(hz - 1, vt);
                            Console.Write("");
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        // can only move up when the user is on the password field
                        if (loc != 1)
                            break;

                        Console.SetCursorPosition(10 + ul, vt - 2);
                        loc = 0;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.DownArrow:
                        // can only move down when the user is on the username field
                        if (loc != 0)
                            break;

                        Console.SetCursorPosition(10 + pl, vt + 2);
                        loc = 1;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (hz == 10)
                            break;

                        Console.SetCursorPosition(hz - 1, vt);
                        break;

                    case ConsoleKey.RightArrow:
                        if (hz == 10 + (loc == 0 ? ul : pl))
                            break;

                        Console.SetCursorPosition(hz + 1, vt);
                        break;

                    case ConsoleKey.Backspace:
                        Console.SetCursorPosition(10, vt);
                        for (int i = 0; i < (loc == 0 ? ul : pl); i++)
                            Console.Write(" ");

                        if (loc == 0)
                        {
                            username.Clear();
                            ul = 0;
                        }
                        else
                        {
                            password.Clear();
                            pl = 0;
                        }

                        Console.SetCursorPosition(10, vt);
                        break;

                    case ConsoleKey.End:
                        Console.SetCursorPosition(10 + (loc == 0 ? ul : pl), vt);
                        break;

                    // count chars on each line
                    default:
                        if (Console.GetCursorPosition().Left <= (10 + (loc == 0 ? ul : pl))) break;

                        if (loc == 0) ul++;
                        else pl++;
                        break;
                }
            }

            // username, password
            string u = "", p = "";
            for (int i = 0; i < username.Count; i++)
            {
                if ((int)username[i] != 13)
                    u += username[i];
            }
            for (int i = 0; i < password.Count; i++)
            {
                if ((int)password[i] != 13)
                    p += password[i];
            }

            Arcade.ConsoleColors.Set("cyan");
            Console.SetCursorPosition(0, vt + 3);
            return new Tuple<string, string>(u.Trim(' '), p.Trim(' '));
        }

        /// <summary>
        /// Prompt user to create a username and password
        /// </summary>
        /// <param name="space">Distance from <see cref="Console.CursorLeft"/> (centers the title)</param>
        /// <returns><see cref="Tuple{T1, T2}"/> T1 = username, T2 = password</returns>
        private static async Task<Tuple<string, string>> CreateUsernameAndPassword(string space)
        {
            string baseScreen = $"{dl}\n\n{space}Sign Up\n\n{dl}\n\nCreate Your Username: \n\nCreate Your Password: \n\nConfirm Your Password: \n\n{dl}\n";

            // 0 is the username field, 1 is the password field, 2 is the confirm password field
            int loc = 0;

            // vertical, horizontal
            int vt, hz;

            // username line length, password line length
            int ul = 0, pl = 0, cpl = 0;

            // username, password, confirm password
            List<char> username = new List<char>(), password = new List<char>(), cpassword = new List<char>();
            string u = "", p = "", cp = "";

            bool clearUsernameField = false, clearPasswordField = false;

            while (true) 
            {
                // reset screen
                Console.Clear();
                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine(baseScreen);
                Console.SetCursorPosition(22, 6);
                Arcade.ConsoleColors.Set("magenta");

                // clear variables and populate fields with values from previous iterations
                if (!clearUsernameField)
                {
                    Console.SetCursorPosition(22, 6);
                    Console.Write(u);

                    // set to empty password field
                    Console.SetCursorPosition(22, 8);
                    loc = 1;
                }
                else
                {
                    u = "";
                    ul = 0;
                    username.Clear();
                }

                if (!clearPasswordField)
                {
                    Console.SetCursorPosition(22, 8);
                    Console.Write(p);
                    Console.SetCursorPosition(23, 10);
                    Console.Write(cp);

                    // set to empty username field
                    Console.SetCursorPosition(22, 6);
                    loc = 0;
                }
                else
                {
                    p = "";
                    cp = "";
                    pl = 0;
                    cpl = 0;
                    password.Clear();
                    cpassword.Clear();
                }

                clearUsernameField = false; clearPasswordField = false;

                while (true)
                {
                    ConsoleKeyInfo Key = Console.ReadKey();
                    char kc = Key.KeyChar;
                
                    if (loc == 0) username.Add(kc);
                    else if (loc == 1) password.Add(kc);
                    else cpassword.Add(kc);

                    ConsoleKey key = Key.Key;

                    (hz, vt) = Console.GetCursorPosition();

                    if (key == ConsoleKey.Enter && loc == 2)
                    {
                        if (ul > 0 && pl > 0 && cpl > 0)
                            break;
                        else if (ul <= 0)
                        {
                            Console.SetCursorPosition(22, 6);
                            loc = 0;
                            continue;
                        }
                        else if (pl <= 0)
                        {
                            Console.SetCursorPosition(22, 8);
                            loc = 1;
                            continue;
                        }
                        else
                            Console.SetCursorPosition(23, 10);
                    }

                    switch (key)
                    {
                        case ConsoleKey.Spacebar:
                            if (hz >= (22 + (loc == 0 ? ul : loc == 1 ? pl : cpl)))
                            {
                                Console.SetCursorPosition(hz - 1, vt);
                                Console.Write("");
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            // can only move up when the user is on the password field
                            if (loc == 0)
                                break;
                            else if (loc == 1)
                                Console.SetCursorPosition(22 + ul, vt - 2);
                            else
                                Console.SetCursorPosition(22 + pl, vt - 2);
                            loc -= 1;
                            break;

                        case ConsoleKey.Enter:
                        case ConsoleKey.DownArrow:
                            // can only move down when the user is on the username field
                            if (loc == 2)
                                break;
                            else if (loc == 1)
                                Console.SetCursorPosition(23 + cpl, vt + 2);
                            else
                                Console.SetCursorPosition(22 + pl, vt + 2);
                            loc += 1;
                            break;

                        case ConsoleKey.LeftArrow:
                            if (loc == 2 && hz == 23) break;
                            else if (loc != 2 && hz == 22) break;

                            Console.SetCursorPosition(hz - 1, vt);
                            break;

                        case ConsoleKey.RightArrow:
                            if (loc == 0)
                            {
                                if (hz == 22 + ul) break;
                            }
                            else if (loc == 1)
                            {
                                if (hz == 22 + pl) break;
                            }
                            else
                            {
                                if (hz == 23 + cpl) break;
                            }

                            Console.SetCursorPosition(hz + 1, vt);
                            break;

                        case ConsoleKey.Backspace:
                            Console.SetCursorPosition(22 + (loc == 2 ? 1 : 0), vt);
                            for (int i = 0; i < (loc == 0 ? ul : loc == 1 ? pl : cpl); i++)
                                Console.Write(" ");

                            if (loc == 0)
                            {
                                username.Clear();
                                ul = 0;
                            }
                            else if (loc == 1)
                            {
                                password.Clear();
                                pl = 0;
                            }
                            else
                            {
                                cpassword.Clear();
                                cpl = 0;
                            }

                            Console.SetCursorPosition(22 + (loc == 2 ? 1 : 0), vt);
                            break;

                        case ConsoleKey.End:
                            Console.SetCursorPosition(22 + (loc == 2 ? 1 : 0) + (loc == 0 ? ul : loc == 1 ? pl : cpl), vt);
                            break;

                        case ConsoleKey.Home:
                            Console.SetCursorPosition(22 + (loc == 2 ? 1 : 0), vt);
                            break;

                        // count chars on each line
                        default: // todo for both this function and the GetUsernameAndPassword function, make it so the user can't type more than 32 chars for any field; can be done by modifying the default case below
                            if (Console.GetCursorPosition().Left <= (22 + (loc == 2 ? 1 : 0) + (loc == 0 ? ul : loc == 1 ? pl : cpl))) break;
                            if (loc == 0) ul++;
                            else if (loc == 1) pl++;
                            else cpl++;
                            break;
                    }
                }

                u = ""; p = ""; cp = "";
                for (int i = 0; i < username.Count; i++)
                {
                    if ((int)username[i] != 13)
                        u += username[i];
                }
                for (int i = 0; i < password.Count; i++)
                {
                    if ((int)password[i] != 13)
                        p += password[i];
                }
                for (int i = 0; i < cpassword.Count; i++)
                {
                    if ((int)cpassword[i] != 13)
                        cp += cpassword[i];
                }

                Console.SetCursorPosition(0, vt + 4);

                /* check password */

                string error = "";
                if (p != cp)
                    error = "Passwords must match";
                else if (p.Length < 8)
                    error = "Your password must be 8 characters or longer";

                if (error != "")
                {
                    Arcade.ConsoleColors.Set("darkred");
                    Console.Write(error);
                    Arcade.ConsoleColors.Set("cyan");
                    Console.Write($"\n\n{dl}\n\n");

                    clearPasswordField = true;
                    Thread.Sleep(1000);
                    continue;
                }

                /* check username */

                DotAnimation dotAnimation = new DotAnimation(message: "Checking Username Availability", messageConsoleColor: "cyan");
                bool available = await Backend.CheckUsernameAvailability(u);
                if (!available)
                {
                    Thread.Sleep(500);
                    dotAnimation.End(endMessage: "Not Available", endMessageConsoleColor: "darkred");
                    Console.WriteLine($"\n{dl}\n");
                    Thread.Sleep(500);

                    clearUsernameField = true;
                    continue;
                }
                else
                {
                    Thread.Sleep(500);
                    dotAnimation.End(endMessage: "Available", endMessageConsoleColor: "magenta");
                    Console.WriteLine($"\n{dl}\n");
                    Thread.Sleep(500);

                    break;
                }
            }
            return new Tuple<string, string>(u.Trim(' '), p.Trim(' '));
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

            int option = -1;
            bool passed;

            do
            {
                Thread.Sleep(15);
                Console.Clear();

                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine(dl + "\n");
                
                Console.Write("You must either ");                                             Arcade.ConsoleColors.Set("magenta");
                Console.Write("SIGN IN ");                                                     Arcade.ConsoleColors.Set("cyan");
                Console.Write("or ");                                                          Arcade.ConsoleColors.Set("magenta");
                Console.Write("SIGN UP ");                                                     Arcade.ConsoleColors.Set("cyan");
                Console.Write("to play this game!\n\nType ");                                  Arcade.ConsoleColors.Set("magenta");
                Console.Write("1 ");                                                           Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine("to Sign-In to an already existing account.");
                Console.Write("Type ");                                                        Arcade.ConsoleColors.Set("magenta");
                Console.Write("2 ");                                                           Arcade.ConsoleColors.Set("cyan");
                Console.Write($"to Sign-Up for an Arcade account. Choose an option:\n\n{dl}"); Arcade.ConsoleColors.Set("magenta");

                Console.SetCursorPosition(59, 5);

                ConsoleKey response = Console.ReadKey().Key;
                if (response == ConsoleKey.D1)
                {
                    passed = true;
                    option = 1;
                }
                else if (response == ConsoleKey.D2)
                {
                    passed = true;
                    option = 2;
                }
                else
                    passed = false;
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
                Console.Write($"Place Your Bet: \n\n{dl}"); Arcade.ConsoleColors.Set("magenta");
                Console.SetCursorPosition(16, 7);
                string response = Console.ReadLine().Trim(' ').ToLower();

                while (response.Contains("$"))
                    response = response.Replace("$", "");

                bool valid = int.TryParse(response, out bet);
                if (valid)
                {
                    if (bet > this.balance)
                    {
                        error = "You can't afford this bet!";
                    }
                    else if (bet <= 0)
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
            Console.WriteLine($"${bet}\n");

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

