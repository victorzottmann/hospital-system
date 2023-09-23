using System;
using System.Collections.Generic;
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

            string userType = user.GetType().Name;

            Menu menu = new Menu();
            menu.Subtitle(userType == "Patient" ? "Add Patient" : "Add Doctor");

            Console.WriteLine($"Registering a new {userType.ToLower()} with the DOTNET Hospital Management System\n");

            // organising the prompts in an array to loop through them
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
                string currentInput = Console.ReadLine()!;

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
            string firstName = inputs[0].Trim();
            string lastName = inputs[1].Trim();
            string email = inputs[2].Trim();
            string phone = inputs[3].Trim();
            string streetNumber = inputs[4].Trim();
            string street = inputs[5].Trim();
            string city = inputs[6].Trim();
            string state = inputs[7].Trim();

            Address address = new Address(streetNumber, street, city, state);

            string filepath = userType == "Patient" ? _patientsFilePath : _doctorsFilePath;
            int id = FindLargestUserID(filepath);
            int newUserId = ++id;

            if (userType == "Patient")
            {
                Patient newPatient = new Patient(newUserId, firstName, lastName, email, phone, address.ToString());
                PatientDatabase.AddPatient(newUserId, newPatient);   
            }   

            if (userType == "Doctor")
            {
                Doctor newDoctor = new Doctor(newUserId, firstName, lastName, email, phone, address.ToString());
                DoctorDatabase.AddDoctor(newUserId, newDoctor);
            }

            string role = userType == "Patient" ? "Patient" : "Doctor";

            string userInfo = $"{newUserId},{firstName},{lastName},{email},{phone},{streetNumber},{street},{city},{state}" + Environment.NewLine;
            string password = GeneratePassword(role);

            string userCredentials = $"{newUserId},{password},{role}";

            File.AppendAllText(filepath, userInfo);
            AddLoginCredential(role, _loginFilePath, userCredentials);

            Console.WriteLine($"{firstName} {lastName} added to the system!");
            Utils.ReturnToMenu(this, true);
        }

        public string GeneratePassword(string role)
        {
            string password = role == "Patient" ? "pat" : "doc";
            return password;
        }

        public void AddLoginCredential(string role, string filepath, string credentials)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    int insertionIndex = -1;

                    switch (role)
                    {
                        case "Patient":
                            insertionIndex = FindLoginIdIndex(lines, "1");
                            break;
                        case "Doctor":
                            insertionIndex = FindLoginIdIndex(lines, "2");
                            break;
                        default:
                            Console.WriteLine("Invalid user");
                            break;
                    }

                    List<string> updatedLines = new List<string>(lines);
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

        public int FindLoginIdIndex(string[] arr, string num)
        {
            int index = -1;

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i].StartsWith(num))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public static int FindLargestUserID(string filepath)
        {
            int largestId = 0;
            string[] lines = File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                string[] data = line.Split(',');

                if (data.Length > 0)
                {
                    int id;

                    if (int.TryParse(data[0], out id))
                    {
                        if (id > largestId)
                        {
                            largestId = id;
                        }
                    }
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