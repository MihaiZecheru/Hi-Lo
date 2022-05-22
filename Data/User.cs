using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade
{
    public class User
    {
        public int id { get; }
        public string name { get; set; }
        public string password { get; set; }
        public double balance { get; set; }

        /// <summary>
        /// Initializes the user
        /// </summary>
        /// <param name="id">
        /// Database Identifier, MUST MATCH WITH THE DATABASE.<br></br>
        /// Use <see cref="Arcade.Database.GetNextId"/> to get the proper id
        /// </param>
        /// <param name="name"><see cref="User"/>'s name</param>
        /// <param name="password"><see cref="User"/>'s password</param>
        /// <param name="balance"><see cref="User"/>'s starting balance</param>
        public User(int id, string name, string password, double balance)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.balance = balance;
        }

        /// <summary>
        /// Packages the user into a formatted string-JSON element
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            string s = "    ";
            return $"{{\n{s}\"id\": {this.id}\n{s}\"username\": \"{this.name}\"\n{s}\"password\": \"{this.password}\"\n{s}\"balance\": {this.balance}\n}}";
        }

        /// <summary>
        /// Prints the <see cref="User"/> to the console as a formatted string-JSON element 
        /// </summary>
        public void Print()
        {
            Console.WriteLine(this.ToString());
        }

        /// <summary>
        /// Get the <see cref="User"/> as a <see cref="List{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/><br></br>
        /// Used for packaging the <see cref="User"/> for updating the database
        /// </summary>
        /// <returns>The packaged <see cref="User"/> as a <see cref="List{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/></returns>
        public IEnumerable<KeyValuePair<string, string>> GetAsKVpairs()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", this.name),
                new KeyValuePair<string, string>("password", this.password),
                new KeyValuePair<string, string>("balance", this.balance.ToString()),
            };
        }

        /// <summary>
        /// Adds <paramref name="amount"/> to the user's balance, then updates the user in the database.
        /// </summary>
        /// <param name="amount">Add <paramref name="amount"/> to user</param>
        /// <returns><see cref="User"/></returns>
        public async Task<User> AddBalance(double amount)
        {
            this.balance += amount;
            return await Arcade.Database.UpdateUser(this);
        }

        /// <summary>
        /// Removes <paramref name="amount"/> from the user's balance, then updates the user in the database.
        /// </summary>
        /// <param name="amount">Remove <paramref name="amount"/> from user</param>
        /// <returns><see cref="User"/></returns>
        public async Task<User> RemoveBalance(double amount)
        {
            this.balance -= amount;
            return await Arcade.Database.UpdateUser(this);
        }
    }
}
