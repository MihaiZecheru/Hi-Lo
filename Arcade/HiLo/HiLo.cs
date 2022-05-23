namespace Arcade.HiLo
{
    public class Game
    {
        /// <summary>
        /// Dotted line
        /// </summary>
        private static string dl { get; } = "------------------------------------------------------------------------------------------------------------------------";

        public static async Task Main(string[] args)
        {
            //(User user, double bet) = await Initialize();

            //StartGame(user, bet);
            StartGame(null, 5);
        }

        /// <summary>
        /// Shows the help message for Hi-Lo
        /// </summary>
        private static void ShowHelpMessage()
        {
            string helpMessage = 
@"Hi-Lo (pronounced ""High-Low"") is a game where you try to guess if the card succeeding the current card will be HIGHER
or LOWER than your current card

Playing Card Points:
Face cards hold face value, Ace = 1, Jack = 12, Queen = 13, King = 14" + "\n";
            Arcade.ConsoleColors.Set("cyan"); Console.WriteLine($"{dl}\n\n{helpMessage}\n{dl}");
            Arcade.ConsoleColors.Set("white"); Console.ReadLine().Trim(' ');
            Arcade.ConsoleColors.Set("cyan"); Console.Clear();
        }

        /// <summary>
        /// Setup the game by signing the <see cref="User"/> in and getting their bet
        /// </summary>
        /// <returns><see cref="Tuple{T1, T2}"/> containing the <see cref="User"/> and the <see cref="User"/>'s bet on the game</returns>
        private static async Task<Tuple<User, double>> Initialize()
        {
            Console.Title = "Arcade: Hi-Lo";

            ShowHelpMessage();

            User user = await Arcade.User.SignIn();

            double bet = await user.GetBet();
            Console.Clear();
            Arcade.ConsoleColors.Reset();
            return new Tuple<User, double>(user, bet);
        }

        /// <summary>
        /// The Hi-Lo Game
        /// </summary>
        /// <param name="bet">The <see cref="User"/>'s bet</param>
        public static void StartGame(User user, double bet)
        {
            Card card = new Card();

            string response;
            while (true)
            {
                Console.Clear();
                Arcade.ConsoleColors.Set("cyan");
                Console.WriteLine($"{dl}\n\n{card.Model}\n{dl}\n");
                Console.Write("Will the next card be "); Arcade.ConsoleColors.Set("magenta");
                Console.Write("HIGHER "); Arcade.ConsoleColors.Set("cyan");
                Console.Write("or "); Arcade.ConsoleColors.Set("magenta");
                Console.Write("LOWER"); Arcade.ConsoleColors.Set("cyan");
                Console.Write(": "); Arcade.ConsoleColors.Set("magenta");
                response = Console.ReadLine().Trim(' ').ToLower();
                Arcade.ConsoleColors.Set("cyan");

                if (new string[] { "higher", "lower", "less", "more", "low", "high", "l", "h" }.Contains(response)) break;
            }

            int previousCardValue = card.Value;
            
            card = new Card(not: previousCardValue);

            // Display next card
            Console.Clear();
            Console.WriteLine($"{dl}\n\n\n\n\n\n\n\n\n\n\n\n{dl}\n");
            Arcade.DotAnimation dotAnimation = new Arcade.DotAnimation(message: "\t\t\t\t\t\t The next card is", messageConsoleColor: "cyan", endMessageConsoleColor: "magenta", endMessage: "");
            Thread.Sleep(2000);
            dotAnimation.End();
            Console.Clear();
            Console.WriteLine($"{dl}\n\n{card.Model}\n{dl}\n");
            Console.Write("\t\t\t\t\t\t The next card is: ");
            Arcade.ConsoleColors.Set("magenta");
            Console.WriteLine($"{card.Name}\n");

            bool wins;

            // LOWER
            if (card.Value < previousCardValue)
            {
                // check if user was right
                if (new string[] { "lower", "less", "low", "l" }.Contains(response))
                {
                    // user is right, user wins
                    wins = true;
                }
                else
                {
                    // user is wrong, user loses
                    // todo
                    wins = false;
                }
            }
            // HIGHER
            else
            {
                if (new string[] { "higher", "more", "high", "h" }.Contains(response))
                {
                    // user is right, user wins
                    wins = true;
                }
                else
                {
                    // user is wrong, user loses
                    wins = false;
                }
            }

            Console.Write("\t\t\t\t\t\t      ");
            if (wins)
            {
                Arcade.ConsoleColors.Set("green");
                Console.Write(" YOU WIN!!!");

                // todo add more here
            }
            else
            {
                Arcade.ConsoleColors.Set("darkred");
                Console.Write("YOU LOSE!!!");

                // todo add more here
            }

            Arcade.ConsoleColors.Set("cyan");
        }
    }
}