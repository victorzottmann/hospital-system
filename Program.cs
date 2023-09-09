using System;
using HospitalSystem.Users;

namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Address doctorJacobAddress = new Address("456 Albert Ave", "Sydney", "NSW");
            //Doctor doctorJacob = new Doctor("Jacob", "Hirsch", "j@hirsch.com", "234567890", doctorJacobAddress.ToString());

            //Address doctorSamAddress = new Address("789 Elizabth Street", "Sydney", "NSW");
            //Doctor doctorSam = new Doctor("Sam", "Smith", "d@smith.com", "345678901", doctorSamAddress.ToString());

            Address patientAddress = new Address("123 Victoria Ave", "Sydney", "NSW");
            Patient patient = new Patient("Victor", "Zottmann", "v@z.com", "123456789", patientAddress.ToString());

            //patient.AddAppointment(doctorJacob, "cold symptoms");
            //patient.AddAppointment(doctorSam, "regular checkup with doc");

            //patient.DisplayMenu();

            //Login login = new Login();
            //login.DisplayMenu();

            Administrator admin = new Administrator();
        }
    }
}