// Replace all occurances of the word "Template" with the name of the game

/*
namespace Arcade.Template
{
    public class Game
    {
        /// <summary>
        /// The name of the <see cref="Game"/>
        /// </summary>
        private static string Name { get; } = "Template";

        /// <summary>
        /// Dotted line the size of <see cref="Console.BufferWidth"/> (120)
        /// </summary>
        private static string dl { get; } = "------------------------------------------------------------------------------------------------------------------------";

        public static async Task Main(string[] args)
        {
            (User user, double bet) = await Initialize();

            while (true)
            {
                Console.Clear();
                bool continueGame = StartGame(user, bet);
                if (!continueGame) break;
            }

            ConsoleColors.Set("cyan");
            Backend.Show_WhereToFindLeaderboardsMessage();
        }

        /// <summary>
        /// Shows the help message for Template
        /// </summary>
        private static void ShowHelpMessage()
        {
            string helpMessage = @"Template HelpMessage";
            Arcade.ConsoleColors.Set("cyan"); Console.WriteLine($"{dl}\n\n{helpMessage}\n{dl}");
            Arcade.ConsoleColors.Set("white"); Console.ReadKey();
            Arcade.ConsoleColors.Set("cyan"); Console.Clear();
        }

        /// <summary>
        /// Setup the game by signing the <see cref="User"/> in and getting their bet
        /// </summary>
        /// <returns><see cref="Tuple{T1, T2}"/> containing the <see cref="User"/> and the <see cref="User"/>'s bet on the game</returns>
        private static async Task<Tuple<User, double>> Initialize()
        {
            Console.Title = "Arcade: Template";

            ShowHelpMessage();

            User user = await Arcade.User.SignIn();

            double bet = await user.GetBet();
            Console.Clear();
            Arcade.ConsoleColors.Reset();
            return new Tuple<User, double>(user, bet);
        }

        /// <summary>
        /// The Template Game
        /// </summary>
        /// <param name="bet">The <see cref="User"/>'s bet</param>
        public static bool StartGame(User user, double bet)
        {
            // the user's winning status
            bool won = false;

            Thread.Sleep(1000);
            Console.Clear();

            return Backend.EndGameScene(won, bet, user.balance, Game.Name);
        }
    }
}
*/