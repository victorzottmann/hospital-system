using System;
using HospitalSystem.Users;

namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Address doctorAddress = new Address("456 Albert Ave", "Sydney", "NSW");
            Address patientAddress = new Address("123 Victoria Ave", "Sydney", "NSW");

            Doctor doctor = new Doctor("Jacob", "Smith", "d@smith.com", "234567890", doctorAddress.ToString());
            Patient patient = new Patient(doctor, "Victor", "Zottmann", "v@z.com", "123456789", patientAddress.ToString());

            doctor.AddPatient(patient);

            patient.DisplayMenu();
        }
    }
}