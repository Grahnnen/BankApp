using BankApp2.Menu;
using BankApp2.Users;
using System;

namespace BankApp2.Models
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // emoji support

            BankMenu bank = new BankMenu();
            bank.ShowMenu();

        }
    }
}