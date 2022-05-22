using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade
{
    /// <summary>
    /// Change the color of <see cref="Console.ForegroundColor"/>
    /// </summary>
    public class ConsoleColors
    {
        public static ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;
        private static ConsoleColor CurrentColor { get; set; } = DefaultColor;

        private static Dictionary<string, ConsoleColor> Colors = new Dictionary<string, ConsoleColor>(new List<KeyValuePair<string, ConsoleColor>>()
        {
            new KeyValuePair<string, ConsoleColor>("black", ConsoleColor.Black),
            new KeyValuePair<string, ConsoleColor>("darkblue", ConsoleColor.DarkBlue),
            new KeyValuePair<string, ConsoleColor>("darkgreen", ConsoleColor.DarkGreen),
            new KeyValuePair<string, ConsoleColor>("darkcyan", ConsoleColor.DarkCyan),
            new KeyValuePair<string, ConsoleColor>("darkred", ConsoleColor.DarkRed),
            new KeyValuePair<string, ConsoleColor>("darkmagenta", ConsoleColor.DarkMagenta),
            new KeyValuePair<string, ConsoleColor>("darkyellow", ConsoleColor.DarkYellow),
            new KeyValuePair<string, ConsoleColor>("darkgray", ConsoleColor.DarkGray),
            new KeyValuePair<string, ConsoleColor>("gray", ConsoleColor.Gray),
            new KeyValuePair<string, ConsoleColor>("blue", ConsoleColor.Blue),
            new KeyValuePair<string, ConsoleColor>("green", ConsoleColor.Green),
            new KeyValuePair<string, ConsoleColor>("cyan", ConsoleColor.Cyan),
            new KeyValuePair<string, ConsoleColor>("red", ConsoleColor.Red),
            new KeyValuePair<string, ConsoleColor>("magenta", ConsoleColor.Magenta),
            new KeyValuePair<string, ConsoleColor>("yellow", ConsoleColor.Yellow),
            new KeyValuePair<string, ConsoleColor>("white", ConsoleColor.White)
        });

        /// <summary>
        /// Changes the <see cref="Console"/>'s text color
        /// </summary>
        public static void Set(string color)
        {
            if (!Colors.Keys.Contains(color))
                throw new Exception(color + " does not exist\n\nExisting colors: black, darkblue, darkgreen, darkcyan, darkred, darkmagenta, darkyellow, darkgray, gray, blue, green, cyan, red, magenta, yellow, white");

            if (Colors[color] == CurrentColor) return;
            ConsoleColors.CurrentColor = Colors[color];
            Console.ForegroundColor = ConsoleColors.CurrentColor;
        }

        /// <summary>
        /// Set <see cref="Console.ForegroundColor"/> back to <see cref="ConsoleColors.DefaultColor"/>
        /// </summary>
        public static void Reset()
        {
            Console.ForegroundColor = ConsoleColors.DefaultColor;
        }
    }
}
