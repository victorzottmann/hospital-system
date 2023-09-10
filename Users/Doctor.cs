using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace HospitalSystem.Users
{
    public class Doctor : User
    {
        // the ? removes the CS8625 error from the constructor
        // but isn't there a better alternative than saying AssignedPatient can be nullable?
        private Patient? AssignedPatient { get; set; }
        private int DoctorID = 20000;

        public Doctor()
        {
            this.AssignedPatient = null;
            this.DoctorID++;
        }

        public Doctor(string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            this.AssignedPatient = null;
            this.DoctorID++;
        }

        public int GetDoctorId() => this.DoctorID;

        public void AssignPatient(Patient patient)
        {
            this.AssignedPatient = patient;
        }

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

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName} | {this.Email} | {this.Phone} | {this.Address}";
        }
    }
}
