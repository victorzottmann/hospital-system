using System;
using System.Collections.Generic;

namespace HospitalSystem.Users
{
    public class Patient : User
    {
        private Doctor AssignedDoctor { get; }
        private int PatientID = 10000;

        private List<Doctor> RegisteredDoctors { get; } = new List<Doctor>();

        private List<string> Appointments { get; } = new List<string>()
        {
            "cold symptoms",
            "regular checkup with doc"
        };

        public Patient(Doctor doctor, string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            AssignedDoctor = doctor;
            RegisteredDoctors.Add(doctor);
            PatientID++;
        }

        public void RegisterDoctor(Doctor doctor)
        {
            RegisteredDoctors.Add(doctor);
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

        public void ListAllAppointments()
        {
            Console.Clear();

            Menu patientAppointments = new Menu();
            patientAppointments.Subtitle("My Appointments");

            Doctor doctor = AssignedDoctor;
            int doctorStrLength = doctor.ToString().Length;

            Console.WriteLine($"Appointments for {this.FirstName} {this.LastName}\n");

            Console.WriteLine("Doctor | Patient | Description");
            Console.WriteLine(new string('-', doctorStrLength));

            foreach (var appointment in this.Appointments)
            {
                Console.WriteLine(
                    $"{doctor.FirstName} {doctor.LastName} | " +
                    $"{this.FirstName} {this.LastName} | " +
                    $"{appointment}"
                );
            }

            Console.ReadKey();
        }

        public void BookAppointment()
        {
            Console.Clear();

            Menu bookAppointment = new Menu();
            bookAppointment.Subtitle("Book Appointment");

            Console.WriteLine("You are not registered with any doctor! Please choose which doctor you would like to register with\n");

            for (int i = 0; i < this.RegisteredDoctors.Count; i++)
            {
                Doctor doctor = RegisteredDoctors[i];
                Console.WriteLine($"{i+1}. {doctor.FirstName} {doctor.LastName}");
            }

            Console.WriteLine("\nPlease choose a doctor:");
            string input = Console.ReadLine()!;

            bool optionSelected = int.TryParse(input, out int selection);

            if (optionSelected)
            {
                Doctor doctor = RegisteredDoctors[selection - 1];
                Console.WriteLine($"\nYou are booking an appointment with {doctor.FirstName} {doctor.LastName}");
                
                Console.Write("Description of the appointment: ");
                string description = Console.ReadLine()!;

                if (description != null)
                {
                    Console.WriteLine("The appointment was booked successfully");
                }
                /*
                else
                {
                    prompt the user to input it again 
                }
                */

                Console.WriteLine("Press any key to return to the Patient Menu:");
            }

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
                case "3":
                    ListAllAppointments();
                    break;
                case "4":
                    BookAppointment();
                    break;
            }
        }
    }
}