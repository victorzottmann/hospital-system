using HospitalSystem.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Users
{
    public class Administrator : User
    {
        private int AdministratorID = 30000;

        public Administrator() { }

        public Administrator(string firstName, string lastName) : base(firstName, lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AdministratorID++;
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

        public void ListAllPatients()
        {
            PatientDatabase.GetPatients();
        }

        public void CheckPatientDetails()
        {
            PatientDatabase.GetPatientDetails();
        }

        public void AddPatient()
        {
            Console.Clear();

            Menu addPatientMenu = new Menu();
            addPatientMenu.Subtitle("Add Patient");

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
            string firstName = inputs[0];
            string lastName = inputs[1];
            string email = inputs[2];
            string phone = inputs[3];
            string streetNumber = inputs[4];
            string street = inputs[5];
            string city = inputs[6];
            string state = inputs[7];

            Address address = new Address(streetNumber, street, city, state);

            Patient patient = new Patient(firstName, lastName, email, phone, address.ToString());
            int patientID = patient.GetPatientId();
            PatientDatabase.AddPatient(patientID, patient);

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

        public void ProcessSelectedOption(string input)
        {
            switch (input)
            {
                case "1":
                    //ListAllDoctors();
                    break;
                case "2":
                    //CheckDoctorDetails();
                    break;
                case "3":
                    ListAllPatients();
                    break;
                case "4":
                    CheckPatientDetails();
                    break;
                case "5":
                    //AddDoctor();
                    break;
                case "6":
                    AddPatient();
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
