using System;
using System.IO;
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
            DBUtils.LoadUserDB(filepath, patientDB);
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
            DBUtils.GetUsersFromDatabase(patientDB);
        }

        public static void GetPatientDetails()
        {
            List<string> headers = new List<string>()
            {
                "Patient", "Doctor", "Email Address", "Phone", "Address"
            };

            DBUtils.GetUserDetails(patientDB, headers);
        }
    }
}