using System;
using System.Numerics;
using System.Collections.Generic;
using System.Xml;

namespace HospitalSystem
{
    public class Patient : User
    {
        // these files are stored at ./bin/Debug/net6.0
        private static string _appointmentsFilePath = "appointments.txt";
        private static string _doctorPatientsFilePath = "doctor-patients.txt";

        private int PatientID { get; set; }
        private Doctor PatientDoctor { get; set; }
        private Dictionary<Doctor, List<string>> DoctorAppointments { get; } = new Dictionary<Doctor, List<string>>();

        public Patient() { }

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

        private void AddAppointment(Doctor doctor, string description, string textToFile)
        {
            // in case the dictionary doesn't have any appointments for a given doctor,
            // add a new key as the doctor object, and a value as a list
            if (!DoctorAppointments.ContainsKey(doctor))
            {
                DoctorAppointments[doctor] = new List<string>();
            }

            // the key is the doctor object, and value a list of descriptions
            // this is because a doctor can have multiple appointments
            DoctorAppointments[doctor].Add(description);

            List<string> existingAppointments = File.ReadAllLines(_appointmentsFilePath).ToList();

            int insertionIndex = -1;
            bool doctorExists = false;
            int doctorId = doctor.GetDoctorId();

            for (int i = existingAppointments.Count - 1; i >= 0; i--)
            {
                // check if the first "cell" in a given line starts with the doctor ID
                if (existingAppointments[i].StartsWith(doctorId.ToString()))
                {
                    // if so, target that line as the insertionIndex
                    doctorExists = true;
                    insertionIndex = i;
                    break;
                }
            }

            if (doctorExists)
            {
                // create a new list with all the existing appointments
                List<string> updatedLines = new List<string>(existingAppointments);

                /*
                 * Insert the new appointment below the target line
                 * the +1 moves the insertionIndex beneath the target line
                 * Environment.Newline is not needed in this case, it'll add a new blank line after
                 * the newly inserted line
                 */
                updatedLines.Insert(insertionIndex + 1, textToFile);

                File.WriteAllLines(_appointmentsFilePath, updatedLines);
            }
            else
            {
                // If the doctor doesn't exist, then just append the appointment at the end of the file.
                // This assumes that every new doctor will have an ID larger than the largest one found in the file.
                File.AppendAllText(_appointmentsFilePath, textToFile + Environment.NewLine);
            }
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
            Utils.FormatTable(tableHeaders.ToArray(), tableRows);

            Console.Write("\nPress any key to return to the Patient Menu: ");
            Console.ReadKey();

            Utils.ShowUserMenu(this);
        }

        private void ShowDoctorsTable(List<Doctor> doctors)
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

                // Using toStringArray() here because the details need to be loaded into an array
                // instead of just being printed as a single string
                string[] doctorDetails = doctor.ToStringArray();

                string[] row = new string[]
                {
                    // The index is being included in order to correctly match the 'No.' column in the headers
                    $"{i + 1}. ", doctorDetails[0], doctorDetails[1], doctorDetails[2], doctorDetails[3]
                };

                tableRows.Add(row);
            }

