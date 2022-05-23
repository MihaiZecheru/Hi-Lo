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
        public string Model { get; set; }

        public Card()
        {
            int number;
            do
            {
                Random random = new Random();
                number = random.Next(1, 15); // cards between 1 and 14 inclusive
            } while (number != 11);

            

        }
    }
}
