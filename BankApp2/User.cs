using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp2
{
    public class User
    {
        public string id;
        public string name;
        public List<Account> account = new List<Account>();

        public User(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}