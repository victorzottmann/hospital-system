using System;
using System.Collections.Generic;
using System.Numerics;
using HospitalSystem.Databases;

namespace HospitalSystem.Users
{
    public class Patient : User
    {
        private int PatientID = 10000;
        private static string _appointmentsFilePath = "appointments.txt";

        private Dictionary<Doctor, List<string>> DoctorAppointments { get; } = new Dictionary<Doctor, List<string>>();

        public Patient(string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            PatientID++;
        }

        public int GetPatientId() => this.PatientID;

        public void DisplayMenu()
        {
            Console.Clear();

            Menu patientMenu = new Menu();
            patientMenu.Subtitle("Patient Menu");

            Console.WriteLine($"Welcome to DOTNET Hospital Management {this.FirstName} {this.LastName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit system\n");

            string input = Console.ReadLine()!;
            ProcessSelectedOption(input);
        }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName} | {this.Email} | {this.Phone} | {this.Address}";
        }

        public void AssignDoctor(Doctor doctor)
        {
            doctor.AssignPatient(this);
        }

        public void AddAppointment(Doctor doctor, string description, string textToFile)
        {
            if (!DoctorAppointments.ContainsKey(doctor))
            {
                DoctorAppointments[doctor] = new List<string>();
            }

            DoctorAppointments[doctor].Add(description);

            doctor.AssignPatient(this);

            File.AppendAllText("appointments.txt", textToFile + Environment.NewLine);
        }

        // Maybe overload this later to display details in relation to user permissions (admin vs user)
        public void ListPatientDetails()
        {
            Console.Clear();

            Menu patientDetails = new Menu();
            patientDetails.Subtitle("My Details");

            Console.WriteLine($"{this.FirstName} {this.LastName}'s Details\n");

            Console.WriteLine($"Patient ID: {this.PatientID}");
            Console.WriteLine($"Full name: {this.FirstName} {this.LastName}");
            Console.WriteLine($"Address: {this.Address}");
            Console.WriteLine($"Email: {this.Email}");
            Console.WriteLine($"Phone: {this.Phone}");

            Console.Write("\n\nPress any key to the Patient Menu: ");
            Console.ReadKey();

            DisplayMenu();
        }

        public void ListMyDoctorDetails()
        {
            Console.Clear();

            Menu patientDoctorDetails = new Menu();
            patientDoctorDetails.Subtitle("My Doctor");

            Console.WriteLine("Your doctors:\n");
            Console.WriteLine("Name | Email Address | Phone | Address");
            Console.WriteLine("------------------------------------------------------------------------");

           

            Console.Write("\nPress any key to the Patient Menu: ");
            Console.ReadKey();

            DisplayMenu();
        }

        public void ListAllAppointments()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("My Appointments");

            Console.WriteLine($"Appointments for {this.FirstName} {this.LastName}\n");

            try
            {
                if (File.Exists(_appointmentsFilePath))
                {
                    string[] lines = File.ReadAllLines(_appointmentsFilePath);

                    Console.WriteLine("Doctor | Patient | Description");
                    Console.WriteLine("---------------------------------------------------");

                    bool appointmentsFound = false;

                    foreach (string line in lines)
                    {
                        string[] arr = line.Split(',');

                        if (arr.Length == 5)
                        {
                            string doctorFirstName = arr[0];
                            string doctorLastName = arr[1];
                            string patientFirstName = arr[2];
                            string patientLastName = arr[3];
                            string description = arr[4];

                            string doctorFullName = $"{doctorFirstName} {doctorLastName}";
                            string patientFullName = $"{patientFirstName} {patientLastName}";

                            if (this.FullName == patientFullName)
                            {
                                Console.WriteLine($"{doctorFullName} | {patientFullName} | {description}");
                                appointmentsFound = true;
                            }
                        }
                    }

                    if (!appointmentsFound)
                    {
                        Console.WriteLine("You do not have any appointments");
                    }

                    Console.Write("\nPress any key to return to the menu: ");
                    Console.ReadKey();
                    DisplayMenu();
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
        }

        public void BookAppointment()
        {
            Console.Clear();

            Menu bookAppointment = new Menu();
            bookAppointment.Subtitle("Book Appointment");

            DoctorDatabase.LoadDoctorDB("doctors.txt");

            List<Doctor> allDoctors = DoctorDatabase.GetDoctorDatabase().Values.ToList();

            int totalDoctors = allDoctors.Count;

            if (totalDoctors == 0)
            {
                Console.WriteLine("There are no doctors available at the moment.\n");
                Console.Write("Press any key to return to the Patient Menu: ");
                Console.ReadKey();
                this.DisplayMenu();
            }

            Console.WriteLine("You are not registered with any doctor! Please choose which doctor you would like to register with\n");

            for (int i = 0; i < totalDoctors; i++)
            {
                Doctor doctor = allDoctors[i];

                /* 
                 * remember that doctor needs a Getter because the User fields are protected
                 * and the doctor object is not derived from User within the Patient class
                 */
                Console.WriteLine($"{i + 1}. {doctor.ToString()}");
            }

            string input;
            int selection;

            /* 
             * this do while loop keeps prompting the user for a description
             * while the input is null or empty
             */
            do
            {
                Console.Write($"\nPlease choose a doctor (1 to {totalDoctors}): ");

                /* 
                 * ? indicates that it can be null.
                 * The null-coalescing operator ?? returns the value of its left-hand operand if it isn't null; 
                 * otherwise, it evaluates the right-hand operand and returns its result.
                 */
                input = Console.ReadLine()?.Trim() ?? string.Empty;

            } while (!int.TryParse(input, out selection) || selection < 1 || selection > totalDoctors);

            Doctor selectedDoctor = allDoctors[selection - 1]; // -1 because the List starts at 0
            Console.WriteLine($"\nYou are booking an appointment with {selectedDoctor.GetFirstName()} {selectedDoctor.GetLastName()}\n");

            string description;

            do
            {
                Console.Write("Description of the appointment: ");
                description = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("The description cannot be blank\n");
                }

            } while (string.IsNullOrWhiteSpace(description));

            string textToFile = $"{selectedDoctor.GetFirstName()},{selectedDoctor.GetLastName()},{this.FirstName},{this.LastName},{description}";

            AddAppointment(selectedDoctor, description, textToFile);

            Console.WriteLine("The appointment was booked successfully\n\n");
            Console.Write("Press any key to return to the Patient Menu: ");

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
                    ListPatientDetails();
                    break;
                case "2":
                    ListMyDoctorDetails();
                    break;
                case "3":
                    ListAllAppointments();
                    break;
                case "4":
                    BookAppointment();
                    break;
                case "5":
                    Logout();
                    break;
                case "6":
                    Exit();
                    break;
                default:
                    Console.WriteLine("Please select an option between 1 and 6");
                    break;
            }
        }
    }
}