using Arcade;

namespace HiLo
{
    public class Game
    {
        private static string dl { get; } = "------------------------------------------------------------------------------------------------------------------------";
        private static string HelpMessage { get; } = 
            "Hi-Lo (pronounced \"High-Low\") is a game where you try to guess if the card succeeding the current card will be HIGHER\nor LOWER than your current card" +
            "\n\nPlaying Card Points:\nFace cards hold face value, Ace = 1, Jack = 12, Queen = 13, King = 14\n";

        public static async Task Main(string[] args)
        {
            Console.Title = "Arcade: Hi-Lo";

            Arcade.ConsoleColors.ChangeDefaultColor("cyan"); Arcade.ConsoleColors.Reset();
            
            Console.WriteLine($"{dl}\n\n{HelpMessage}\n{dl}"); Arcade.ConsoleColors.Reset();
            
            Console.ReadLine().Trim(' ');
            Console.Clear();

            User user = await Arcade.User.SignIn();
            user.Print();
        }
    }
}