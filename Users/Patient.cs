using System;
using System.Data;
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

        public void DisplayMenu()
        {
            Console.Clear();

            Menu patientMenu = new Menu();
            patientMenu.Subtitle("Patient Menu");

            Console.WriteLine($"Welcome to DOTNET Hospital Management {this.FirstName} {this.LastName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit system");

            string input = Console.ReadLine()!;
            ProcessSelectedOption(input);
        }

        // Maybe overload this later to display details in relation to user permissions (admin vs user)
        public void ListPatientDetails()
        {
            Console.Clear();

            Menu patientMenu = new Menu();
            patientMenu.Subtitle("My Details");

            Console.WriteLine($"{this.FirstName} {this.LastName}'s Details\n");

            Console.WriteLine($"Patient ID: {this.PatientID}");
            Console.WriteLine($"Full name: {this.FirstName} {this.LastName}");
            Console.WriteLine($"Address: {this.Address}");
            Console.WriteLine($"Email: {this.Email}");
            Console.WriteLine($"Phone: {this.Phone}");

            // read key to close program
            Console.ReadKey();
        }

        public void ProcessSelectedOption(string input)
        {
            switch (input)
            {
                case "1":
                    ListPatientDetails();
                    break;
            }
        }
    }
}