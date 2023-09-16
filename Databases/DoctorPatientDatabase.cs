using System;
using System.Collections.Generic;
using HospitalSystem.Users;


namespace HospitalSystem.Databases
{
    public class DoctorPatientDatabase
    {
        public static Dictionary<int, Doctor> DoctorPatients { get; set; } = new Dictionary<int, Doctor>();

        public static void LoadDB(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        string[]arr = line.Split(',');

                        /*
                         * File structure:
                         * docId,docName,docLastName,patId,patName,patLastName
                         * [0]  ,[1]    ,[2]        ,[3]  ,[4]    ,[5]
                         */
                        int doctorId = int.Parse(arr[0]);
                        int patientId = int.Parse(arr[3]);

                        Doctor doctor = DoctorDatabase.GetDoctorById(doctorId);
                        Patient patient = PatientDatabase.GetPatientById(patientId);

                        if (doctor != null && patient != null)
                        {
                            doctor.AssignPatient(doctor, patient);
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not found: {e.Message}");
            }
        }
    }
}
