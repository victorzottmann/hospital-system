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

            Console.Write("ID: ");
            string userId = Console.ReadLine()!;

            Console.Write("Password: ");
            string password = Console.ReadLine()!;

            ValidateCredentials(this.FilePath, userId, password);

            Console.ReadKey();
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

            return false;
        }
    }
}
