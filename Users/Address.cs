using System;

namespace HospitalSystem
{
    // It was preferred to isolate the Address into a class in order to minimise 
    // the complexity of creating a new address a new user in the User class
    public class Address
    {
        public string StreetNumber { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        public Address(string streetNumber, string street, string city, string state)
        {
            this.StreetNumber = streetNumber;
            this.Street = street;
            this.City = city;
            this.State = state;
        }

        public override string ToString() => $"{this.StreetNumber} {this.Street}, {this.City}, {this.State}";
    }
}