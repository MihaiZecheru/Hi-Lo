using Arcade;

namespace HiLo
{
    public class Game
    {
        private static string dl { get; } = "------------------------------------------------------------------------------------------------------------------------";

        public static async Task Main(string[] args)
        {
            Initialize();
        }

        private static void ShowHelpMessage()
        {
            string helpMessage = "Hi-Lo (pronounced \"High-Low\") is a game where you try to guess if the card succeeding the current card will be HIGHER\nor LOWER than your current card" +
            "\n\nPlaying Card Points:\nFace cards hold face value, Ace = 1, Jack = 12, Queen = 13, King = 14\n";
            Arcade.ConsoleColors.Reset(); Console.WriteLine($"{dl}\n\n{helpMessage}\n{dl}");
            Arcade.ConsoleColors.Set("white"); Console.ReadLine().Trim(' ');
            Arcade.ConsoleColors.Reset(); Console.Clear();
        }

        private static async Task<double> Initialize()
        {
            Console.Title = "Arcade: Hi-Lo";

            Arcade.ConsoleColors.ChangeDefaultColor("cyan");
            ShowHelpMessage();

            User user = await Arcade.User.SignIn();

            double bet = await user.GetBet();
            Console.Clear();
            return bet;
        }
    }   
}