﻿using System;
using System.Collections.Generic;
using HospitalSystem.Interfaces;

namespace HospitalSystem.Users
{
    public class User : IUserActions
    {
        protected string FirstName { get; set; }
        protected string LastName { get; set; }
        protected string Email { get; set; }
        protected string Phone { get; set; }
        protected string Address { get; set; }
        protected string FullName { get; set; }

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

        public string GetFirstName() => this.FirstName;
        public string GetLastName() => this.LastName;
        public string GetEmail() => this.Email;
        public string GetPhone() => this.Phone;
        public string GetAddress() => this.Address;
        public string GetFullName() => this.FullName;

        public void Logout()
        {
            Login login = new Login();
            login.DisplayMenu();
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}