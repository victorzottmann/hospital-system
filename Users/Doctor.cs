﻿using HospitalSystem.Databases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Numerics;

namespace HospitalSystem.Users
{
    public class Doctor : User
    {
        // the ? removes the CS8625 error from the constructor
        // but isn't there a better alternative than saying AssignedPatient can be nullable?
        private int DoctorID { get; set; }
        private static string _appointmentsFilePath = "appointments.txt";
        private static string _doctorPatientsFilePath = "doctor-patients.txt";

        //private List<Patient> AssociatedPatients { get; }
        private Dictionary<Doctor, List<Patient>> AssociatedPatients { get; }

        public Doctor()
        {
            this.AssociatedPatients = new Dictionary<Doctor,List<Patient>>();
        }

        public Doctor(int id, string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            this.DoctorID = id;
            this.AssociatedPatients = new Dictionary<Doctor,List<Patient>>();
        }

        public int GetDoctorId() => this.DoctorID;

        public void AssignPatient(Doctor doctor, Patient patient)
        {
            if (!AssociatedPatients.ContainsKey(doctor))
            {
                AssociatedPatients[doctor] = new List<Patient>();
            }

            AssociatedPatients[doctor].Add(patient);
        }

        public void WriteToFile(string filepath, Doctor doctor, Patient patient)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    Console.WriteLine(lines.Length);

                    string doctorId = doctor.GetDoctorId().ToString();
                    string doctorFirstName = doctor.FirstName;
                    string doctorLastName = doctor.LastName;
                    string patientId = patient.GetPatientId().ToString();
                    string patientFirstName = patient.GetFirstName();
                    string patientLastName = patient.GetLastName();

                    int insertionIndex = -1;

                    /*
                     * For loop:
                     *      Starting from the last index (last line in the file),
                     *      it iterates through each line from the last line to the first line
                     *      until it reaches the first line (i >= 0)
                     * If Statement:
                     *      If a line starts with the target doctorId,
                     *      set the insertionIndex of the new line to i
                     *      Then break out of the loop once it assigns i to insertionIndex
                     *      of the doctorId
                     *      
                     *      NOTE: it will be appended to the bottom portion of the same doctorId because 
                     *      it's iterating in reverse order
                     * 
                     * NOTE: it must be lines.Length - 1 because line 1 is actually line 0 in terms of Arrays
                     * So, if a file has 27 lines, it's not that it goes from 1-27, but from 0-26
                     */
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        if (lines[i].StartsWith(doctorId))
                        {
                            insertionIndex = i;
                            break;
                        }
                    }

                    if (insertionIndex != -1)
                    {
                        string textToFile =
                            $"{doctorId}," +
                            $"{doctorFirstName}," +
                            $"{doctorLastName}," +
                            $"{patientId}," +
                            $"{patientFirstName}," +
                            $"{patientLastName}"
                        ;

                        // initialising the list with the elements of the lines array
                        List<string> updatedLines = new List<string>(lines);

                        /*
                         * Here it's necessary to add 1 to insertionIndex because, unlike an array,
                         * a file starts at line 1, not 0.
                         * 
                         * So, if you consider that the indexes of an array go from 0-26,
                         * and a text file actually goes from 1-27, you need to add 1 to the insertion index
                         * in order to correctly append the new line relative to the target doctorId.
                         */
                        updatedLines.Insert(insertionIndex + 1, textToFile);

                        File.WriteAllLines(_doctorPatientsFilePath, updatedLines);
                    }
                    else
                    {
                        Console.WriteLine($"Doctor with ID {doctorId} not found");
                    }
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
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
            Console.WriteLine("7. Exit\n");

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
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine(this.ToString());

            Console.Write("\nPress any key to the Doctor Menu: ");
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

