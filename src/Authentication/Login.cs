using System;
using System.IO;

namespace HospitalSystem
{
    public class Login
    {
        // the file is stored at ./bin/Debug/net6.0
        private static string _loginFile = "login-credentials.txt";

        public Login() { }

        // load all existing databases (text files) on login
        public Login(string doctorsFile, string patientsFile, string doctorPatientsFile)
        {
            DoctorDatabase.LoadDB(doctorsFile);
            PatientDatabase.LoadDB(patientsFile);
            DoctorPatientDatabase.LoadDB(doctorPatientsFile);
        }

        public void LoginMenu()
        {
            Console.Clear();

            Menu loginMenu = new Menu();
            loginMenu.Subtitle("Login");

            Console.WriteLine("ID: ");
            Console.WriteLine("Password: ");

            // this will place the cursor right beside "ID: "
            Console.SetCursorPosition(4, 4);
            string userId = Console.ReadLine()!;

            // this will place the cursor right beside "Password: " after ID is read
            Console.SetCursorPosition(10, 5);
            string password = ReadPassword();

            string role = ValidateCredentials(_loginFile, userId, password);
            int id = int.Parse(userId);

            bool isAdmin = role == "admin";
            bool isDoctor = role == "doctor";
            bool isPatient = role == "patient";

            if (isAdmin)
            {
                Admin admin = new Admin();
                Utils.ShowUserMenu(admin);
            }
            else if (isDoctor)
            {
                // find the doctor from the static list of patients whose ID matches the one entered in the console
                var doctorDB = DoctorDatabase.GetDoctorDatabase();
                Doctor doctor = FindUserById(id, doctorDB);

                if (doctor != null)
                {
                    Utils.ShowUserMenu(doctor);
                }
            }
            else if (isPatient)
            {
                // find the patient from the static list of patients whose ID matches the one entered in the console
                var patientDB = PatientDatabase.GetPatientDatabase();
                Patient patient = FindUserById(id, patientDB);

                if (patient != null)
                {
                    Utils.ShowUserMenu(patient);
                }
            }

            Console.ReadKey();
        }

        /*
         * Since the method for finding a user by ID would be the same for either Patient or Doctor,
         * having a generic method that receives either the Patient or Doctor database is more suitable
         */
        private T FindUserById<T>(int userId, Dictionary<int, T> userDb) where T : User
        {
            if (userDb.ContainsKey(userId))
            {
                return userDb[userId];
            }

            return null;
        }

        private string ReadPassword()
        {
            /* References
             * Tutorial: https://www.c-sharpcorner.com/article/show-asterisks-instead-of-characters-for-password-input-in-console-application/
             * ConsoleKeyInfo https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo?view=net-7.0
             * ReadKey(Boolean) https://learn.microsoft.com/en-us/dotnet/api/system.console.readkey?view=net-7.0#system-console-readkey(system-boolean
             * Console.Write("\b \b") https://stackoverflow.com/questions/5195692/is-there-a-way-to-delete-a-character-that-has-just-been-written-using-console-wr
             */

            string password = "";

            // describes the console key that was pressed
            // in this case, the pressed key will come from Console.ReadKey()
            ConsoleKeyInfo keyInfo;

            do
            {
                // the boolean intercept suppressed the display of the keys in the console
                // if the value is false or empty, it will be shown in the console
                keyInfo = Console.ReadKey(intercept: true);

                // if the key is a Backspace and the password has at least one character
                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // remove the Backspace char from the password string if it's entered
                    password = password.Remove(password.Length - 1);

                    /* visually erases the Backspace char from the console
                     * the first \b is "erased" from the console
                     * the ' ' space in between replaces the Backspace with a white space
                     * the second \b moves the cursor back to the position before the space char
                     */
                    Console.Write("\b \b");
                }
                else if (keyInfo.Key != ConsoleKey.Enter)
                {
                    // adds the pressed console key to the password string while Enter is not pressed
                    password += keyInfo.KeyChar;

                    // essentially "replaces" the original char by a * when typing the password
                    Console.Write("*");
                }

                // repeat the above while Enter is not pressed
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return password;
        }

        // validates whether the entered user ID and password match those found in a text file.
        // it returns the user role, which is then used in DisplayMenu() to authenticate the user
        public string ValidateCredentials(string filepath, string userId, string password)
        {
            try
            {
                string[] lines = File.ReadAllLines(filepath);

                // assuming that the position of each element is known in the file
                foreach (var line in lines)
                {
                    string[] arr = line.Split(',');

                    // using Trim() to remove any white spaces from their boundaries
                    string targetUserId = arr[0].Trim();
                    string targetPassword = arr[1].Trim();
                    string targetRole = arr[2].Trim();

                    if (userId == targetUserId && password == targetPassword)
                    {
                        Console.WriteLine("Valid Credentials. Logging in...");
                        Thread.Sleep(1000);
                        return targetRole;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nOops... {e.Message}");
                Console.WriteLine("\nPlease ensure the file exists to log into the system.");
                Console.Write("\nPress any key to exit: ");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Console.WriteLine("Invalid Credentials\n");

            ReEnterCredentials();

            return null;
        }

        public void ReEnterCredentials()
        {
            while (true)
            {
                Console.Write("Press 1 to try again, or 0 to exit: ");

                string input = Console.ReadLine()!;

                if (input == "1")
                {
                    LoginMenu();
                    break;
                }
                else if (input == "0")
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
