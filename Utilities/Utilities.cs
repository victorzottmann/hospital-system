using System;

namespace HospitalSystem
{
    public class Utilities
    {
        public static void ShowUserMenu(User user)
        {
            Console.Clear();

            Menu menu = new Menu();
            string userType = user.GetType().Name; // get the name of the class

            switch (userType)
            {
                case "Patient":
                    menu.Subtitle("Patient Menu");
                    Console.WriteLine($"Welcome to DOTNET Hospital Management {user.FirstName} {user.LastName}\n");
                    Console.WriteLine("Please choose an option:");
                    Console.WriteLine("1. List patient details");
                    Console.WriteLine("2. List my doctor details");
                    Console.WriteLine("3. List all appointments");
                    Console.WriteLine("4. Book appointment");
                    Console.WriteLine("5. Exit to login");
                    Console.WriteLine("6. Exit system\n");
                    break;
                case "Doctor":
                    menu.Subtitle("Doctor Menu");
                    Console.WriteLine($"Welcome to DOTNET Hospital Management {user.FirstName} {user.LastName}\n");
                    Console.WriteLine("Please choose an option:");
                    Console.WriteLine("1. List doctor details");
                    Console.WriteLine("2. List patients");
                    Console.WriteLine("3. List appointments");
                    Console.WriteLine("4. Check particular patient");
                    Console.WriteLine("5. List appointments with patient");
                    Console.WriteLine("6. Logout");
                    Console.WriteLine("7. Exit\n");
                    break;
                case "Admin":
                    menu.Subtitle("Administrator Menu");
                    Console.WriteLine($"Welcome to DOTNET Hospital Management {user.FirstName} {user.LastName}\n");
                    Console.WriteLine("Please choose an option:");
                    Console.WriteLine("1. List all doctors");
                    Console.WriteLine("2. Check doctor details");
                    Console.WriteLine("3. List all patients");
                    Console.WriteLine("4. Check patient details");
                    Console.WriteLine("5. Add doctor");
                    Console.WriteLine("6. Add patient");
                    Console.WriteLine("7. Logout");
                    Console.WriteLine("8. Exit\n");
                    break;
                default:
                    Console.WriteLine("Invalid user type");
                    break;
            }

            string input = Console.ReadLine()!;

            // every User has an implementation of this method
            user.ProcessSelectedOption(input);
        }

        public delegate void Action();

        public static void TryAgainAndReturn(User user, Action methodToExecute)
        {
            while (true)
            {
                Console.Write($"\nPress 1 to try again or 'N' to return to the Doctor Menu: ");
                string key = Console.ReadLine()!;

                if (key == "1")
                {
                    methodToExecute();
                }
                else if (key == "n")
                {
                    break;
                }
            }

            ShowUserMenu(user);
        }

        public static void FormatTable(string[] headers, List<string[]> rows)
        {
            // Calculate max width for each column
            int[] maxWidths = new int[headers.Length];
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i].Length > maxWidths[i])
                    {
                        maxWidths[i] = row[i].Length;
                    }
                }
            }

            // Print the table headers
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write(string.Format("{0,-" + (maxWidths[i] + 4) + "}", headers[i]));
            }

            Console.WriteLine();

            // Print the table horizontal bar
            Console.WriteLine(new string('-', maxWidths.Sum() + 4 * headers.Length));

            // Print the table rows
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    Console.Write(string.Format("{0,-" + (maxWidths[i] + 4) + "}", row[i]));
                }
                Console.WriteLine();
            }
        }
    }
}
