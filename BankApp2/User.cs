using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2
{
    internal class User
    {
        string username;
        string password;
        float money;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public float Money
        {
            get { return money; }
            set { money = value; }
        }

    }
}
