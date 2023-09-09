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

        public static void PrintPatients()
        {
            Console.Clear();

            Menu adminMenu = new Menu();
            adminMenu.Subtitle("All Patients");

            Console.WriteLine("All patients registered to the DOTNET Hospital Management System\n");

            // NEED TO COMPLETE FOR DOCTOR
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

            Console.Write($"\nPress any key to go back: ");
            Console.ReadKey();
            Administrator admin = new Administrator();
            admin.DisplayMenu();
        }

        public static void PrintPatientDetails()
        {
            Console.Clear();

            Menu patientDetailsMenu = new Menu();
            patientDetailsMenu.Subtitle("Patient Details");

            int id = Administrator.ReadUserID("patient");

            // clear the console after reading from input and reload the menu title followed by the details
            Console.Clear();
            patientDetailsMenu.Subtitle("Patient Details");

            Patient patient = GetPatientById(id);

            string fullName = $"{patient.GetFirstName()} {patient.GetLastName()}";
            string address = patient.GetAddress();
            string email = patient.GetEmail();
            string phone = patient.GetPhone();

            Console.WriteLine($"{fullName}'s Details\n");

            Console.WriteLine($"Patient ID: {id}");
            Console.WriteLine($"Full Name: {fullName}");
            Console.WriteLine($"Address: {address}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Phone: {phone}");

            Console.Write($"\nPress any key to go back: ");
            Console.ReadKey();
            Administrator admin = new Administrator();
            admin.DisplayMenu();
        }
    }
}
