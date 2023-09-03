using System;

namespace HospitalSystem
{
    public class Patient
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private string Phone { get; set; }
        private string Address { get; set; }

        private int PatientID = 10000;

        public Patient(string firstName, string lastName, string email, string phone, string address)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Phone = phone;
            this.Address = address;
            this.PatientID++;
        }

        public void DisplayMenu()
        {
            Console.Clear();

            AppMenu patientMenu = new AppMenu();
            patientMenu.MenuSubtitle("Patient Menu");

            string patientFullName = $"{this.FirstName} {this.LastName}";
            patientMenu.DisplayMenuOptions(patientFullName);

            // read key to close program
            Console.ReadKey();
        }

        public void ListDetails()
        {
            Console.Clear();

            AppMenu patientMenu = new AppMenu();
            patientMenu.MenuSubtitle("My Details");

            string patientFullName = $"{this.FirstName} {this.LastName}";
            Console.WriteLine($"{patientFullName}'s Details\n");

            Console.WriteLine($"Patient ID: {this.PatientID}");
            Console.WriteLine($"Full name: {this.FirstName} {this.LastName}");
            Console.WriteLine($"Address: {this.Address}");
            Console.WriteLine($"Email: {this.Email}");
            Console.WriteLine($"Phone: {this.Phone}");

            // read key to close program
            Console.ReadKey();
        }
    }
}