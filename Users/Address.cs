using System;

namespace HospitalSystem
{
    public class Address
    {
        private string StreetNumber { get; set; }
        private string Street { get; set; }
        private string City { get; set; }
        private string State { get; set; }

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
