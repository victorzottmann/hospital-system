using HospitalSystem.Users;
using System;
using System.IO;

namespace HospitalSystem
{
    public class Login
    {
        private string FilePath;

        public Login(string filePath)
        {
            this.FilePath = filePath;
        }

        public void DisplayMenu()
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
            string password = this.ReadPassword();

            ValidateCredentials(this.FilePath, userId, password);

            Console.ReadKey();
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

        public bool ValidateCredentials(string filepath, string userId, string password)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    foreach (var line in lines)
                    {
                        string[] arr = line.Split(',');

                        // trim strings to ensure there are no white spaces
                        string targetUserId = arr[0].Trim();
                        string targetPassword = arr[1].Trim();

                        bool validUserId = userId == targetUserId;
                        bool validPassword = password == targetPassword;

                        if (validUserId && validPassword)
                        {
                            Console.WriteLine("Valid Credentials");
                            return true;
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

            Console.WriteLine("Invalid Credentials\n");

            this.ReEnterCredentials();

            return false;
        }

        public void ReEnterCredentials()
        {
            while (true)
            {
                Console.Write("Press 1 to try again, or 0 to exit: ");

                string input = Console.ReadLine()!;

                if (input == "1")
                {
                    this.DisplayMenu();
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
