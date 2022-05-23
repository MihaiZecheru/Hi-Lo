namespace Arcade
{
    /// <summary>
    /// Loading animation with dots ...
    /// </summary>
    public class DotAnimation
    {
        private bool END { get; set; }
        private string ENDCOLOR { get; set; }
        public string ENDMESSAGE { get; set; }
        private string MESSAGE { get; set; }
        private string MESSAGECOLOR { get; set; }

        /// <summary>
        /// Takes the given <paramref name="message"/> and appends periods (.) to the end to create a loading animation
        /// </summary>
        /// <param name="message">The initial <paramref name="message"/> to animate</param>
        /// <param name="endMessage">The message to display when <see cref="DotAnimation.End"/> is called</param>
        /// <param name="messageConsoleColor">The color of <paramref name="message"/> that's being animated</param>
        /// <param name="endMessageConsoleColor">The color of the <see cref="DotAnimation.ENDMESSAGE"/> at the end of the animation</param>
        public DotAnimation(string message, string endMessage = null, string messageConsoleColor = null, string endMessageConsoleColor = null)
        {
            this.END = false;
            this.ENDCOLOR = endMessageConsoleColor;
            this.ENDMESSAGE = endMessage;
            this.MESSAGECOLOR = messageConsoleColor;
            this.MESSAGE = message;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Console.CursorVisible = false;

                if (this.MESSAGECOLOR != null)
                    Arcade.ConsoleColors.Set(this.MESSAGECOLOR);

                Console.Write(this.MESSAGE);
                
                while (true)
                {
                    if (END) return;
                    Console.Write($"\r{this.MESSAGE} .  ");
                    if (END) return;
                    Thread.Sleep(350);
                    if (END) return;
                    Console.Write($"\r{this.MESSAGE} .. ");
                    if (END) return;
                    Thread.Sleep(350);
                    if (END) return;
                    Console.Write($"\r{this.MESSAGE} ...");
                    if (END) return;
                    Thread.Sleep(350);
                }
            }).Start();
        }

        /// <summary>
        /// Ends this <see cref="DotAnimation"/>
        /// </summary>
        /// <param name="endMessage">The phrase to append to the end of the <paramref name="message"/> when <see cref="DotAnimation.End"/> is called</param>
        /// <param name="endMessageConsoleColor">The color of the <see cref="DotAnimation.ENDMESSAGE"/> in the animation</param
        public void End(string endMessage = null, string endMessageConsoleColor = null)
        {
            this.END = true;
            Console.CursorVisible = true;

            Console.Write($"\r{this.MESSAGE} ... ");

            if (endMessageConsoleColor != null)
                this.ENDCOLOR = endMessageConsoleColor;

            if (endMessage != null)
                this.ENDMESSAGE = endMessage;
            if (this.ENDMESSAGE == null)
                throw new Exception("No <endMessage> given. Call End() or DotAnimation() with an <endMessage>, or manually change the <endMessage> property with myDotAnimation.ENDMESSAGE = \"my end message\"");

            if (this.ENDCOLOR != null)
                Arcade.ConsoleColors.Set(this.ENDCOLOR);
            Console.WriteLine(this.ENDMESSAGE);
            Arcade.ConsoleColors.Set(this.MESSAGECOLOR); // reset
        }
    }
}