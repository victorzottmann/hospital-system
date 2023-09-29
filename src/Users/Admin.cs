using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public class Admin : User
    {
        /*
         * these files are stored at ./bin/Debug/net6.0
         * The naming convention for when a variable is private but doesn't have
         * auto-implemented propeties is _variable: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields
         */
        private static string _patientsFilePath = "patients.txt";
        private static string _doctorsFilePath = "doctors.txt";
        private static string _loginFilePath = "login-credentials.txt";

        public Admin()
        {
            /*
             * It was opted to set the names automatically when creating new instances of
             * an Admin to make doing so easier since there is currently only one Admin
             * registered in the system
             */
            SetFirstName("Victor");
            SetLastName("Zottmann");
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
            string filepath = GetUserFilePath(user);

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
            Utils.ReturnToMenu(this);
        }

        private string GetUserFilePath(User user)
        {
            return user is Patient ? _patientsFilePath : _doctorsFilePath;
        }

        private int GetNewUserId(string filepath)
        {
            // find the largest ID on the given filepath (Patient or Doctor), add 1 to it and return
            int id = FindLargestUserId(filepath);
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

            /*
             * This delegate Func variable declares an array of functions that
             * receive a string as input and return a string as output
             * 
             * Validators.CapitaliseString receive a string as input,
             * capitalise the first letter, and return the formatted string
             * 
             * str => str simply takes a string and returns a string without any changes
             * 
             * str => str.ToUpper() takes a string and returns it in upper case
             */
            Func<string, string>[] formatters =
            {
                Validators.CapitaliseString, // For First Name
                Validators.CapitaliseString, // For Last Name
                str => str,                  // No string formatting needed for email
                str => str,                  // No string formatting needed for phone
                str => str,                  // No string formatting needed for street number
                Validators.CapitaliseString, // For Street
                Validators.CapitaliseString, // For City
                str => str.ToUpper()         // For State
            };

            /*
             * This delegate Func variable declares an array of functions that
             * take a string as input, a boolean, and return a string (the error message)
             * 
             * Each lambda expression takes both input and isValid as parameters.
             * If isValid is true, return an empty string, else return a custom error message
             */
            Func<string, bool, string>[] errorMessages =
            {
                (input, isValid) => isValid ? string.Empty : "The first name can only contain letters\n",
                (input, isValid) => isValid ? string.Empty : "The first name can only contain letters\n",
                (input, isValid) => isValid ? string.Empty : "Please ensure that the email matches the format: name@example.com\n",
                (input, isValid) => isValid ? string.Empty : "The phone number must contain only 8 digits\n",
                (input, isValid) => isValid ? string.Empty : "The street number must contain up to 3 digits\n",
                (input, isValid) => isValid ? string.Empty : "The street name must only contain letters\n",
                (input, isValid) => isValid ? string.Empty : "The city cannot contain numbers or punctuation\n",
                (input, isValid) => isValid ? string.Empty : "Please enter an abbreviate state of up to 3 letters. For example: NSW\n"
            };

            /*
             * This Func delegate variable declares an array of functions that 
             * take a string as input and return a boolean.
             * 
             * Each function runs a Regex check against the expected format of the corresponding inputs
             * If the Regex evaluates to true, they return true, else they return false
             */
            Func<string, bool>[] validators =
            {
                Validators.ValidateName, // first name
                Validators.ValidateName, // last name follows the same format as first name
                Validators.ValidateEmail,
                Validators.ValidatePhone,
                Validators.ValidateStreetNumber,
                Validators.ValidateStreet,
                Validators.ValidateCity,
                Validators.ValidateState
            };

            // this loop prints the prompt in order and only proceeds to the next one if the input is not blank
            for (int i = 0; i < prompts.Length; i++)
            {
                Console.Write($"{prompts[i]}: ");
                string currentInput = Console.ReadLine()!.Trim();

                /*
                 * Check if the input is not blank and if the input matches the Regex in each validator
                 * Example: 
                 *      validators[0](currentInput) is the same as Validators.ValidateName(currentInput) 
                 */
                bool isValid = !string.IsNullOrWhiteSpace(currentInput) && validators[i](currentInput);

                if (isValid)
                {
                    /*
                     * The formatters array execute each function that corresponds to the index of the current input. 
                     * This operation works because the input is a string and the functions stored in formatters 
                     * take a string as input.
                     * 
                     * Example: inputs[i] = Validators.CapitaliseString(currentInput)
                     * 
                     * While the validators check for a Regex match, it is necessary to run 
                     * each formatter function individually in order to assign the formatted
                     * input into inputs[i]
                     */
                    inputs[i] = formatters[i](currentInput);
                }
                else
                {
                    // otherwise prompt the user to re-enter the value and repeat the same prompt
                    Console.WriteLine(errorMessages[i](currentInput, isValid));
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

        private int FindLargestUserId(string filepath)
        {
            int largestId = 0;

            try
            {
                string[] lines = File.ReadAllLines(filepath);

                int id;

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
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"\nOops... {e.Message}");
                Console.WriteLine($"\nPlease ensure that the file exists and the path is set correctly");
                Utils.ReturnToMenu(this);
            }

            return largestId;
        }

        public override void ProcessSelectedOption(string input)
        {
            // please refer to ShowUserMenu() in ./Utils for the list of options
            // for the Admin
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