using System;
using HospitalSystem.Users;

namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Address doctorJacobAddress = new Address("456 Albert Ave", "Sydney", "NSW");
            Doctor doctorJacob = new Doctor("Jacob", "Smith", "d@smith.com", "234567890", doctorJacobAddress.ToString());

            Address doctorSamAddress = new Address("456 Albert Ave", "Sydney", "NSW");
            Doctor doctorSam = new Doctor("Sam", "Smith", "d@smith.com", "234567890", doctorSamAddress.ToString());

            Address patientAddress = new Address("123 Victoria Ave", "Sydney", "NSW");
            Patient patient = new Patient(doctorJacob, "Victor", "Zottmann", "v@z.com", "123456789", patientAddress.ToString());

            doctorJacob.AddPatient(patient);
            doctorSam.AddPatient(patient);

            patient.DisplayMenu();
        }
    }
}