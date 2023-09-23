using System;

namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // simply load the login menu to interact with the whole app
            Login login = new Login();
            login.LoginMenu();
        }
    }
}