namespace Arcade
{
    /// <summary>
    /// Loading animation with dots ...
    /// </summary>
    internal class DotAnimation
    {
        private bool END { get; set; }
        private string MESSAGE { get; set; }
        public string ENDMESSAGE { get; set; }

        /// <summary>
        /// Takes the given <paramref name="message"/> and appends periods (.) to the end to create a loading animation
        /// </summary>
        /// <param name="message">The initial <paramref name="message"/> to animate</param>
        /// <param name="endMessage">The message to display when <see cref="DotAnimation.End"/> is called</param>
        /// <param name="consoleColor">The color of <paramref name="endMessage"/> in the animation</param>
        public DotAnimation(string message, string endMessage = null, string consoleColor = null)
        {
            this.ENDMESSAGE = endMessage;
            this.MESSAGE = message;
            this.END = false;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                if (consoleColor != null)
                    Arcade.ConsoleColors.Set(consoleColor);

                Console.Write(this.MESSAGE);
                while (true)
                {
                    Console.CursorVisible = false;
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
        /// <param name="consoleColor">The color of the <paramref name="endMessage"/> in the animation</param>
        public void End(string endMessage, string consoleColor = null)
        {
            this.END = true;
            Console.CursorVisible = true;

            Console.Write($"\r{this.MESSAGE} ... ");

            if (consoleColor != null)
                Arcade.ConsoleColors.Set(consoleColor);

            Console.WriteLine(endMessage);
        }

        /// <summary>
        /// Ends this <see cref="DotAnimation"/>
        /// </summary>
        /// <param name="consoleColor">The color of the <see cref="DotAnimation.ENDMESSAGE"/> in the animation</param>
        public void End(string consoleColor = null)
        {
            this.END = true;
            Console.CursorVisible = true;

            if (this.ENDMESSAGE == null)
                throw new Exception("No <endMessage> given. Call End() with an <endMessage>, or manually change the <endMessage> property with myDotAnimation.ENDMESSAGE = \"my end message\"");

            Console.Write($"\r{this.MESSAGE} ... ");

            if (consoleColor != null)
                Arcade.ConsoleColors.Set(consoleColor);

            Console.WriteLine(this.ENDMESSAGE);
        }
    }
}