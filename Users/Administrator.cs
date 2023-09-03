using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Users
{
    public class Administrator : User
    {
        private int AdministratorID = 30000;

        public Administrator(string firstName, string lastName, string email, string phone, string address) : base(firstName, lastName, email, phone, address)
        {
            AdministratorID++;
        }
    }
}
