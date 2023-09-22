using System;
using System.Collections.Generic;
using HospitalSystem.Interfaces;

namespace HospitalSystem.Users
{
    public abstract class User : IUserActions
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public string FullName { get; private set; }

        public User() { }

        public User(string firstName, string lastName) 
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = $"{this.FirstName} {this.LastName}";
        }

        public User(string firstName, string lastName, string email, string phone, string address)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Phone = phone;
            this.Address = address;
            this.FullName = $"{this.FirstName} {this.LastName}";
        }

        public void SetFirstName(string firstName)
        {
            this.FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            this.LastName = lastName;
        }

        public void Logout()
        {
            Login login = new Login();
            login.DisplayMenu();
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public abstract void DisplayMenu();
        public abstract void ProcessSelectedOption(string input);
    }
}