using System;
using System.Data;
using System.Net;
using System.Numerics;

namespace HospitalSystem.Users
{
    public class Patient : User
    {
        private Doctor AssignedDoctor { get; }
        private int PatientID = 10000;

        public Patient(Doctor doctor, string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            AssignedDoctor = doctor;
            PatientID++;
        }

        public void DisplayMenu()
        {
            Console.Clear();

            Menu patientMenu = new Menu();
            patientMenu.Subtitle("Patient Menu");

            Console.WriteLine($"Welcome to DOTNET Hospital Management {FirstName} {LastName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit system\n");

            string input = Console.ReadLine()!;
            ProcessSelectedOption(input);
        }

        // Maybe overload this later to display details in relation to user permissions (admin vs user)
        public void ListPatientDetails()
        {
            Console.Clear();

            Menu patientDetails = new Menu();
            patientDetails.Subtitle("My Details");

            Console.WriteLine($"{FirstName} {LastName}'s Details\n");

            Console.WriteLine($"Patient ID: {PatientID}");
            Console.WriteLine($"Full name: {FirstName} {LastName}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Phone: {Phone}");

            Console.WriteLine("\n\nPress any key to the Patient Menu:");
            Console.ReadKey();

            DisplayMenu();
        }

        public void ListMyDoctorDetails()
        {
            Console.Clear();

            Menu patientDoctorDetails = new Menu();
            patientDoctorDetails.Subtitle("My Doctor");

            Doctor doctor = AssignedDoctor;
            int doctorStrLength = doctor.ToString().Length;

            Console.WriteLine("Your doctor:\n");
            Console.WriteLine("Name: | Email Address | Phone | Address");
            Console.WriteLine(new string('-', doctorStrLength));
            Console.WriteLine(doctor);

            Console.WriteLine("\n\nPress any key to the Patient Menu:");
            Console.ReadKey();

            DisplayMenu();
        }

        public void ProcessSelectedOption(string input)
        {
            switch (input)
            {
                case "1":
                    ListPatientDetails();
                    break;
                case "2":
                    ListMyDoctorDetails();
                    break;
            }
        }
    }
}