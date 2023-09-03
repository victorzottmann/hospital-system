using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Users
{
    public class Doctor : User
    {
        private int DoctorID = 20000;

        public Doctor(string firstName, string lastName, string email, string phone, string address) : base(firstName, lastName, email, phone, address)
        {
            DoctorID++;
        }
    }
}
