using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Security.Principal;

namespace HospitalSystem
{
    public class Administrator : User
    {
        /*
         * The naming convention for when a variable is private but doesn't have
         * auto-implemented propeties is _variable: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields
         */
        private static string _patientsFilePath = "patients.txt";
        private static string _doctorsFilePath = "doctors.txt";
        private static string _loginFilePath = "login-credentials.txt";

        public Administrator() 
        {
            /*
             * It was opted to set the names automatically when creating new instances of
             * an Admin to make doing so easier since there is currently only one Admin
             * registered in the system
             */
            this.SetFirstName("Victor");
            this.SetLastName("Zottmann");
        }

        public void ListAllDoctors()
        {
            DoctorDatabase.GetDoctors();
        }

        public void CheckDoctorDetails()
        {
            DoctorDatabase.GetDoctorDetails();
        }

        public void ListAllPatients()
        {
            PatientDatabase.GetPatients();
        }

        public void CheckPatientDetails()
        {
            PatientDatabase.GetPatientDetails();
        }

        public void AddUser(User user)
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle(user is Patient ? "Add Patient" : "Add Doctor");

            string userType = user.GetType().Name;
            Console.WriteLine($"Registering a new {userType.ToLower()} with the DOTNET Hospital Management System\n");

            // this notation destructures the return value (tuple) into their respective variables for further use
            (string firstName, string lastName, string email, string phone, Address address) = GetUserInputs();

            // depending on the user type (Patient or Doctor), the filepath will point to each respective file
            string filepath = GetFilePath(user);

            // then the newUserId will add 1 to the largest ID found in that file
            int newUserId = GetNewUserId(filepath);

            // then this method will add a new Patient or Doctor with the correct details
            CreateUser(user, newUserId, firstName, lastName, email, phone, address);
            
            string role = userType == "Patient" ? "patient" : "doctor";

            string userInfo =
                $"{newUserId},{firstName},{lastName},{email},{phone}," +
                $"{address.StreetNumber},{address.Street},{address.City},{address.State}" + 
                Environment.NewLine
            ;

            string password = GeneratePassword(role);

            string userCredentials = $"{newUserId},{password},{role}";

            File.AppendAllText(filepath, userInfo);

            /* add the credential where the role matches either "patient" or "doctor"
             *
             * role = "patient" => 1XXXX,"pat","patient"
             * role = "doctor"  => 2XXXX,"doc","doctor"
             */
            AddLoginCredentials(role, _loginFilePath, userCredentials);

            Console.WriteLine($"{firstName} {lastName} added to the system!");
            Utils.ReturnToMenu(this, true);
        }

        private string GetFilePath(User user)
        {
            return user is Patient ? _patientsFilePath : _doctorsFilePath;
        }

        private int GetNewUserId(string filepath)
        {
            // find the largest ID on the given filepath (Patient or Doctor), add 1 to it and return
            int id = FindLargestUserID(filepath);
            return ++id;
        }

        private string GeneratePassword(string role)
        {
            string password = role == "patient" ? "pat" : "doc";
            return password;
        }

        private void CreateUser(User user, int id, string firstName, string lastName, string email, string phone, Address address)
        {
            if (user is Patient)
            {
                Patient newPatient = new Patient(id, firstName, lastName, email, phone, address.ToString());
                PatientDatabase.AddPatient(id, newPatient);
            }

            if (user is Doctor)
            {
                Doctor newDoctor = new Doctor(id, firstName, lastName, email, phone, address.ToString());
                DoctorDatabase.AddDoctor(id, newDoctor);
            }
        }

        /* 
         * This method of type Tuple returns all the arguments as a single value.
         * It seemed appropriate to design it this way because the objective is to get all the
         * values inputted by the Admin when adding a new user.
         */
        private (string firstName, string lastName, string email, string phone, Address address) GetUserInputs()
        {
            string[] prompts =
            {
                "First Name",
                "Last Name",
                "Email",
                "Phone",
                "Street Number",
                "Street",
                "City",
                "State"
            };

            // creating an array of inputs to store them
            string[] inputs = new string[prompts.Length];

            // this loop prints the prompt in order and only proceeds to the next one if the input is not blank
            for (int i = 0; i < prompts.Length; i++)
            {
                Console.Write($"{prompts[i]}: ");
                string currentInput = Console.ReadLine()!.Trim();

                // if the input is not blank
                if (!string.IsNullOrWhiteSpace(currentInput))
                {
                    // the current input is stored in the inputs array
                    inputs[i] = currentInput;
                }
                else
                {
                    // otherwise prompt the user to re-enter the value and repeat the same prompt
                    Console.WriteLine($"The field cannot be empty!\n");
                    i--;
                }
            }

            // then if all goes well, store each input in their respective variable
            string firstName = inputs[0];
            string lastName = inputs[1];
            string email = inputs[2];
            string phone = inputs[3];
            string streetNumber = inputs[4];
            string street = inputs[5];
            string city = inputs[6];
            string state = inputs[7];

            Address address = new Address(streetNumber, street, city, state);

            return (firstName, lastName, email, phone, address);
        }

        private void AddLoginCredentials(string role, string filepath, string credentials)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    int insertionIndex = -1;

                    switch (role)
                    {
                        case "patient":
                            // find the index of the last ID in the file that starts with 1
                            // because IDs that start with 1 correspond to patient IDs
                            insertionIndex = FindLoginIdIndex(lines, "1");
                            break;
                        case "doctor":
                            // similarly, IDs that start with 2 correspond to doctor IDs
                            insertionIndex = FindLoginIdIndex(lines, "2");
                            break;
                        default:
                            Console.WriteLine("Invalid user");
                            break;
                    }

                    List<string> updatedLines = new List<string>(lines);

                    // once found, insert the new credentials below the last one
                    // the +1 is needed for that very reason
                    updatedLines.Insert(insertionIndex + 1, credentials);

                    // No + Environment.Newline because the credentials will be inserted
                    File.WriteAllLines(filepath, updatedLines);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
        }

        private int FindLoginIdIndex(string[] arr, string num)
        {
            int index = -1;

            // iterate in reverse order to find the last ID starting with the specified number
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i].StartsWith(num))
                {
                    index = i;
                    break;
                }
            }

            // once found, return the index position of where that ID is
            return index;
        }

        private int FindLargestUserID(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            
            int id;
            int largestId = 0;

            foreach (string line in lines)
            {
                id = int.Parse(line.Split(',')[0]); // get only the first value
  
                // Assign the id to the largestId if it's larger than largestID.
                // Since it iterates in ascending order, it is easy to predict that the largestID will be in the last row.
                if (id > largestId)
                {
                    largestId = id;
                }
            }

            return largestId;
        }

        public override void ProcessSelectedOption(string input)
        {
            switch (input)
            {
                case "1":
                    ListAllDoctors();
                    break;
                case "2":
                    CheckDoctorDetails();
                    break;
                case "3":
                    ListAllPatients();
                    break;
                case "4":
                    CheckPatientDetails();
                    break;
                case "5":
                    AddUser(new Doctor());
                    break;
                case "6":
                    AddUser(new Patient());
                    break;
                case "7":
                    Logout();
                    break;
                case "8":
                    Exit();
                    break;
                default:
                    Console.Write("\nPlease select an option between 1 and 8: ");
                    ProcessSelectedOption(Console.ReadLine()!);
                    break;
            }
        }
    }
}