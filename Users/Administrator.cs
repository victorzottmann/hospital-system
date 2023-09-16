using System;
using System.Collections.Generic;
using HospitalSystem.Databases;

namespace HospitalSystem.Users
{
    public class Administrator : User
    {
        private static string _patientsFilePath = "patients.txt";
        private static string _doctorsFilePath = "doctors.txt";

        public Administrator() 
        {
            this.FirstName = "Victor";
            this.LastName = "Zottmann";
        }

        public Administrator(string firstName, string lastName) : base(firstName, lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public void DisplayMenu()
        {
            Console.Clear();

            Menu adminMenu = new Menu();
            adminMenu.Subtitle("Administrator Menu");

            Console.WriteLine($"Welcome to DOTNET Hospital Management {this.FirstName} {this.LastName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List all doctors");
            Console.WriteLine("2. Check doctor details");
            Console.WriteLine("3. List all patients");
            Console.WriteLine("4. Check patient details");
            Console.WriteLine("5. Add doctor");
            Console.WriteLine("6. Add patient");
            Console.WriteLine("7. Logout");
            Console.WriteLine("8. Exit\n");

            string input = Console.ReadLine()!;
            ProcessSelectedOption(input);
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

        public void AddUser(string role)
        {
            Console.Clear();

            Menu addUserMenu = new Menu();

            if (role != null)
            {
                if (role == "patient")
                {
                    addUserMenu.Subtitle("Add Patient");
                }

                if (role == "doctor")
                {
                    addUserMenu.Subtitle("Add Doctor");
                }
            }

            Console.WriteLine("Registering a new patient with the DOTNET Hospital Management System");

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
                    Console.WriteLine($"\nThe field cannot be empty!");
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

            if (role == "patient")
            {
                int id = FindLargestUserID(_patientsFilePath);
                int newPatientId = ++id;

                Patient newPatient = new Patient(firstName, lastName, email, phone, address.ToString());
                PatientDatabase.AddPatient(newPatientId, newPatient);

                // need to fix the address part (streetNumber)
                string patientInfo = $"{newPatientId},{firstName},{lastName},{email},{phone},{streetNumber},{street},{city},{state}" + Environment.NewLine;
                File.AppendAllText(_patientsFilePath, patientInfo);
            }   

            if (role == "doctor")
            {
                int id = FindLargestUserID(_doctorsFilePath);
                int newDoctorId = ++id;

                Doctor newDoctor = new Doctor(firstName, lastName, email, phone, address.ToString());
                DoctorDatabase.AddDoctor(newDoctorId, newDoctor);

                // need to fix the address part (streetNumber)
                string doctorInfo = $"{newDoctorId},{firstName},{lastName},{email},{phone},{streetNumber},{street},{city},{state}" + Environment.NewLine;
                File.AppendAllText(_doctorsFilePath, doctorInfo);
            }

            Console.WriteLine($"{firstName} {lastName} added to the system!\n");

            Console.Write("Press any key to go back: ");
            Console.ReadKey();
            DisplayMenu();
        }

        public void Logout()
        {
            Login login = new Login();
            login.DisplayMenu();
        }

        public void Exit()
        {
            Environment.Exit(0);
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

        public void ProcessSelectedOption(string input)
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
                    AddUser("doctor");
                    break;
                case "6":
                    AddUser("patient");
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
