using System;

namespace HospitalSystem
{
    public static class DatabaseFileManager
    {
        public static void RunLogin(string doctorsFile, string patientsFile, string doctorPatientsFile)
        {
            if (!File.Exists(doctorsFile) || !File.Exists(patientsFile) || !File.Exists(doctorPatientsFile))
            {
                Console.WriteLine("One or more database files are missing. Please make sure all required files are available.");
                Console.Write("\nPress any key to exit: ");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Login login = new Login(doctorsFile, patientsFile, doctorPatientsFile);
            login.LoginMenu();
        }
    }
}
