using System;
using System.Collections.Generic;
using System.Numerics;

namespace HospitalSystem
{
    public class Patient : User
    {
        private static string _appointmentsFilePath = "appointments.txt";
        private static string _doctorPatientsFilePath = "doctor-patients.txt";

        private int PatientID { get; set; }
        private Doctor PatientDoctor { get; set; }

        private Dictionary<Doctor, List<string>> DoctorAppointments { get; } = new Dictionary<Doctor, List<string>>();

        public Patient(int id, string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            PatientID = id;
        }

        public int GetPatientId() => this.PatientID;

        public Doctor GetPatientDoctor() => this.PatientDoctor;

        public void AssignDoctor(Doctor doctor)
        {
            PatientDoctor = doctor;
        }

        public Dictionary<Doctor, List<string>> GetDoctorAppointments() => this.DoctorAppointments;

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}, {this.Email}, {this.Phone}, {this.Address}";
        }

        public string[] ToStringArray()
        {
            return new string[] { this.FullName, this.Email, this.Phone, this.Address };
        }

        private void AddAppointment(Doctor doctor, string description, string textToFile)
        {
            if (!DoctorAppointments.ContainsKey(doctor))
            {
                DoctorAppointments[doctor] = new List<string>();
            }

            DoctorAppointments[doctor].Add(description);

            doctor.AssignPatient(doctor, this);

            File.AppendAllText(_appointmentsFilePath, textToFile + Environment.NewLine);
        }

        private void ShowPatientDoctor(Doctor doctor)
        {
            List<string> tableHeaders;
            List<string[]> tableRows = new List<string[]>();

            tableHeaders = new List<string>()
            {
                "Name", "Email", "Phone", "Address"
            };

            tableRows.Add(doctor.ToStringArray());

            Console.WriteLine("Your doctor:");
            Utilities.FormatTable(tableHeaders.ToArray(), tableRows);

            Console.WriteLine("\nPress any key to return to the Patient Menu: ");
            Console.ReadKey();

            Utilities.ShowUserMenu(this);
        }

        private void ShowDoctorTable(List<Doctor> doctors)
        {
            List<string> tableHeaders = new List<string>()
            {
                "No.", "Name", "Email", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            int totalDoctors = doctors.Count;

            for (int i = 0; i < totalDoctors; i++)
            {
                Doctor doctor = doctors[i];
                string[] doctorDetails = doctor.ToStringArray();

                string[] row = new string[]
                {
                    $"{i + 1}. ", doctorDetails[0], doctorDetails[1], doctorDetails[2], doctorDetails[3]
                };

                tableRows.Add(row);
            }

            Utilities.FormatTable(tableHeaders.ToArray(), tableRows);
        }

        private Doctor SelectDoctor(List<Doctor> doctors)
        {
            ShowDoctorTable(doctors);

            string input;
            int selection;

            int totalDoctors = doctors.Count();

            /* 
             * this do while loop keeps prompting the user for a description
             * while the input is null or empty
             */
            do
            {
                Console.Write($"\nPlease select a doctor (1 to {totalDoctors}), or press 'N' to exit: ");

                /* 
                 * The null-coalescing operator ?? returns the value of its left-hand operand if it isn't null; 
                 * otherwise, it evaluates the right-hand operand and returns its result.
                 */
                input = Console.ReadLine()!.Trim().ToLower() ?? string.Empty;

                if (input == "n")
                {
                    Utilities.ShowUserMenu(this);
                }
            }
            while (!int.TryParse(input, out selection) || selection < 1 || selection > totalDoctors);

            Doctor selectedDoctor = doctors[selection - 1]; // -1 because the List starts at 0

            return selectedDoctor;
        }

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

            Utilities.ShowUserMenu(this);
        }

        public void ListMyDoctorDetails()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("My Doctor");

            List<Doctor> doctors = DoctorDatabase.GetDoctorDatabase().Values.ToList();
            
            Doctor patientDoctor = this.GetPatientDoctor();

            if (patientDoctor != null)
            {
                ShowPatientDoctor(patientDoctor);
            }
            else
            {
                bool confirmed = false;
                Doctor selectedDoctor;

                Console.WriteLine("\nIt appears that you do not have a doctor yet.");

                do
                {
                    selectedDoctor = SelectDoctor(doctors);
                    Console.WriteLine($"\nYou selected Dr. {selectedDoctor.FirstName} {selectedDoctor.LastName}");

                    Console.Write("\nPress 1 to confirm, or '0' to select a different doctor: ");
                    string input = Console.ReadLine()!;

                    if (input == "1")
                    {
                        confirmed = true;
                    }
                    else if (input != "0")
                    {
                        Console.WriteLine("Invalid input. Please enter either '1' or '0'.");
                    }
                } while (!confirmed);

                ConfirmDoctor(selectedDoctor);
            }
        }

        private void ConfirmDoctor(Doctor selectedDoctor)
        {
            AssignDoctor(selectedDoctor);
            Utilities.WriteToFile(_doctorPatientsFilePath, selectedDoctor, this);

            Console.WriteLine("Confirmed!\n");
            Console.Write("Press any key to return to the Patient Menu: ");

            Console.ReadKey();
            Utilities.ShowUserMenu(this);
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

                    List<string> headers = new List<string>
                    { 
                        "Doctor", "Patient", "Description"
                    };

                    List<string[]> tableRows = new List<string[]>();

                    bool appointmentsFound = false;

                    foreach (string line in lines)
                    {
                        string[] arr = line.Split(',');

                        if (arr.Length > 0)
                        {
                            string doctorFirstName = arr[1];
                            string doctorLastName = arr[2];

                            string patientId = arr[3];
                            string patientFirstName = arr[4];
                            string patientLastName = arr[5];

                            string description = arr[6];

                            string doctorFullName = $"{doctorFirstName} {doctorLastName}";
                            string patientFullName = $"{patientFirstName} {patientLastName}";

                            if (this.PatientID == int.Parse(patientId))
                            {
                                appointmentsFound = true;
                                tableRows.Add(new string[] { doctorFullName, patientFullName, description });
                            }
                        }
                    }

                    if (!appointmentsFound)
                    {
                        Console.WriteLine("You do not have any appointments");
                    }
                    else
                    {
                        Utilities.FormatTable(headers.ToArray(), tableRows);
                    }

                    Console.Write("\nPress any key to return to the menu: ");
                    Console.ReadKey();

                    Utilities.ShowUserMenu(this);
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

            Menu menu = new Menu();
            menu.Subtitle("Book Appointment");

            List<Doctor> doctors = DoctorDatabase.GetDoctorDatabase().Values.ToList();
            
            Doctor selectedDoctor = SelectDoctor(doctors);
            Console.WriteLine($"\nYou are booking an appointment with {selectedDoctor.FirstName} {selectedDoctor.LastName}\n");

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

            string textToFile =
                $"{selectedDoctor.GetDoctorId()}," +
                $"{selectedDoctor.FirstName}," +
                $"{selectedDoctor.LastName}," +
                $"{this.GetPatientId()}," +
                $"{this.FirstName}," +
                $"{this.LastName}," +
                $"{description}"
            ;

            AddAppointment(selectedDoctor, description, textToFile);

            Console.WriteLine("The appointment was booked successfully\n\n");
            Console.Write("Press any key to return to the Patient Menu: ");

            Console.ReadKey();
            Utilities.ShowUserMenu(this);
        }

        public override void ProcessSelectedOption(string input)
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