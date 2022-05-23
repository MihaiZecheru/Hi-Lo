using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade.HiLo
{
    internal class Card
    {
        /// <summary>
        /// The <see cref="Card"/>'s name, eg. two, king
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The <see cref="Card"/>'s value, eg. 5, 6
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Translate a <see cref="Card.Value"/> to a <see cref="Card.Name"/>
        /// </summary>
        private static Dictionary<int, string> translate { get; set; } = new Dictionary<int, string>(new List<KeyValuePair<int, string>>()
        {
            new KeyValuePair<int, string>(1, "ace"),
            new KeyValuePair<int, string>(2, "two"),
            new KeyValuePair<int, string>(3, "three"),
            new KeyValuePair<int, string>(4, "four"),
            new KeyValuePair<int, string>(5, "five"),
            new KeyValuePair<int, string>(6, "six"),
            new KeyValuePair<int, string>(7, "seven"),
            new KeyValuePair<int, string>(8, "eight"),
            new KeyValuePair<int, string>(9, "nine"),
            new KeyValuePair<int, string>(10, "ten"),
            new KeyValuePair<int, string>(12, "jack"),
            new KeyValuePair<int, string>(13, "queen"),
            new KeyValuePair<int, string>(14, "king")
        });

        /// <summary>
        /// The <see cref="Card.Model"/> in its deconstructed form
        /// </summary>
        private string[] model { get; set; }
        
        /// <summary>
        /// The full ASCII playing card
        /// </summary>
        public string Model 
        {
            get
            {
                string m = "";
                foreach (var line in model)
                    m += line + "\n";
                return m;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Card"/>
        /// </summary>
        /// <param name="not">The next card will <paramref name="not"/> be of this value</param>
        public Card(int not)
        {
            int number;
            do
            {
                Random random = new Random();
                number = random.Next(1, 15); // cards between 1 and 14 inclusive
            } while (number == 11 || number == not);

            this.Value = number;
            this.Name = Card.translate[number];
            this.model = File.ReadAllLines(@$"cards\{this.Name}.card");
        }
    }
}