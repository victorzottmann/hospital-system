using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public class DBUtils
    {
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

            Console.WriteLine($"All {userType}s registered to the DOTNET Hospital Management System");

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
                Console.WriteLine($"There are no {userType}s registered in the system yet.");
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