            Utils.FormatTable(tableHeaders.ToArray(), tableRows);
        }

        private Doctor SelectDoctor(List<Doctor> doctors)
        {
            ShowDoctorsTable(doctors);

            string input;
            int selection;

            int totalDoctors = doctors.Count();

            /* 
             * this do while loop keeps prompting the user for a description
             * while the input is null or empty
             */
            do
            {
                Console.Write($"\nEnter a number from 1 to {totalDoctors} to select, or press 'N' to exit: ");

                /* 
                 * The null-coalescing operator ?? returns the value of its left-hand operand if it isn't null; 
                 * otherwise, it evaluates the right-hand operand and returns its result.
                 */
                input = Console.ReadLine()!.Trim().ToLower() ?? string.Empty;

                if (input == "n")
                {
                    Utils.ShowUserMenu(this);
                }
            }
            /*
             * This while loop tries to parse the input and output it as an int to selection
             * It also keeps on looping if the selection is less than 1, or if it's larger than the 
             * total number of doctors available in the list
             */
            while (!int.TryParse(input, out selection) || selection < 1 || selection > totalDoctors);

            Doctor selectedDoctor = doctors[selection - 1]; // -1 because the List starts at 0

            return selectedDoctor;
        }

        private void ConfirmDoctor(Doctor selectedDoctor)
        {
            // once a doctor is selected, assign it to the patient and include the relationship in the
            // doctor-patients.txt file
            AssignDoctor(selectedDoctor);

            // also assign the current patient to the selected doctor
            selectedDoctor.AssignPatient(selectedDoctor, this);
            Utils.WriteToFile(_doctorPatientsFilePath, selectedDoctor, this);
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

            Console.Write("\nPress any key to the Patient Menu: ");
            Console.ReadKey();

            Utils.ShowUserMenu(this);
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

                Console.WriteLine("It appears that you do not have a doctor yet.\n");
                Console.WriteLine("Please select one from the list below:");

                /*
                 * A do...while seemed appropriate here because if a patient doesn't have a doctor yet,
                 * they are prompted to choose one from the list
                 */
                do
                {
                    // run the list of doctors, prompt the user to select one, then assign the
                    // return value to selectedDoctor
                    selectedDoctor = SelectDoctor(doctors);

                    Console.WriteLine($"\nYou selected Dr. {selectedDoctor.FirstName} {selectedDoctor.LastName}");
                    
                    // provide the patient a chance to change their mind before confirming
                    Console.Write("\nPress 1 to confirm, or '0' to select a different doctor: ");
                    
                    string input = Console.ReadLine()!;

                    if (input == "1")
                    {
                        confirmed = true;
                    }
                    else if (input == "0")
                    {
                        ListMyDoctorDetails();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter either '1' or '0'.");
                    }

                    // keep prompting for a doctor to be selected while confirmed = false
                } while (!confirmed);

                if (confirmed)
                {
                    ConfirmDoctor(selectedDoctor);

                    Console.WriteLine("Confirmed!\n");
                    Console.Write("Press any key to return to the Patient Menu: ");

                    Console.ReadKey();
                    Utils.ShowUserMenu(this);
                }
            }
        }

        public void ListAllAppointments()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("My Appointments");

            Console.WriteLine($"Appointments for {this.FullName}");

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

                    /*
                     * File structure:
                     * 
                     * [0]      [1]             [2]            [3]       [4]              [5]             [6]
                     * doctorId,doctorFirstName,doctorLastName,patientId,patientFirstName,patientLastName,description
                     */
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

                            // only add the data to the table if the current patient ID matches that of the file
                            if (this.PatientID == int.Parse(patientId))
                            {
                                appointmentsFound = true;
                                tableRows.Add(new string[] { doctorFullName, patientFullName, description });
                            }
                        }
                    }

                    if (!appointmentsFound)
                    {
                        Console.WriteLine("\nYou do not have any appointments");
                    }
                    else
                    {
                        Utils.FormatTable(headers.ToArray(), tableRows);
                    }

                    Console.Write("\nPress any key to return to the menu: ");
                    Console.ReadKey();

                    Utils.ShowUserMenu(this);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"\nFile not found: {e.Message}");
            }
        }

        public void BookAppointment()
        {
            Console.Clear();

            Menu menu = new Menu();
            menu.Subtitle("Book Appointment");

            try
            {
                List<Doctor> doctors = DoctorDatabase.GetDoctorDatabase().Values.ToList();
                
                // SelectDoctor displays a list of all doctors and after the patient selects it,
                // the doctor is stored in selectedDoctor 
                Doctor selectedDoctor = SelectDoctor(doctors);
                Console.WriteLine($"\nYou are booking an appointment with {selectedDoctor.FirstName} {selectedDoctor.LastName}\n");

                string description;

                // keep prompting for a description while the input is invalid
                while (true)
                { 
                    Console.Write("Description of the appointment: ");
                    description = Console.ReadLine()!.Trim();

                    // only break the loop if the description is not null or blank
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        break;
                    }

                    Console.WriteLine("The description cannot be blank!\n");
                }

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

                Console.WriteLine("The appointment was booked successfully\n");
                Console.Write("Press any key to return to the Patient Menu: ");

                Console.ReadKey();
                Utils.ShowUserMenu(this);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nAn error occured: {e.Message}");
            }
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