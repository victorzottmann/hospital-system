using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public class DBUtils
    {
        public static void LoadUserDB<T>(string filepath, Dictionary<int, T> db) where T : User
        {
            Admin admin = new Admin();

            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    foreach (var line in lines)
                    {
                        string[] arr = line.Split(',');

                        int userId = int.Parse(arr[0]);
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
                            /*
                             * Defined in the User class, this static method creates a new instance of User
                             * that is either a Doctor or a Patient.
                             * This is necessary for loading the correct database file
                             * 
                             * For example, this would be the same as either:
                             * 
                             * Doctor doctor = new Doctor(doctorId, firstName, lastName, email, phone, address.ToString());
                             * Patient patient = new Patient(patientId, firstName, lastName, email, phone, address.ToString());
                             * 
                             * For reference: https://learn.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=net-7.0
                             */
                            T user = User.CreateInstanceOfUser<T>(userId, firstName, lastName, email, phone, address.ToString());

                            if (!db.ContainsKey(userId))
                            {
                                db.Add(userId, user);
                            }
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Error creating user: {e.Message}");
                            Utils.ReturnToMenu(admin);

                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
                Utils.ReturnToMenu(admin);
            }
        }

        /*
        * This is a generic method to get either all patients or doctors from their respective
        * databases. Both PatientDatabase and DoctorDatabase had the same method, but the only
        * difference was the database being used to get the data from
        */
        public static void GetUsersFromDatabase<T>(Dictionary<int, T> userDB) where T : User
        {
            Console.Clear();

            Admin admin = new Admin();
            Menu menu = new Menu();

            string userType = typeof(T).Name;

            // either "All Patients or "All Doctor"
            menu.Subtitle($"All {userType}s");

            Console.WriteLine($"All {userType.ToLower()}s registered to the DOTNET Hospital Management System");

            List<string> tableHeaders = new List<string>()
            {
                "Name", "Email Address", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            if (userDB.Count > 0)
            {
                foreach (T user in userDB.Values)
                {
                    // ToStringArray is declared in IUser and implemented in User
                    tableRows.Add(user.ToStringArray());
                }

                Utils.FormatTable(tableHeaders.ToArray(), tableRows);
                Utils.ReturnToMenu(admin, true);
            }
            else
            {
                Console.WriteLine($"\nThere are no {userType}s registered in the system yet.");
                Utils.ReturnToMenu(admin, true);
            }
        }

        public static void GetUserDetails<T>(Dictionary<int, T> userDB, List<string> tableHeaders) where T : User
        {
            Console.Clear();

            Admin admin = new Admin();
            Menu menu = new Menu();

            string userType = typeof(T).Name;
            menu.Subtitle($"{userType}'s Details");

            Console.Write($"Please enter the ID of the {userType.ToLower()} whose details you are checking, or press 'N' to return to menu: ");
            string input = Console.ReadLine()!;
            int id = int.Parse(input);

            List<string[]> tableRows = new List<string[]>();

            try
            {
                if (input != null && userDB.ContainsKey(id))
                {
                    T user = userDB[id];
                    Console.WriteLine($"\nDetails for {user.FullName}");

                    if (user is Doctor doctor)
                    {
                        tableRows.Add(user.ToStringArray());
                    }
                    else if (user is Patient patient)
                    {
                        string doctorName = patient.GetPatientDoctor() == null ? "Not Assigned" : patient.GetPatientDoctor().FullName;
                        tableRows.Add(new string[]
                        {
                            patient.FullName,
                            doctorName,
                            patient.Email,
                            patient.Phone,
                            patient.Address
                        });
                    }

                    Utils.FormatTable(tableHeaders.ToArray(), tableRows);

                    Utils.TryAgainOrReturn(admin, () => GetUserDetails(userDB, tableHeaders));
                }
                else
                {
                    Console.WriteLine($"\nA {userType.ToLower()} with ID {id} does not exist.");
                    Utils.TryAgainOrReturn(admin, () => GetUserDetails(userDB, tableHeaders));
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"\nAn error occurred: {e.Message}");
                Utils.TryAgainOrReturn(admin, () => GetUserDetails(userDB, tableHeaders));
            }
            catch (FormatException e)
            {
                Console.WriteLine($"\nAn error occurred: {e.Message}");
                Utils.TryAgainOrReturn(admin, () => GetUserDetails(userDB, tableHeaders));
            }
        }
    }
}
