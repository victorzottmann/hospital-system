﻿using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public class PatientDatabase
    {
        // static dictionary to allow other classes to find patients registered in the system
        private static Dictionary<int, Patient> patientDB = new Dictionary<int, Patient>();

        public static void AddPatient(int id, Patient patient)
        {
            // key: PatientID, value: Patient obj
            patientDB.Add(id, patient);
        }

        public static void LoadDB(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    foreach (var line in lines)
                    {
                        string[] arr = line.Split(',');

                        int patientId = int.Parse(arr[0]);
                        string firstName = arr[1];
                        string lastName = arr[2];
                        string email = arr[3];
                        string phone = arr[4];
                        string streetNumber = arr[5];
                        string street = arr[6];
                        string city = arr[7];
                        string state = arr[8];

                        Address address = new Address(streetNumber, street, city, state);

                        try
                        {
                            Patient patient = new Patient(patientId, firstName, lastName, email, phone, address.ToString());

                            if (!patientDB.ContainsKey(patientId))
                            {
                                patientDB.Add(patientId, patient);
                            }
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"An error occured: {e.Message}");
                            Console.Write("Press any key to continue: ");
                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"File not found: {filepath}");
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static Dictionary<int, Patient> GetPatientDatabase()
        {
            return patientDB;
        }

        public static int GetPatientId(int patientId)
        {
            if (patientDB.ContainsKey(patientId))
            {
                return patientId;
            }
            else
            {
                Console.WriteLine("Patient not found. Please check that the ID is valid.");
                return -1;
            }
        }

        public static Patient GetPatientById(int patientId)
        {
            if (patientDB.ContainsKey(patientId))
            {
                return patientDB[patientId];
            }
            return null;
        }

        public static void GetPatients()
        {
            Console.Clear();

            Administrator admin = new Administrator();

            Menu menu = new Menu();
            menu.Subtitle("All Patients");

            Console.WriteLine("All patients registered to the DOTNET Hospital Management System");

            List<string> tableHeaders = new List<string>()
            {
                "Patient", "Email Address", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            if (patientDB.Count > 0)
            {
                foreach (var kvp in patientDB)
                {
                    var patient = kvp.Value;
                    tableRows.Add(patient.ToStringArray());
                }

                Utilities.FormatTable(tableHeaders.ToArray(), tableRows);
                Utilities.ReturnToMenu(admin, true);
            }
            else
            {
                Console.WriteLine("There are no patients registered in the system yet.");
                Utilities.ReturnToMenu(admin, true);
            }
        }

        public static void GetPatientDetails()
        {
            Console.Clear();

            Administrator admin = new Administrator();

            Menu menu = new Menu();
            menu.Subtitle("Patient Details");

            Console.Write("Please enter the ID of the patient whose details you are checking, or press 'N' to return to menu: ");   
            string id = Console.ReadLine()!;

            if (id == "n")
            {
                Utilities.ReturnToMenu(admin, false);
            }

            List<string> tableHeaders = new List<string>()
            {
                "Patient", "Doctor", "Email Address", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            try
            {
                if (id != null && patientDB.ContainsKey(int.Parse(id)))
                {
                    Patient patient = GetPatientById(int.Parse(id));
                    Console.WriteLine($"\nDetails for {patient.FullName}");

                    bool noPatientDoctor = patient.GetPatientDoctor() == null;
                    string doctorName = noPatientDoctor ? "Not Assigned" : patient.GetPatientDoctor().FullName;
                    
                    tableRows.Add(new string[]
                    {
                            patient.FullName,
                            doctorName,
                            patient.Email,
                            patient.Phone,
                            patient.Address
                    });

                    Utilities.FormatTable(tableHeaders.ToArray(), tableRows);

                    Utilities.TryAgainOrReturn(admin, GetPatientDetails);
                }
                else
                {
                    Console.WriteLine($"\nA patient with ID {id} does not exist.");
                    Utilities.TryAgainOrReturn(admin, GetPatientDetails);
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"\nAn error occured: {e.Message}");
                Utilities.TryAgainOrReturn(admin, GetPatientDetails);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"\nAn error occured: {e.Message}");
                Utilities.TryAgainOrReturn(admin, GetPatientDetails);
            }
        }
    }
}