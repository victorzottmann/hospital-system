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
            return $"{this.FirstName} {this.LastName},{this.Email},{this.Phone},{this.Address}";
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

        public void WriteToFile(string filepath, Doctor doctor, Patient patient)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string[] lines = File.ReadAllLines(filepath);

                    string doctorId = doctor.GetDoctorId().ToString();
                    string doctorFirstName = doctor.FirstName;
                    string doctorLastName = doctor.LastName;

                    string patientId = patient.GetPatientId().ToString();
                    string patientFirstName = patient.FirstName;
                    string patientLastName = patient.LastName;

                    int insertionIndex = -1;

                    /*
                     * For loop:
                     *      Starting from the last index (last line in the file),
                     *      it iterates through each line from the last line to the first line
                     *      until it reaches the first line (i >= 0)
                     * If Statement:
                     *      If a line starts with the target doctorId,
                     *      set the insertionIndex of the new line to i
                     *      Then break out of the loop once it assigns i to insertionIndex
                     *      of the doctorId
                     *      
                     *      NOTE: it will be appended to the bottom portion of the same doctorId because 
                     *      it's iterating in reverse order
                     * 
                     * NOTE: it must be lines.Length - 1 because line 1 is actually line 0 in terms of Arrays
                     * So, if a file has 27 lines, it's not that it goes from 1-27, but from 0-26
                     */
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        if (lines[i].StartsWith(doctorId))
                        {
                            insertionIndex = i;
                            break;
                        }
                    }

                    if (insertionIndex != -1)
                    {
                        string textToFile =
                            $"{doctorId}," +
                            $"{doctorFirstName}," +
                            $"{doctorLastName}," +
                            $"{patientId}," +
                            $"{patientFirstName}," +
                            $"{patientLastName}"
                        ;

                        // initialising the list with the elements of the lines array
                        List<string> updatedLines = new List<string>(lines);

                        /*
                         * Here it's necessary to add 1 to insertionIndex because, unlike an array,
                         * a file starts at line 1, not 0.
                         * 
                         * So, if you consider that the indexes of an array go from 0-26,
                         * and a text file actually goes from 1-27, you need to add 1 to the insertion index
                         * in order to correctly append the new line relative to the target doctorId.
                         */
                        updatedLines.Insert(insertionIndex + 1, textToFile);

                        File.WriteAllLines(_doctorPatientsFilePath, updatedLines);
                    }
                    else
                    {
                        Console.WriteLine($"Doctor with ID {doctorId} not found");
                    }
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
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

            Utilities.FormatTable(tableHeaders.ToArray(), tableRows);

            Console.Write("\n\nPress any key to the Doctor Menu: ");
            Console.ReadKey();

            Utilities.ShowUserMenu(this);
        }

        public void ListPatients()
        {
            Console.Clear();

            Menu myPatients = new Menu();
            myPatients.Subtitle("My Patients");

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

            Utilities.FormatTable(tableHeaders.ToArray(), tableRows);

            Console.Write("\n\nPress any key to return to the Doctor Menu: ");
            Console.ReadKey();

            Utilities.ShowUserMenu(this);
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

                    if (!appointmentsFound)
                    {
                        Console.WriteLine("You do not have any appointments");
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

        public void CheckPatient()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("Check Patient Details");

            Console.Write("Enter the ID of the patient to check: ");
            string id = Console.ReadLine()!.Trim();

            try
            {
                if (int.TryParse(id, out int patientId))
                {
                    Patient patient = PatientDatabase.GetPatientById(patientId);

                    List<string[]> tableRows = new List<string[]>();
                    List<string> tableHeaders = new List<string>()
                    {
                        "Patient",
                        "Doctor",
                        "Email Address",
                        "Phone",
                        "Address"
                    };

                    if (patient != null)
                    {
                        tableRows.Add(new string[]
                            {
                                patient.FullName,
                                patient.GetPatientDoctor().FullName,
                                patient.Email,
                                patient.Phone,
                                patient.Address
                            }
                        );

                        Utilities.FormatTable(tableHeaders.ToArray(), tableRows);

                        // prompt to try again or return to menu
                        Utilities.TryAgainAndReturn(this, CheckPatient);
                    }
                    else
                    {
                        // not null by format doesn't match 1xxxx
                        Console.WriteLine($"\nPatient with ID {id} does not exist.");

                        // prompt to try again or return to menu
                        Utilities.TryAgainAndReturn(this, CheckPatient);
                    }
                }
                else
                {
                    // if not an integer
                    Console.WriteLine("\nInvalid ID. Only numeric values are accepted.");

                    // prompt to try again or return to menu
                    Utilities.TryAgainAndReturn(this, CheckPatient);
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
                            Utilities.TryAgainAndReturn(this, ListAppointmentsWithPatient);
                        }
                        else
                        {
                            Utilities.FormatTable(tableHeaders.ToArray(), tableRows);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\nPatient with ID {inputId} does not exist.");
                        Utilities.TryAgainAndReturn(this, ListAppointmentsWithPatient);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
                Utilities.TryAgainAndReturn(this, ListAppointmentsWithPatient);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"\n{e.Message}");
                Utilities.TryAgainAndReturn(this, ListAppointmentsWithPatient);
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