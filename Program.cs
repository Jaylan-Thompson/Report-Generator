using System;
using System.Collections.Generic;
using System.IO;

namespace ReportGenerator
{
    class Program
    {
        static List<string> usedTicketNumbers = new List<string>(); // Move the list outside of the Main method to retain its state

        static void Main(string[] args)
        {
            // Capture user's credentials
            Console.WriteLine("Enter First Name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter Last Name: ");
            string lastName = Console.ReadLine();

            int year, month, day;
            DateTime birthdate = DateTime.MinValue; // Initialize with a default value
            bool validDate = false;

            do
            {
                if (!validDate)
                {
                    Console.WriteLine("Invalid date format. Please enter your Date of Birth.");
                }

                Console.Write("Enter Birth Year: ");
                validDate = int.TryParse(Console.ReadLine(), out year);

                if (!validDate)
                {
                    continue;
                }

                Console.Write("Enter Birth Month: ");
                validDate = int.TryParse(Console.ReadLine(), out month);

                if (!validDate || month < 1 || month > 12)
                {
                    validDate = false;
                    continue;
                }

                Console.Write("Enter Birth Day: ");
                validDate = int.TryParse(Console.ReadLine(), out day);

                if (!validDate || day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    validDate = false;
                    continue;
                }

                birthdate = new DateTime(year, month, day);

            } while (!validDate);


            // Ask the user for their email
            Console.WriteLine("Enter your Email: ");
            string email = Console.ReadLine();

            int age = DateTime.Now.Year - birthdate.Year;
            if (DateTime.Now < birthdate.AddYears(age)) age--;

            if (age < 16)
            {
                Console.WriteLine("Sorry, you're not old enough to use the platform.");
                return;
            }

            // Generate a unique 5-digit ticket number
            string ticketNumber;
            do
            {
                ticketNumber = GenerateUniqueTicketNumber();
            } while (usedTicketNumbers.Contains(ticketNumber));

            // Add the generated ticket number to the list of used numbers
            usedTicketNumbers.Add(ticketNumber);

            // Generate the current timestamp
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Generate the report
            string report = $"Report Timestamp: {timestamp}\n" +
                            $"Thank you for your Report we will contact you as soon as possible\n" +
                            $"First Name: {firstName}\n" +
                            $"Last Name: {lastName}\n" +
                            $"Birth Date: {birthdate.ToShortDateString()}\n" +
                            $"Email: {email}\n" +
                            $"Age: {age}\n" +
                            $"Ticket Number: {ticketNumber}";

            // Specify the directory where reports will be saved
            string directoryPath = "/Users/jaylanthompson/Desktop/Report-Generator/Reports";

            // Create the directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Determine the filename using the first letter of the first name and the last name
            string filename = $"{firstName[0]}.{lastName}";

            // Combine the directory path and filename to create the full file path
            string filePath = Path.Combine(directoryPath, filename + "_" + timestamp + ".txt");

            // Save the report to a text file
            File.WriteAllText(filePath, report);

            Console.WriteLine(report);
            Console.WriteLine($"Report saved as '{filePath}'");
        }

        // Function to generate a unique 5-digit ticket number
        static string GenerateUniqueTicketNumber()
        {
            Random random = new Random();
            string ticketNumber;

            do
            {
                int number = random.Next(10000, 100000); // Generate a random 5-digit number
                ticketNumber = $"Ticket {number}";
            } while (usedTicketNumbers.Contains(ticketNumber));

            return ticketNumber;
        }
    }
}
