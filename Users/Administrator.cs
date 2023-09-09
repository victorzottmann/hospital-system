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

        public Administrator()
        {
            AdministratorID++;
        }

        public void DisplayMenu()
        {
            Console.Clear();

            Menu adminMenu = new Menu();
            adminMenu.Subtitle("Administrator Menu");

            Console.WriteLine($"Welcome to DOTNET Hospital Management {FirstName} {LastName}");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List all doctors");
            Console.WriteLine("2. Check doctor details");
            Console.WriteLine("3. List all patients");
            Console.WriteLine("4. Check patient details");
            Console.WriteLine("5. Add doctor");
            Console.WriteLine("6. Add patient");
            Console.WriteLine("7. Logout");
            Console.WriteLine("8. Exit");

            Console.ReadKey();
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
                    //ListAllPatients();
                    break;
                case "4":
                    //CheckPatientDetails();
                    break;
                case "5":
                    //AddDoctor();
                    break;
                case "6":
                    //Add Patient();
                    break;
                case "7":
                    //Logout();
                    break;
                case "8":
                    //Exit();
                    break;
                default:
                    Console.WriteLine("Please select an option between 1 and 8");
                    break;
            }
        }
    }
}
