using System;
using System.IO;
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

            Menu menu = new Menu();
            menu.Subtitle("All doctors");

            Console.WriteLine("All doctors registered to the DOTNET Hospital Management System");

            List<string> tableHeaders = new List<string>()
            {
                "Name", "Email Address", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            if (doctorDB.Count > 0)
            {
                foreach (var kvp in doctorDB)
                {
                    var doctor = kvp.Value;

                    tableRows.Add(doctor.ToStringArray());
                }

                Utilities.FormatTable(tableHeaders.ToArray(), tableRows);
            }
            else
            {
                Console.WriteLine("There are no doctors registered in the system yet.");
            }

            Console.Write($"\nPress any key to return to the Admin Menu: ");
            Console.ReadKey();

            Administrator admin = new Administrator();
            Utilities.ShowUserMenu(admin);
        }

        public static void GetDoctorDetails()
        {
            Console.Clear();

            Administrator admin = new Administrator();

            Menu menu = new Menu();
            menu.Subtitle("Doctor's Details");

            Console.Write("Please enter the ID of the doctor whose details you are checking, or press 'N' to return to menu: ");
            string id = Console.ReadLine()!;

            List<string> tableHeaders = new List<string>()
            {
                "Doctor", "Email Address", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            try
            {
                if (id != null && doctorDB.ContainsKey(int.Parse(id)))
                {                 
                    Doctor doctor = GetDoctorById(int.Parse(id));
                    Console.WriteLine($"\nDetails for {doctor.FullName}");

                    tableRows.Add(doctor.ToStringArray());
                    Utilities.FormatTable(tableHeaders.ToArray(), tableRows);
                    
                    Utilities.TryAgainOrReturn(admin, GetDoctorDetails);
                }
                else
                {
                    Console.WriteLine($"\nA doctor with ID {id} does not exist.");
                    Utilities.TryAgainOrReturn(admin, GetDoctorDetails);
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"\nAn error occured: {e.Message}");
                Utilities.TryAgainOrReturn(admin, GetDoctorDetails);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"\nAn error occured: {e.Message}");
                Utilities.TryAgainOrReturn(admin, GetDoctorDetails);
            }
        }
    }
}