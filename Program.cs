using System;

namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // the files are stored at ./bin/Debug/net6.0
            string doctorsFile = "doctors.txt";
            string patientsFile = "patients.txt";
            string doctorPatientsFile = "doctor-patients.txt";

            DatabaseFileManager.RunLogin(doctorsFile, patientsFile, doctorPatientsFile);
        }
    }
}