            foreach (var kvp in AssociatedPatients)
            {
                Doctor doctor = kvp.Key;
                List<Patient> patients = kvp.Value;

                string doctorFullName = $"{doctor.FirstName} {doctor.LastName}";

                foreach (var patient in patients)
                {
                    string patientFullName = $"{patient.GetFirstName()} {patient.GetLastName()}";
                    string patientEmail = patient.GetEmail();
                    string patientPhone = patient.GetPhone();
                    string patientAddress = patient.GetAddress();

                    Console.WriteLine($"{patientFullName} | {doctorFullName} | {patientEmail} | {patientPhone} | {patientAddress}");
                }
            }

            Console.Write("\nPress any key to return to the doctor menu: ");
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

                        if (arr.Length > 0)
                        {
                            string doctorFirstName = arr[1];
                            string doctorLastName = arr[2];
                            string patientFirstName = arr[4];
                            string patientLastName = arr[5];
                            string description = arr[6];

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

        public void CheckPatient()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("Check Patient Details");

            Console.Write("Enter the ID of the patient to check: ");
            string id = Console.ReadLine()!.Trim();

            try
            {
                if (int.TryParse(id, out int patientId))
                {
                    Patient patient = PatientDatabase.GetPatientById(patientId);

                    if (patient != null)
                    {
                        bool patientFound = false;

                        Console.WriteLine("\nPatient | Doctor | Email Address | Phone | Address");
                        Console.WriteLine("-------------------------------------------------------------------------");

                        foreach (var kvp in AssociatedPatients)
                        {
                            if (kvp.Value.Contains(patient))
                            {
                                patientFound = true;

                                string docFullName = this.FullName;
                                string patFullName = patient.GetFullName();
                                string patEmail = patient.GetEmail();
                                string patPhone = patient.GetPhone();
                                string patAddress = patient.GetAddress();

                                Console.WriteLine($"{patFullName} | {docFullName} | {patEmail} | {patPhone} | {patAddress}");
                                break;
                            }
                        }

                        if (!patientFound)
                        {
                            Console.WriteLine($"\nPatient with ID {id} does not seem to be associated with you");
                        }

                        Console.Write("\nPress 'N' to return to the Doctor Menu: ");
                        Console.ReadKey();

                        DisplayMenu();

                    }

                    Console.Write("\nPress 'N' to return to the Doctor Menu: ");
                    Console.ReadKey();
                    DisplayMenu();
                }
                else
                {
                    Console.WriteLine("\nInvalid ID. Only numeric values are accepted.");
                    Console.Write("\nPress 1 to try again or 'N' to return to the Doctor Menu: ");
                    string key;

                    while (true)
                    {
                        key = Console.ReadLine()!;

                        if (key == "1")
                        {
                            CheckPatient();
                        }
                        else if (key == "n")
                        {
                            break;
                        }
                    }

                    DisplayMenu();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        public void ListAppointmentsWithPatient()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("Appointments with Patient");

            Console.Write("Input the patient ID: ");
            string id = Console.ReadLine()!.Trim();

            try
            {
                if (File.Exists(_appointmentsFilePath))
                {
                    Patient patient = PatientDatabase.GetPatientById(int.Parse(id));

                    if (patient != null)
                    {
                        bool found = false;

                        string[] lines = File.ReadAllLines(_appointmentsFilePath);

                        Console.WriteLine("Doctor | Patient | Description");
                        Console.WriteLine("-----------------------------------------------");

                        foreach (var line in lines)
                        {
                            string[] arr = line.Split(',');

                            string doctorFullName = $"{arr[1]} {arr[2]}";
                            string patientId = arr[3];
                            string patientFullName = $"{arr[4]} {arr[5]}";
                            string description = arr[6];

                            if (patientId == id)
                            {
                                found = true;
                                Console.WriteLine($"{doctorFullName} | {patientFullName} | {description}");
                            }                        
                        }

                        if (!found)
                        {
                            Console.WriteLine("\nNo appointments found for this patient");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Patient not found. Please enter a valid ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric ID.");
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
                    ListPatients();
                    break;
                case "3":
                    ListAppointments();
                    break;
                case "4":
                    CheckPatient();
                    break;
                case "5":
                    ListAppointmentsWithPatient();
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