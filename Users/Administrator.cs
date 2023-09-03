using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Users
{
    public class Administrator : User
    {
        private int AdministratorID = 30000;

        public Administrator(string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            AdministratorID++;
        }

        public void DisplayMenu()
        {
            Console.Clear();

            Menu adminMenu = new Menu();
            adminMenu.Subtitle("Administrator Menu");

            Console.WriteLine($"Welcome to DOTNET Hospital Management {FirstName} {LastName}");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List all doctors");
            Console.WriteLine("2. Check doctor details");
            Console.WriteLine("3. List all patients");
            Console.WriteLine("4. Check patient details");
            Console.WriteLine("5. Add doctor");
            Console.WriteLine("6. Add patient");
            Console.WriteLine("7. Logout");
            Console.WriteLine("8. Exit");

            Console.ReadKey();
        }
    }
}
