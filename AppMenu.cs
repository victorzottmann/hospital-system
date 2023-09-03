﻿using System;

namespace HospitalSystem
{
    public class AppMenu
    {
        private string Title = "DOTNET Hospital Management System";

        public void MenuSubtitle(string subtitle)
        {
            // 33 chars for "DOTNET Hospital Management System"
            int titleLength = Title.Length; 

            //  5 chars for "Login"
            // 12 chars for "Patient Menu"
            int subtitleLength = subtitle.Length;
            
            // (33 - 5)  / 2 => padding = 14
            // (33 - 12) / 2 => padding = 10 (rounded down)
            int padding = (titleLength - subtitleLength) / 2;

            // shifting MainHeading by 3 spaces because of line 28 below
            Console.WriteLine($"   {this.Title}");

            // expanding the dashes by 6 characters so that the Title starts and stops before and after 3 dashes
            Console.WriteLine(new string('-', titleLength + 6));

            //"                       Login => 17 spaces + 5 chars (14 + 3 + 5)
            //"             Patient Menu => 13 spaces + 12 chars (10 + 3 + 12)
            Console.WriteLine($"{subtitle.PadLeft(padding + 3 + subtitleLength)}\n");
        }

        public void DisplayMenuOptions(string userFullName)
        {
            Console.WriteLine($"Welcome to DOTNET Hospital Management System {userFullName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit System");
        }
    }
}
