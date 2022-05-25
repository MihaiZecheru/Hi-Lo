using System.Media;
// todo maybe add some "chug-a chug-a chug-a chug-a sounds throughout the train_whistle.wav
namespace Arcade
{
    /// <summary>
    /// Cute ASCII train will move across the console and toot its horn halfway through the animation
    /// </summary>
    internal class TrainAnimation
    {
        /// <summary>
        /// When this is <see langword="true"/>, the <see cref="TrainAnimation"/> will end
        /// </summary>
        private static bool EndAnimation = false;

        /// <summary>
        /// ASCII train with the "You Win" message
        /// </summary>
        private static string[] WinTrain = new string[]
        {
"          __   __                         __      __  _            ",
"    o O O \\ \\ / /   ___    _  _      o O O\\ \\    / / (_)    _ _    ",
"   o       \\ V /   / _ \\  | +| |    o      \\ \\/\\/ /  | |   | ' \\   ",
"  TS__[O]  _|_|_   \\___/   \\_,_|   TS__[O]  \\_/\\_/  _|_|_  |_||_|  ",
" {======|_| \"\"\" |_|\"\"\"\"\"|_|\"\"\"\"\"| {======|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"| ",
"./o--000'\"`-0-0-'\"`-0-0-'\"`-0-0-'./o--000'\"`-0-0-'\"`-0-0-'\"`-0-0-' "
        };

        /// <summary>
        /// ASCII train with the "You Lose" message
        /// </summary>
        private static string[] LoseTrain = new string[]
        {
            "          __   __                            _                            ",
            "    o O O \\ \\ / /   ___    _  _      o O O  | |      ___     ___     ___  ",
            "   o       \\ V /   / _ \\  | +| |    o       | |__   / _ \\   (_-<    / -_) ",
            "  TS__[O]  _|_|_   \\___/   \\_,_|   TS__[O]  |____|  \\___/   /__/_   \\___| ",
            " {======|_| \"\"\" |_|\"\"\"\"\"|_|\"\"\"\"\"| {======|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"| ",
            "./o--000'\"`-0-0-'\"`-0-0-'\"`-0-0-'./o--000'\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-' "
        };

        /// <summary>
        /// Show an animation where a train will cross the console carryin the words "You Lose"<br><br></br></br>
        /// A train whistle will blow as the train becomes fully visible
        /// </summary>
        public static void ShowYouWin()
        {
            ShowTrain(Train: WinTrain, fast: false);
        }

        /// <summary>
        /// Show an animation where a train will cross the console carrying the words "You Lose"<br><br></br></br>
        /// A train whistle will blow as the train becomes fully visible
        /// </summary>
        public static void ShowYouLose()
        {
            ShowTrain(Train: LoseTrain, fast: true);
        }

        /// <summary>
        /// Shows the ASCII train animation, based on which <paramref name="Train"/> is given
        /// </summary>
        /// <param name="Train">ASCII <paramref name="Train"/></param>
        /// <param name="fast">Whether the train will go slow or <paramref name="fast"/>. NOTE: when fast = true, the shape  of the <paramref name="Train"/> may become slightly distorted on weaker machines</param>
        private static void ShowTrain(string[] Train, bool fast = true)
        {
            // todo make it so that if the user presses a key on their keyboard while the train is going the animation will skip
            int DELAY = fast ? 10 : 30;

            Console.Clear();
            Console.CursorVisible = false;

            SoundPlayer TrainWhistle = null;
            if (OperatingSystem.IsWindows())
            {
                TrainWhistle = new SoundPlayer("train_whistle.wav");
                TrainWhistle.Load();
            }

            Start_KeyEventListener();

            for (int k = Train[0].Length; k != -1 - Console.WindowWidth; k--)
            {
                if (EndAnimation) break;
                Thread.Sleep(DELAY);
                for (int i = 0; i < Train.Length; i++)
                {
                    if (EndAnimation) break;
                    for (int j = 0; j < Train[i].Length; j++)
                    {
                        int loc;
                        if (k > 0)
                            loc = j + (Console.WindowWidth - Train[i].Length) + k;
                        else
                            loc = j + (Console.WindowWidth - Train[i].Length) + k;

                        if (loc >= Console.WindowWidth) break;
                        if (loc <= 0) continue;
                        Console.SetCursorPosition(loc, i);
                        Console.Write(Train[i][j]);
                    }
                }

                // halfway through the animation sound the whistle
                if (k == Train[0].Length / 2 + (fast ? 20 : 30))
                {
                    if (OperatingSystem.IsWindows())
                        TrainWhistle?.Play();
                }
            }

            if (OperatingSystem.IsWindows())
                TrainWhistle?.Stop();
            Console.Clear();
        }

        /// <summary>
        /// Begins the KeyEventListener in charge for terminating the <see cref="TrainAnimation"/>
        /// </summary>
        private static void Start_KeyEventListener()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                ConsoleKeyInfo keyinfo;
                keyinfo = Console.ReadKey();

                EndAnimation = true;
            }).Start();
        }
    }
}
