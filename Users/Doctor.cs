using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace HospitalSystem.Users
{
    public class Doctor : User
    {
        private int DoctorID = 20000;

        private List<Patient> AssignedPatients { get; } = new List<Patient>();

        public Doctor(string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            DoctorID++;
        }

        public List<Patient> GetAssignedPatients() => AssignedPatients;

        public void DisplayMenu()
        {
            Console.Clear();

            Menu doctorMenu = new Menu();
            doctorMenu.Subtitle("Doctor Menu");

            Console.WriteLine($"Welcome to DOTNET Hospital Management {this.FirstName} {this.LastName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List doctor details");
            Console.WriteLine("2. List patients");
            Console.WriteLine("3. List appointments");
            Console.WriteLine("4. Check particular patient");
            Console.WriteLine("5. List appointments with patient");
            Console.WriteLine("6. Logout");
            Console.WriteLine("7. Exit");

            Console.Read();
        }

        public void AssignPatient(Patient patient)
        {
            AssignedPatients.Add(patient);
        }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName} | {this.Email} | {this.Phone} | {this.Address}";
        }
    }
}
