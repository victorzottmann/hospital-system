using System;
using System.IO;
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

        public static void LoadDoctorDB(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    foreach (var line in lines)
                    {
                        string[] arr = line.Split(',');

                        string firstName = arr[0];
                        string lastName = arr[1];
                        string email = arr[2];
                        string phone = arr[3];
                        string address = arr[4];

                        Doctor doctor = new Doctor(firstName, lastName, email, phone, address);
                        int doctorId = doctor.GetDoctorId();

                        doctorDB.Add(doctorId, doctor);
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