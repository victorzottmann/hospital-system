using System;

namespace HospitalSystem
{
    /*
     * this static class is meant to handle several global methods that manage
     * features shared among all users, such as:
     * 
     * 1. showing their menu options after logging in,
     * 2. prompting to try again or return to the previous menu
     * 3. formatting tables when selecting to view certain details
     * 4. writing data to file
     */
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
                    Console.WriteLine("5. Logout");
                    Console.WriteLine("6. Exit\n");
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
                    /*
                     * Depending on the user executing this method, it references another method
                     * with a view of prompting the user to try again or return to the {userType} menu
                     */
                    methodToExecute();
                }
                else if (key == "n")
                {
                    break;
                }
            }

            // if n is pressed and the loop breaks, show the user menu
            ShowUserMenu(user);
        }

        public static void ReturnToMenu(User user, bool? message)
        {
            string userType = user.GetType().Name;

            // defaultMessage is an optional value
            // if passed as "true", show the message below
            if (message.HasValue)
            {
                Console.Write($"\nPress any key to return to the {userType} Menu: ");
                Console.ReadKey();
                ShowUserMenu(user);
            }

            // otherwise show custom message
            Console.WriteLine($"\n{message}");
            Console.ReadKey();
            ShowUserMenu(user);
        }

        public static void FormatTable(string[] headers, List<string[]> rows)
        {
            // make an array with the widths of each header
            int[] maxHeaderWidths = new int[headers.Length];

            // Calculate max width for each column based on the rows
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    try
                    {
                        // assign the largest length of the rows for each corresponding column
                        // in order to dynamically adjust the size of each column
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

            for (int i = 0; i < headers.Length; i++)
            {
                /* 
                 * String.Format helps with aligning the content in a particular way
                 * Here, the dash in "{0,-" aligns the content to the left 
                 * maxHeaderWidths[i] places the content on the left,
                 * and + 4 increases the width by 4 to give just enough room between each column
                 * Reference: https://learn.microsoft.com/en-us/dotnet/api/system.string.format?view=net-7.0
                 */

                // print each table header and align them to the left
                Console.Write(String.Format("{0,-" + (maxHeaderWidths[i] + 4) + "}", headers[i]));
            }

            Console.WriteLine();

            // print the table horizontal bar according to the sum of the length of all headers and their max widths
            Console.WriteLine(new string('-', maxHeaderWidths.Sum() + 4 * headers.Length));

            // format the rows in accordance with the headers (to the left and with enough space in between columns)
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
                /*
                 * This method inserts a new record into the doctor-patients.txt file
                 * and sorts it in ascending order relative to both the doctor and patient IDs
                 * 
                 * File format:
                 * doctorId,doctorFirstName,doctorLastName,patientId,patientFirstName,patientLastName
                 * 
                 * Since doctors can have many patients, if a doctor is assigned to another patient,
                 * that relationship must be inserted below the previously existing ones.
                 * 
                 * For example:
                 * 20001,Gregory,House,10001,Rebecca,Adler
                 * 20001,Gregory,House,10015,Mara,Keaton     => same doctor, different patient
                 * 20002,Lisa,Cuddy,10002,Clancy,Harris
                 * 20003,James,Wilson,10003,Joe,Luria
                 * ...
                 * 20025,Nathan,Riggs,10025,Henry,Burton 
                 */
                if (File.Exists(filepath))
                {
                    string doctorId = doctor.GetDoctorId().ToString();
                    string doctorFirstName = doctor.FirstName;
                    string doctorLastName = doctor.LastName;

                    string patientId = patient.GetPatientId().ToString();
                    string patientFirstName = patient.FirstName;
                    string patientLastName = patient.LastName;

                    string textToFile =
                       $"{doctorId}," +
                       $"{doctorFirstName}," +
                       $"{doctorLastName}," +
                       $"{patientId}," +
                       $"{patientFirstName}," +
                       $"{patientLastName}"
                    ;

                    List<string> lines = File.ReadAllLines(filepath).ToList();

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
                    for (int i = lines.Count - 1; i >= 0; i--)
                    {
                        if (lines[i].StartsWith(doctorId))
                        {
                            doctorExists = true;
                            insertionIndex = i;
                            break;
                        }
                    }

                    /*
                     * If a doctor matching the ID exists, insert the new record below it
                     * otherwise simply add the record to the lines List
                     */
                    if (doctorExists)
                    {
                        lines.Insert(insertionIndex + 1, textToFile);
                    }        
                    else
                    {
                        lines.Add(textToFile);
                    }

                    /*
                     * The Sort() method iterates through a List and compares two values at a time
                     * In the case of this implementation, if the IDs don't match, it'll sort them in
                     * ascending order. For reference, this is similar to sort() in JS
                     */
                    lines.Sort((a, b) =>
                    {
                        int docIdA = int.Parse(a.Split(',')[0]);
                        int docIdB = int.Parse(b.Split(",")[0]);

                        // first ensure that the doctor IDs are equal and sort them
                        if (docIdA != docIdB)
                        {
                            return docIdA.CompareTo(docIdB);
                        }

                        // and only then sort the patient IDs
                        int patIdA = int.Parse(a.Split(',')[3]);
                        int patIdB = int.Parse(b.Split(',')[3]);

                        /* CompareTo() returns a value indicating the relative order of a and b based on the IDs
                         * 
                         * if a < b,  it returns -1
                         * if a == b, it returns 0
                         * if a > b,  it returns 1
                         */
                        return patIdA.CompareTo(patIdB);
                    });

                    File.WriteAllLines(filepath, lines);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
        }
    }
}