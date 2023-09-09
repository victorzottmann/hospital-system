using System;
using System.Collections.Generic;

namespace HospitalSystem.Users
{
    public class User
    {
        protected string FirstName { get; set; }
        protected string LastName { get; set; }
        protected string Email { get; set; }
        protected string Phone { get; set; }
        protected string Address { get; set; }

        public User() { }

        public User(string firstName, string lastName, string email, string phone, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public string GetFirstName() => this.FirstName;
        public string GetLastName() => this.LastName;
        public string GetEmail() => this.Email;
        public string GetPhone() => this.Phone;
        public string GetAddress() => this.Address;
    }
}
