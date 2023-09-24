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

        /* 
         * Note that this method is exactly the same as for the DoctorDatabase
         * I do not understand yet how to make this particular feature generic so as to load
         * either the Patient or Doctor database from only one method
         */
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