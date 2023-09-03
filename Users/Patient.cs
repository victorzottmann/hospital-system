using System;
using System.Net;
using System.Numerics;

namespace HospitalSystem.Users
{
    public class Patient : User
    {
        private int PatientID = 10000;

        public Patient(string firstName, string lastName, string email, string phone, string address) : base(firstName, lastName, email, phone, address)
        {
            PatientID++;
        }

        public void DisplayMenuOptions()
        {
            Console.Clear();

            Console.WriteLine($"Welcome to DOTNET Hospital Management System Victor Zottmann\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit System");
        }

        // Maybe overload this later to display details in relation to user permissions (admin vs user)
        public void ListDetails()
        {
            Console.Clear();

            Menu patientMenu = new Menu();
            patientMenu.MenuSubtitle("My Details");

            Console.WriteLine($"{FirstName} {LastName}'s Details\n");

            Console.WriteLine($"Patient ID: {PatientID}");
            Console.WriteLine($"Full name: {FirstName} {LastName}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Phone: {Phone}");

            // read key to close program
            Console.ReadKey();
        }
    }
}