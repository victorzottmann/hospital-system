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

                    Console.WriteLine(
                        $"{patient.GetFirstName()} | " +
                        $"{patient.GetLastName()} | " +
                        $"{patient.GetEmail()} | " +
                        $"{patient.GetAddress()}"
                    );
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

            Console.WriteLine("Please enter the ID of the patient whose details you are checking, or press n to return to menu: ");
            
            int id = Convert.ToInt32(Console.ReadLine()!);
            Patient patient = GetPatientById(id);

            Console.WriteLine($"\nDetails for {patient.GetFirstName()} {patient.GetLastName()}\n");
            Console.WriteLine("Patient | Doctor | Email Address | Phone | Address");
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine(patient.ToString());

            Console.Write($"\nPress any key to return: ");
            Console.ReadKey();
            Administrator admin = new Administrator();
            admin.DisplayMenu();
        }
    }
}
