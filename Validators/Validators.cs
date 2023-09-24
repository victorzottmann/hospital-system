using System;
using System.Text.RegularExpressions;

namespace HospitalSystem
{
    public class Validators
    {
        public static bool ValidateName(string name)
        {
            // capitalise the first letter
            string formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();

            /*
             * Regex breakdown:
             * ^           => Asserts the beginning of the string
             * [A-Z]       => Matches a single uppercase letter from A-Z
             * [a-z]{1,30} => Matches between 1 and 30 lowercase letters from a-z
             */
            bool isValid = Regex.IsMatch(formattedName, @"^[A-Z][a-z]{1,30}$");
            return isValid;

        }

        public static bool ValidateEmail(string email)
        {
            /*
             * Regex breakdown:
             * ^     => Asserts the beginning of the string
             * [^@]+ => Matches 1 or more characters that are not @
             * @     => Matches an @
             * [^@]+ => Matches 1 or more characters that are not @
             * \.    => Matches a literal "." character; the \ escapes the special meaning of "."
             * [^@]+ => Matches 1 or more characters that are not @
             * $     => Asserts the end of the string
             */
            bool isValid = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
            return isValid;

        }

        public static bool ValidatePhone(string phone)
        {
            /*
             * Regex breakdown:
             * ^      => Asserts the beginning of the string
             * \d{10} => Matches 10 digits(0-9)
             * $      => Asserts the end of the string
             */
            bool isValid = Regex.IsMatch(phone, @"^\d{10}$");
            return isValid;
        }

        public static bool ValidateStreetNumber(string streetNumber)
        {
            /*
             * Regex breakdown:
             * ^       => Asserts the beginning of the string
             * \d{1,3} => Matches between 1 and 3 occurrences of a digit(0-9)
             * $       => Asserts the end of the string
             */
            bool isValid = Regex.IsMatch(streetNumber, @"^\d{1,3}$");
            return isValid;
        }

        public static bool ValidateStreet(string street)
        {
            // capitalise every word of the street
            string formattedStreet = CapitaliseString(street);

            /*
             * Regex breakdown:
             * ^               => Asserts the beginning of the string
             * [A-Z]           => Matches a single uppercase letter
             * [a-z]+          => Matches 1 or more lowercase letters
             * ( [A-Z][a-z]+)* => Matches 0 or more occurrences of a space followed by 
             *                    an uppercase and 1 or more lowercase letters.
             *                    The () envelop everything inside it as a single expression
             * $               => Asserts the end of the string
             */
            bool isValid = Regex.IsMatch(formattedStreet, @"^[A-Z][a-z]+( [A-Z][a-z]+)*$");
            return isValid;

        }

        public static bool ValidateCity(string city)
        {
            // capitalise every word of the city
            string formattedCity = CapitaliseString(city);

            /*
             * Regex breakdown:
             * ^      => Asserts the beginning of the string
             * [A-Z]  => Matches a single uppercase letter
             * [a-z]+ => Matches 1 or more lowercase letters
             * $      => Asserts the end to the string
             */
            bool isValid = Regex.IsMatch(formattedCity, @"^[A-Z][a-z]+$");
            return isValid;

        }

        public static bool ValidateState(string state)
        {
            // make the entire state uppercase
            string formattedState = state.ToUpper();

            /*
             * Regex breakdown:
             * ^          => Asserts the beginning of the string
             * [A-Z]{2,3} => Matches between 2 and 3 occurrences of uppercase letters
             * $          => Asserts the end of the string
             */
            bool isValid = Regex.IsMatch(formattedState, @"^[A-Z]{2,3}$");
            return isValid;
        }

        public static string CapitaliseString(string str)
        {
            string[] words = str.Split(" ");

            // a list is necessary in case the string contains more than 1 word
            List<string> capitalisedWords = new List<string>();

            foreach (string word in words)
            {
                // ensure that the string is not blank
                if (word.Length > 0)
                {
                    // isolate the first character, capitalise it, then add it to the rest of each word
                    string capitalisedWord = char.ToUpper(word[0]) + word.Substring(1).ToLower();
                    capitalisedWords.Add(capitalisedWord);
                }
            }

            // join all the words stored in capitalisedWords by a space
            return string.Join(" ", capitalisedWords);
        }
    }
}