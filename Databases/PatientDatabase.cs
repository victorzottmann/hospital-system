using System;
using System.Collections.Generic;
using HospitalSystem.Users;

namespace HospitalSystem.Databases
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

        public static void LoadPatientDB(string filepath)
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
                        string address = arr[5];

                        try
                        {
                            Patient patient = new Patient(firstName, lastName, email, phone, address);

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
            // NEED TO COMPLETE FOR INFO DOCTOR
            Console.Clear();

            Menu adminMenu = new Menu();
            adminMenu.Subtitle("All Patients");

            Console.WriteLine("All patients registered to the DOTNET Hospital Management System\n");

            if (patientDB.Count > 0)
            {             
                Console.WriteLine("Patient | Email Address | Phone | Address");
                Console.WriteLine("------------------------------------------------------------------------");

                foreach (var kvp in patientDB)
                {
                    var patient = kvp.Value;
                    Console.WriteLine(patient.ToString());
                }
            }
            else
            {
                Console.WriteLine("There are no patients registered in the system yet.");
            }
            
            Console.Write($"\nPress any key to return: ");
            Console.ReadKey();

            Administrator admin = new Administrator();
            admin.DisplayMenu();
        }

        public static void GetPatientDetails()
        {
            // NEED TO COMPLETE FOR DOCTOR INFO
            Console.Clear();

            Menu patientDetailsMenu = new Menu();
            patientDetailsMenu.Subtitle("Patient Details");

            Console.WriteLine("Please enter the ID of the patient whose details you are checking, or press 'N' to return to menu: ");
            
            string id = Console.ReadLine()!;

            try
            {
                if (id != null && patientDB.ContainsKey(int.Parse(id)))
                {
                    Patient patient = GetPatientById(int.Parse(id));

                    Console.WriteLine($"\nDetails for {patient.GetFirstName()} {patient.GetLastName()}\n");
                    Console.WriteLine("Patient | Doctor | Email Address | Phone | Address");
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.WriteLine(patient.ToString());
                }
                else
                {
                    Console.WriteLine($"\nA patient with ID {id} does not exist.");
                }
            }
            catch (NullReferenceException e)
            {
                // FIX THIS TO PROMPT THE USER AGAIN UNTIL INPUT IS CORRECT
                Console.WriteLine($"An error occured: {e.Message}");
                Console.WriteLine($"Please make sure that it is not blank.\n");
                Console.Write("Press 1 to try again or 'n' to return to the menu: ");
            }

            string key = Console.ReadLine()!;

            if (key == "n")
            {
                Administrator admin = new Administrator();
                admin.DisplayMenu();
            }
        }
    }
}
