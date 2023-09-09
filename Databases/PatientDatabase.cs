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
            adminMenu.Subtitle("Administrator");

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
                    $"{patient.GetAddress()} | "
                );
            }

            Console.ReadKey();
        }
    }
}
