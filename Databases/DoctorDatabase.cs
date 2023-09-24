using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public class DoctorDatabase
    {
        // static dictionary to allow other classes to find doctors registered in the system
        private static Dictionary<int, Doctor> doctorDB = new Dictionary<int, Doctor>();

        public static void AddDoctor(int id, Doctor doctor)
        {
            // key: DoctorID, value: Doctor obj
            doctorDB.Add(id, doctor);
        }

        /* 
         * Note that this method is exactly the same as for the PatientDatabase
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

                        int doctorId = int.Parse(arr[0]);
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
                            Doctor doctor = new Doctor(doctorId, firstName, lastName, email, phone, address.ToString());

                            if (!doctorDB.ContainsKey(doctorId))
                            {
                                doctorDB.Add(doctorId, doctor);
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

        public static Dictionary<int, Doctor> GetDoctorDatabase()
        {
            return doctorDB;
        }

        public static int GetDoctorId(int doctorId)
        {
            try
            {
                if (doctorDB.ContainsKey(doctorId))
                {
                    return doctorId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
                Console.WriteLine($"Doctor with ID {doctorId} not found. Please check that the ID is valid.\n");
            }
            return -1;
        }

        public static Doctor GetDoctorById(int doctorId)
        {
            if (doctorDB.ContainsKey(doctorId))
            {
                return doctorDB[doctorId];
            }
            return null;
        }

        public static void GetDoctors()
        {
            DBUtils.GetUsersFromDatabase(doctorDB);
        }

        public static void GetDoctorDetails()
        {
            List<string> headers = new List<string>()
            {
                "Doctor", "Email Address", "Phone", "Address"
            };

            DBUtils.GetUserDetails(doctorDB, headers);
        }
    }
}