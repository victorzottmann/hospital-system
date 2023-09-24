﻿using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public abstract class User : IUser
    {
        // to disable CS8618 non-nullable warnings in constructors
        // that don't initialise all fields
        #nullable disable

        /*
         * Using auto-implemented properties with felt more appropriate
         * in order to make it easier to access these fields without getter and setter methods
         * { private set; } restricts assigning a new value only from within this class.
         */
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

        /*
         * This method creates an instance of either a Doctor or a Patient.
         * It is particularly helpful for loading the corresponding database of either type of user
         * 
         * For example, this would be the same as either:
         * 
         * Doctor doctor = new Doctor(doctorId, firstName, lastName, email, phone, address.ToString());
         * Patient patient = new Patient(patientId, firstName, lastName, email, phone, address.ToString());                 
         * 
         * Please refer to ./Utils/DBUtils.cs for reference of its usage
         * 
         * For reference: https://learn.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=net-7.0
         */
        public static T CreateInstanceOfUser<T>(int userId, string firstName, string lastName, string email, string phone, string address) where T : User
        {
            return (T)Activator.CreateInstance(typeof(T), userId, firstName, lastName, email, phone, address);
        }

        // Setter methods for setting a User's first and last names are required because
        // modifying their value is needed in the
        // class
        public void SetFirstName(string firstName)
        {
            this.FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            this.LastName = lastName;
        }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}, {this.Email}, {this.Phone}, {this.Address}";
        }

        public string[] ToStringArray()
        {
            return new string[] { this.FullName, this.Email, this.Phone, this.Address };
        }
        
        // declared these methods as virtual in case they need to be overriden in derived classes
        public virtual void Logout()
        {  
            Login login = new Login();
            login.LoginMenu();
        }

        public virtual void Exit()
        {
            Environment.Exit(0);
        }

        // this method must be abstract because each derived class from User has its own
        // custom implementation of it
        public abstract void ProcessSelectedOption(string input);
    }
}