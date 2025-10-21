using BankApp2.Menu;
using BankApp2.Users;
using System;

namespace BankApp2.Models
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankMenu bank = new BankMenu();
            bank.ShowMenu();

        }
    }
}