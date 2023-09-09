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
            Console.WriteLine("8. Exit");

            string input = Console.ReadLine()!;
            ProcessSelectedOption(input);
        }

        public static int ReadUserID(string role)
        {
            Console.Write($"Please enter the {role} ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            return id;
        }

        public void ListAllPatients()
        {
            PatientDatabase.PrintPatients();
        }

        public void CheckPatientDetails()
        {
            PatientDatabase.PrintPatientDetails();
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
                    //AddPatient();
                    break;
                case "7":
                    Logout();
                    break;
                case "8":
                    Exit();
                    break;
                default:
                    Console.WriteLine("Please select an option between 1 and 8");
                    break;
            }
        }
    }
}
