using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade.HiLo
{
    internal class Card
    {
        public string Name { get; set; }
        public int Value { get; set; }

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

        private string[] model { get; set; }
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

        public Card()
        {
            int number;
            do
            {
                Random random = new Random();
                number = random.Next(1, 15); // cards between 1 and 14 inclusive
            } while (number == 11);

            this.Value = number;
            this.Name = Card.translate[number];
            this.model = File.ReadAllLines(@$"cards\{this.Name}.card");
        }
    }
}

