using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public class Doctor : User
    {
        private static string _appointmentsFilePath = "appointments.txt";
        private static string _doctorPatientsFilePath = "doctor-patients.txt";

        private int DoctorID { get; set; }

        private Dictionary<Doctor, List<Patient>> AssociatedPatients { get; }

        public Doctor()
        {
            this.AssociatedPatients = new Dictionary<Doctor, List<Patient>>();
        }

        public Doctor(int id, string firstName, string lastName, string email, string phone, string address)
            : base(firstName, lastName, email, phone, address)
        {
            this.DoctorID = id;
            this.AssociatedPatients = new Dictionary<Doctor, List<Patient>>();
        }

        public int GetDoctorId() => this.DoctorID;

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}, {this.Email}, {this.Phone}, {this.Address}";
        }

        public string[] ToStringArray()
        {
            return new string[]
            {
                this.FullName,
                this.Email,
                this.Phone,
                this.Address
            };
        }

        public void AssignPatient(Doctor doctor, Patient patient)
        {
            if (!AssociatedPatients.ContainsKey(doctor))
            {
                AssociatedPatients[doctor] = new List<Patient>();
            }

            AssociatedPatients[doctor].Add(patient);
        }

        public void ListDoctorDetails()
        {
            Console.Clear();

            Menu doctorDetails = new Menu();
            doctorDetails.Subtitle("My Details");

            List<string> tableHeaders = new List<string>()
            {
                "Name", "Email Address", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>
            {
                new string[] { this.FullName, this.Email, this.Phone, this.Address }
            };

            Utils.FormatTable(tableHeaders.ToArray(), tableRows);

            Console.Write("\nPress any key to the Doctor Menu: ");
            Console.ReadKey();

            Utils.ShowUserMenu(this);
        }

        public void ListPatients()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("My Patients");

            Console.WriteLine($"Patients assigned to {this.FullName}");

            List<string> tableHeaders = new List<string>()
            {
                "Patient", "Doctor", "Email Address", "Phone", "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            foreach (var kvp in AssociatedPatients)
            {
                Doctor doctor = kvp.Key;
                List<Patient> patients = kvp.Value;
                
                foreach (var patient in patients)
                {
                    tableRows.Add(new string[]
                    {
                        patient.FullName,
                        doctor.FullName,
                        patient.Email,
                        patient.Phone,
                        patient.Address
                    });
                }
            }

            Utils.FormatTable(tableHeaders.ToArray(), tableRows);

            Console.Write("\nPress any key to return to the Doctor Menu: ");
            Console.ReadKey();

            Utils.ShowUserMenu(this);
        }

        public void ListAppointments()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("All Appointments");

            try
            {
                if (File.Exists(_appointmentsFilePath))
                {
                    string[] lines = File.ReadAllLines(_appointmentsFilePath);

                    List<string> tableHeaders = new List<string>()
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
                            string patientFirstName = arr[4];
                            string patientLastName = arr[5];
                            string description = arr[6];

                            string doctorFullName = $"{doctorFirstName} {doctorLastName}";
                            string patientFullName = $"{patientFirstName} {patientLastName}";

                            if (this.FullName == doctorFullName)
                            {
                                appointmentsFound = true;
                                tableRows.Add(new string[] { doctorFullName, patientFullName, description });
                            }
                        }
                    }

                    Utils.FormatTable(tableHeaders.ToArray(), tableRows);

                    if (!appointmentsFound)
                    {
                        Console.WriteLine("You do not have any appointments");
                    }

                    Console.Write("\nPress any key to return to the menu: ");
                    Console.ReadKey();

                    Utils.ShowUserMenu(this);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
        }

        public void CheckPatient()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("Check Patient Details");

            List<string[]> tableRows = new List<string[]>();
            List<string> tableHeaders = new List<string>()
            {
                "Patient",
                "Doctor",
                "Email Address",
                "Phone",
                "Address"
            };

            Console.Write("Enter the ID of the patient to check: ");
            string id = Console.ReadLine()!.Trim();

            try
            {
                id = id.Trim();

                if ((int.TryParse(id, out int patientId) && patientId > 0) && id.Length == 5)
                {
                    Patient patient = PatientDatabase.GetPatientById(patientId);

                    bool noPatientDoctor = patient.GetPatientDoctor() == null;

                    if (patient != null)
                    {
                        string doctorName = noPatientDoctor ? "Not Assigned" : patient.GetPatientDoctor().FullName;

                        tableRows.Add(new string[]
                        {
                            patient.FullName,
                            doctorName,
                            patient.Email,
                            patient.Phone,
                            patient.Address
                        });

                        Utils.FormatTable(tableHeaders.ToArray(), tableRows);

                        // prompt to try again or return to menu
                        Utils.TryAgainOrReturn(this, CheckPatient);
                    }
                    else
                    {
                        // Patient with given ID does not exist
                        Console.WriteLine($"\nPatient with ID {id} does not exist.");

                        // prompt to try again or return to menu
                        Utils.TryAgainOrReturn(this, CheckPatient);
                    }
                }
                else
                {
                    // if length is not 5 chars and not a positive number
                    Console.WriteLine("\nInvalid ID. Please ensure that it has exactly 5 positive digits.");

                    // prompt to try again or return to menu
                    Utils.TryAgainOrReturn(this, CheckPatient);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }


        public void ListAppointmentsWithPatient()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("Appointments with Patient");

            Console.Write("Enter the ID of the patient you would like to view appointments for: ");

            try
            {
                int inputId = int.Parse(Console.ReadLine()!.Trim());

                if (File.Exists(_appointmentsFilePath))
                {
                    Doctor loggedInDoctor = this;
                    Patient patient = PatientDatabase.GetPatientById(inputId);

                    if (patient != null)
                    {
                        bool found = false;

                        string[] lines = File.ReadAllLines(_appointmentsFilePath);

                        List<string> tableHeaders = new List<string>()
                        {
                            "Doctor",
                            "Patient",
                            "Description"
                        };

                        List<string[]> tableRows = new List<string[]>();

                        foreach (var line in lines)
                        {
                            string[] arr = line.Split(',');

                            int doctorId = int.Parse(arr[0]);
                            string doctorFullName = $"{arr[1]} {arr[2]}";

                            int patientId = int.Parse(arr[3]);
                            string patientFullName = $"{arr[4]} {arr[5]}";

                            string description = arr[6];

                            // check if the ID of the doctor logged into the system matches that of the file
                            bool isLoggedDoctor = loggedInDoctor.DoctorID == doctorId;

                            // check if the patient IDs from the file and 
                            bool isPatientMatch = patientId == inputId;

                            if (isLoggedDoctor && isPatientMatch)
                            {
                                found = true;
                                tableRows.Add(new string[] { doctorFullName, patientFullName, description });
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("\nNo appointments found for this patient");
                            Utils.TryAgainOrReturn(this, ListAppointmentsWithPatient);
                        }
                        else
                        {
                            Utils.FormatTable(tableHeaders.ToArray(), tableRows);
                            Utils.TryAgainOrReturn(this, ListAppointmentsWithPatient);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\nPatient with ID {inputId} does not exist.");
                        Utils.TryAgainOrReturn(this, ListAppointmentsWithPatient);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"\nFile not found: {e.Message}");
                Utils.TryAgainOrReturn(this, ListAppointmentsWithPatient);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"\n{e.Message}");
                Utils.TryAgainOrReturn(this, ListAppointmentsWithPatient);
            }
        }

        public override void Logout()
        {
            this.AssociatedPatients.Clear();
            Login login = new Login();
            login.LoginMenu();
        }

        public override void Exit()
        {
            this.AssociatedPatients.Clear();
            base.Exit();
        }

        public override void ProcessSelectedOption(string input)
        {
            switch (input)
            {
                case "1":
                    ListDoctorDetails();
                    break;
                case "2":
                    ListPatients();
                    break;
                case "3":
                    ListAppointments();
                    break;
                case "4":
                    CheckPatient();
                    break;
                case "5":
                    ListAppointmentsWithPatient();
                    break;
                case "6":
                    Logout();
                    break;
                case "7":
                    Exit();
                    break;
                default:
                    Console.Write("\nPlease select an option between 1 and 7: ");
                    ProcessSelectedOption(Console.ReadLine()!);
                    break;
            }
        }
    }
}