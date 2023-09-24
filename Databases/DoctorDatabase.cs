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

        public static void LoadDB(string filepath)
        {
            DBUtils.LoadUserDB(filepath, doctorDB);
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