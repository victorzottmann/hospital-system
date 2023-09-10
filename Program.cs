using System;
using HospitalSystem.Users;

namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Login login = new Login();
            login.DisplayMenu();
        }
    }
}