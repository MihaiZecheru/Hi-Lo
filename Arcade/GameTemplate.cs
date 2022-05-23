/*
using Arcade;

namespace Template
{
    public class Game
    {
        private static string dl { get; } = "------------------------------------------------------------------------------------------------------------------------";

        public static async Task Main(string[] args)
        {
            double bet = await Initialize();

            StartGame(bet);
        }

        private static void ShowHelpMessage()
        {
            string helpMessage = "";
            Arcade.ConsoleColors.Set("cyan"); Console.WriteLine($"{dl}\n\n{helpMessage}\n{dl}");
            Arcade.ConsoleColors.Set("white"); Console.ReadLine().Trim(' ');
            Arcade.ConsoleColors.Set("cyan"); Console.Clear();
        }

        private static async Task<double> Initialize()
        {
            Console.Title = "Template";

            ShowHelpMessage();

            User user = await Arcade.User.SignIn();

            double bet = await user.GetBet();
            Console.Clear();
            Arcade.ConsoleColors.Reset();
            return bet;
        }

        public static void StartGame(double bet)
        {
            
        }
    }
}
*/