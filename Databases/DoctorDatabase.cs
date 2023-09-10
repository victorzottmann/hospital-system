using System;
using System.Collections.Generic;
using HospitalSystem.Users;

namespace HospitalSystem.Databases
{
    public class DoctorDatabase
    {
        // static dictionary to allow other classes to find doctors registered in the system
        private static Dictionary<int, Doctor> doctorDB = new Dictionary<int, Doctor>();

        public static void AddDoctor(int id, Doctor doctor)
        {
            // key: PatientID, value: Patient obj
            doctorDB.Add(id, doctor);
        }

        public static Dictionary<int, Doctor> GetDoctorDatabase()
        {
            return doctorDB;
        }

        public static int GetPatientId(int doctorId)
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
            Console.Clear();

            Menu allDoctors = new Menu();
            allDoctors.Subtitle("All doctors");

            Console.WriteLine("All doctors registered to the DOTNET Hospital Management System\n");

            if (doctorDB.Count > 0)
            {
                Console.WriteLine("Name | Email Address | Phone | Address");
                Console.WriteLine("------------------------------------------------------------------------");

                foreach (var kvp in doctorDB)
                {
                    var doctor = kvp.Value;

                    Console.WriteLine(
                        $"{doctor.GetFirstName()} {doctor.GetLastName()} | " +
                        $"{doctor.GetEmail()} | " +
                        $"{doctor.GetPhone()} | " +
                        $"{doctor.GetAddress()}"                  
                    );
                }
            }
            else
            {
                Console.WriteLine("There are no doctors registered in the system yet.");
            }

            Console.Write($"\nPress any key to return: ");
            Console.ReadKey();

            Administrator admin = new Administrator();
            admin.DisplayMenu();
        }

        public static void GetDoctorDetails()
        {
            Console.Clear();

            Menu doctorDetailsMenu = new Menu();
            doctorDetailsMenu.Subtitle("Patient Details");

            Console.WriteLine("Please enter the ID of the doctor whose details you are checking, or press n to return to menu: ");
            int id = Convert.ToInt32(Console.ReadLine()!);

            try
            {
                Doctor doctor = GetDoctorById(id);
                
                if (doctor != null)
                {
                    Console.WriteLine($"\nDetails for {doctor.GetFirstName()} {doctor.GetLastName()}\n");
                    Console.WriteLine("Doctor | Email Address | Phone | Address");
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.WriteLine(doctor.ToString());
                }
                else
                {
                    Console.WriteLine("\nThere are no doctors registered in the system yet.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured: {e.Message}");
            }

            Console.Write($"\nPress any key to return to the Admin Menu: ");
            Console.ReadKey();

            Administrator admin = new Administrator();
            admin.DisplayMenu();
        }
    }
}