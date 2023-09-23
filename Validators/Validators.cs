using System;
using System.Text.RegularExpressions;

namespace HospitalSystem
{
    public class Validators
    {
        public static bool ValidateName(string name)
        {
            string formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
            bool isValid = Regex.IsMatch(formattedName, @"^[A-Z][a-z]{1,30}$");
            return isValid;

        }

        public static bool ValidateEmail(string email)
        {
            bool isValid = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
            return isValid;

        }

        public static bool ValidatePhone(string phone)
        {
            bool isValid = Regex.IsMatch(phone, @"^\d{10}$");
            return isValid;
        }

        public static bool ValidateStreetNumber(string streetNumber)
        {
            bool isValid = Regex.IsMatch(streetNumber, @"^\d{1,3}$");
            return isValid;
        }

        public static bool ValidateStreet(string street)
        {
            string formattedStreet = CapitaliseString(street);
            bool isValid = Regex.IsMatch(formattedStreet, @"^[A-Z][a-z]+( [A-Z][a-z]+)*$");
            return isValid;

        }

        public static bool ValidateCity(string city)
        {
            string formattedCity = CapitaliseString(city);
            bool isValid = Regex.IsMatch(formattedCity, @"^[A-Za-z]+$");
            return isValid;

        }

        public static bool ValidateState(string state)
        {
            string formattedState = state.ToUpper();
            bool isValid = Regex.IsMatch(formattedState, @"^[A-Z]{1,3}$");
            return isValid;
        }

        public static string CapitaliseString(string str)
        {
            string[] words = str.Split(" ");
            List<string> capitalisedWords = new List<string>();

            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    string capitalisedWord = char.ToUpper(word[0]) + word.Substring(1).ToLower();
                    capitalisedWords.Add(capitalisedWord);
                }
            }

            return string.Join(" ", capitalisedWords);
        }
    }
}