﻿using System;
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
                    Console.WriteLine(doctor.ToString());
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

            Administrator admin = new Administrator();

            Menu doctorDetailsMenu = new Menu();
            doctorDetailsMenu.Subtitle("Doctor's Details");

            Console.WriteLine("Please enter the ID of the doctor whose details you are checking, or press 'N' to return to menu: ");
            
            string id = Console.ReadLine()!;

            try
            {
                if (id != null && doctorDB.ContainsKey(int.Parse(id)))
                {                 
                    Doctor doctor = GetDoctorById(int.Parse(id));

                    Console.WriteLine($"\nDetails for {doctor.GetFirstName()} {doctor.GetLastName()}\n");
                    Console.WriteLine("Doctor | Email Address | Phone | Address");
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.WriteLine(doctor.ToString());

                    string key;

                    while (true)
                    {
                        Console.Write($"\nPress 'N' to return to the menu: ");
                        key = Console.ReadLine()!;

                        if (key == "n")
                        {
                            break;
                        }
                    }

                    admin.DisplayMenu();
                }
                else
                {
                    Console.WriteLine($"\nA doctor with ID {id} does not exist.");
                    Console.Write("\nPress 1 to try again: ");
                    while (true)
                    {
                        string key = Console.ReadLine()!;

                        if (key == "1")
                        {
                            GetDoctorDetails();
                            break;
                        }
                        else if (key == "n")
                        {
                            break;
                        }
                    }

                    admin.DisplayMenu();
                }
            }
            catch (Exception e)
            {
                // FIX THIS TO PROMPT THE USER AGAIN UNTIL INPUT IS CORRECT
                Console.WriteLine($"\nAn error occured: {e.Message}");
                Console.WriteLine($"Please make sure that the value is not blank.\n");

                Console.Write("\nPress 1 to try again or 'N' to return to the menu: ");

                while (true)
                {
                    string key = Console.ReadLine()!;

                    if (key == "1")
                    {
                        GetDoctorDetails();
                        break;
                    }
                    else if (key == "n")
                    {
                        break;
                    }
                }

                admin.DisplayMenu();
            }
        }
    }
}