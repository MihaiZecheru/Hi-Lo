// Replace all occurances of the word "Template" with the name of the game

/*
namespace Arcade.Template
{
    public class Game
    {
        /// <summary>
        /// Dotted line
        /// </summary>
        private static string dl { get; } = "------------------------------------------------------------------------------------------------------------------------";

        public static async Task Main(string[] args)
        {
            (User user, double bet) = await Initialize();
            Arcade.ConsoleColors.Set("white");
            StartGame(user, bet);
        }

        /// <summary>
        /// Shows the help message for Template
        /// </summary>
        private static void ShowHelpMessage()
        {
            string helpMessage = "Template HelpMessage";
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
        public static void StartGame(User user, double bet)
        {
            
        }
    }
}
*/