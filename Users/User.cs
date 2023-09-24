using System;
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