using System;

namespace HospitalSystem
{
    public static class Utils
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
                case "Administrator":
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

        public static void TryAgainOrReturn(User user, Action methodToExecute, string? message = "")
        {
            string userType = user.GetType().Name;
            while (true)
            {
                Console.Write($"\nPress 1 to try again or 'N' to return to the {userType} Menu: ");
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

        public static void ReturnToMenu(User user, bool? defaultMessage)
        {
            string userType = user.GetType().Name;

            if (defaultMessage.HasValue)
            {
                Console.Write($"\nPress any key to return to the {userType} Menu: ");
                Console.ReadKey();
                ShowUserMenu(user);
            }

            Console.ReadKey();
            ShowUserMenu(user);
        }

        public static void FormatTable(string[] headers, List<string[]> rows)
        {
            int[] maxHeaderWidths = new int[headers.Length];

            // Calculate max width for each column based on the rows
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    try
                    {
                        if (row[i].Length > maxHeaderWidths[i])
                        {
                            maxHeaderWidths[i] = row[i].Length;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine($"An error occurred: {e.Message}");
                    }
                }
            }

            Console.WriteLine();

            // Print the table headers
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write(String.Format("{0,-" + (maxHeaderWidths[i] + 4) + "}", headers[i]));
            }

            Console.WriteLine();

            // Print the table horizontal bar
            Console.WriteLine(new string('-', maxHeaderWidths.Sum() + 4 * headers.Length));

            // Print the table rows
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    Console.Write(String.Format("{0,-" + (maxHeaderWidths[i] + 4) + "}", row[i]));
                }
                Console.WriteLine();
            }
        }

        public static void WriteToFile(string filepath, Doctor doctor, Patient patient)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    string doctorId = doctor.GetDoctorId().ToString();
                    string doctorFirstName = doctor.FirstName;
                    string doctorLastName = doctor.LastName;

                    string patientId = patient.GetPatientId().ToString();
                    string patientFirstName = patient.FirstName;
                    string patientLastName = patient.LastName;

                    int insertionIndex = -1;
                    bool doctorExists = false;

                    /*
                     * For loop:
                     *      Starting from the last index (last line in the file),
                     *      it iterates through each line from the last line to the first line
                     *      until it reaches the first line (i >= 0)
                     * If Statement:
                     *      If a line starts with the target doctorId,
                     *      set the insertionIndex of the new line to i
                     *      Then break out of the loop once it assigns i to insertionIndex
                     *      of the doctorId
                     *      
                     *      NOTE: it will be appended to the bottom portion of the same doctorId because 
                     *      it's iterating in reverse order
                     * 
                     * NOTE: it must be lines.Length - 1 because line 1 is actually line 0 in terms of Arrays
                     * So, if a file has 27 lines, it's not that it goes from 1-27, but from 0-26
                     */
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        if (lines[i].StartsWith(doctorId))
                        {
                            doctorExists = true;
                            insertionIndex = i;
                            break;
                        }
                    }

                    string textToFile =
                        $"{doctorId}," +
                        $"{doctorFirstName}," +
                        $"{doctorLastName}," +
                        $"{patientId}," +
                        $"{patientFirstName}," +
                        $"{patientLastName}"
                    ;

                    if (doctorExists)
                    {
                        // initialising the list with the elements of the lines array
                        List<string> updatedLines = new List<string>(lines);

                        /*
                         * Here it's necessary to add 1 to insertionIndex because, unlike an array,
                         * a file starts at line 1, not 0.
                         * 
                         * So, if you consider that the indexes of an array go from 0-26,
                         * and a text file actually goes from 1-27, you need to add 1 to the insertion index
                         * in order to correctly append the new line relative to the target doctorId.
                         */
                        updatedLines.Insert(insertionIndex + 1, textToFile);

                        File.WriteAllLines(filepath, updatedLines);
                    }
                    else
                    {
                        File.AppendAllText(filepath, textToFile + Environment.NewLine);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
        }
    }
}