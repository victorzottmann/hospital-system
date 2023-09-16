﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace HospitalSystem.Users
{
    public class Doctor : User
    {
        // the ? removes the CS8625 error from the constructor
        // but isn't there a better alternative than saying AssignedPatient can be nullable?
        private int DoctorID = 20000;
        private static string _appointmentsFilePath = "appointments.txt";

        private List<Patient> AssociatedPatients { get; } = new List<Patient>();

        public Doctor()
        {
            this.DoctorID++;
        }

        public Doctor(string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            this.DoctorID++; // need to check if an ID exists before incrementing
        }

        public int GetDoctorId() => this.DoctorID;

        public void AssignPatient(Patient patient)
        {
            this.AssociatedPatients.Add(patient);
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

            string input = Console.ReadLine()!;
            ProcessSelectedOption(input);
        }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName} | {this.Email} | {this.Phone} | {this.Address}";
        }

        public void ListDoctorDetails()
        {
            Console.Clear();

            Menu doctorDetails = new Menu();
            doctorDetails.Subtitle("My Details");

            Console.WriteLine($"Name | Email Address | Phone | Address");
            Console.WriteLine(this.ToString());

            Console.Write("\n\nPress any key to the Doctor Menu: ");
            Console.ReadKey();

            DisplayMenu();
        }

        public void ListPatients()
        {
            Console.Clear();

            Menu myPatients = new Menu();
            myPatients.Subtitle("My Patients");

            Console.WriteLine("Patient | Doctor | Email Address | Phone | Address");
            Console.WriteLine("------------------------------------------------------------------------");

            foreach (var patient in this.AssociatedPatients)
            {
                Console.WriteLine($"{patient}");
            }

            Console.Write("Press any key to return to the doctor menu");
            Console.ReadKey();

            DisplayMenu();
        }

        public void ListAppointments()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("All Appointments");

            try
            {
                if (File.Exists(_appointmentsFilePath))
                {
                    string[] lines = File.ReadAllLines(_appointmentsFilePath);

                    Console.WriteLine("Doctor | Patient | Description");
                    Console.WriteLine("---------------------------------------------------");

                    bool appointmentsFound = false;

                    foreach (string line in lines)
                    {
                        string[] arr = line.Split(',');

                        if (arr.Length == 5)
                        {
                            string doctorFirstName = arr[0];
                            string doctorLastName = arr[1];
                            string patientFirstName = arr[2];
                            string patientLastName = arr[3];
                            string description = arr[4];

                            string doctorFullName = $"{doctorFirstName} {doctorLastName}";
                            string patientFullName = $"{patientFirstName} {patientLastName}";

                            if (this.FullName == doctorFullName)
                            {
                                Console.WriteLine($"{doctorFullName} | {patientFullName} | {description}");
                                appointmentsFound = true;
                            }  
                        }
                    }

                    if (!appointmentsFound)
                    {
                        Console.WriteLine("You do not have any appointments");
                    }

                    Console.Write("\nPress any key to return to the menu: ");
                    Console.ReadKey();
                    DisplayMenu();
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
        }

        public void Logout()
        {
            Login login = new Login();
            login.DisplayMenu();
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void ProcessSelectedOption(string input)
        {
            switch (input)
            {
                case "1":
                    ListDoctorDetails();
                    break;
                case "2":
                    //ListPatients();
                    break;
                case "3":
                    ListAppointments();
                    break;
                case "4":
                    //CheckPatient();
                    break;
                case "5":
                    //AppointmentsWithPatient();
                    break;
                case "6":
                    Logout();
                    break;
                case "7":
                    Exit();
                    break;
                default:
                    Console.Write("\nPlease select an option between 1 and 7: ");
                    ProcessSelectedOption(Console.ReadLine()!);
                    break;
            }
        }
    }
}
