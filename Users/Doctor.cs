using System;
using System.IO;
using System.Collections.Generic;

namespace HospitalSystem
{
    public class Doctor : User
    {
        // this file is stored at ./bin/Debug/net6.0
        private static string _appointmentsFilePath = "appointments.txt";

        // these are public to enable easy access through auto-implemented properties
        public int DoctorID { get; private set; }
        public Dictionary<Doctor, List<Patient>> AssociatedPatients { get; private set; }

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

        public void AssignPatient(Doctor doctor, Patient patient)
        {
            // if the AssociatedPatients Dictionary doesn't have the current doctor as a key,
            // add it and assign a new List of patient as the value
            if (!AssociatedPatients.ContainsKey(doctor))
            {
                AssociatedPatients[doctor] = new List<Patient>();
            }

            // if the doctor in the AssociatedPatients Dictionary is not linked to the target patient, add that patient to it
            // otherwise return nothing
            if (!AssociatedPatients[doctor].Contains(patient))
            {
                AssociatedPatients[doctor].Add(patient);
            }
            else
            { 
                return;
            }
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

            // ensure that the Dictionary is not empty
            if (AssociatedPatients.Count > 0)
            {
                List<string> tableHeaders = new List<string>()
                {
                    "Patient", "Doctor", "Email Address", "Phone", "Address"
                };

                List<string[]> tableRows = new List<string[]>();

                // extract both the doctor and their patients for each entry in AssociatedPatients
                foreach (var kvp in AssociatedPatients)
                {
                    Doctor doctor = kvp.Key;
                    List<Patient> patients = kvp.Value;
                
                    // then iterate through each patient and add their respective data into the
                    // tableRows list, including their doctor full name
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
            }
            else
            {
                // if the current doctor has no associated patients, don't format the table but show this instead
                Console.WriteLine("\nThere are no patients assigned to you yet...");
            }

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

                        // a current limitation is that it is assumed that the file will always be populated 
                        if (arr.Length > 0)
                        {
                            int doctorId = int.Parse(arr[0]);
                            string doctorFirstName = arr[1];
                            string doctorLastName = arr[2];
                            string patientFirstName = arr[4];
                            string patientLastName = arr[5];
                            string description = arr[6];

                            string doctorFullName = $"{doctorFirstName} {doctorLastName}";
                            string patientFullName = $"{patientFirstName} {patientLastName}";

                            // only add the details to tableRows if the ID of the current doctor
                            // matches the one being read from the file
                            if (this.DoctorID == doctorId)
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

            List<string> tableHeaders = new List<string>()
            {
                "Patient",
                "Doctor",
                "Email Address",
                "Phone",
                "Address"
            };

            List<string[]> tableRows = new List<string[]>();

            Console.Write("Enter the ID of the patient to check: ");
            string id = Console.ReadLine()!.Trim();

            try
            {
                /*
                 * This converts the id into an int and assigns it to patientId, and checks if patientId is positive.
                 * Then it also checks if the length of the string id is equal to 5.
                 * This is important because the ids must have a length of 5 digits.
                 * If all of the above is true, then proceed.
                 */
                if ((int.TryParse(id, out int patientId) && patientId > 0) && id.Length == 5)
                {
                    Patient patient = PatientDatabase.GetPatientById(patientId);

                    bool noPatientDoctor = patient.GetPatientDoctor() == null;

                    if (patient != null)
                    {
                        // print "Not Assigned" in the Doctor column if a patient has not been assigned to a doctor yet
                        string doctorName = noPatientDoctor ? "Not Assigned" : patient.GetPatientDoctor().FullName;

                        tableRows.Add(new string[]
                        {
                            patient.FullName,
                            doctorName, // either their name or "Not Assigned"
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
                            "Doctor", "Patient", "Description"
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

                            // only add the details to tableRows if the relationship between the doctor and the patient exists
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