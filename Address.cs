using System;

namespace HospitalSystem
{
    public class Address
    {
        private string Street { get; set; }
        private string City { get; set; }
        private string State { get; set; }

        public Address(string street, string city, string state)
        {
            this.Street = street;
            this.City = city;
            this.State = state;
        }

        public override string ToString() => $"{this.Street}, {this.City}, {this.State}";
    }
}
