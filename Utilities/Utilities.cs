using System;

namespace HospitalSystem.Utilities
{
    public class Utilities
    {
        public static void FormatTable(string[] headers, List<string[]> rows)
        {
            // Calculate max width for each column
            int[] maxWidths = new int[headers.Length];
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i].Length > maxWidths[i])
                    {
                        maxWidths[i] = row[i].Length;
                    }
                }
            }

            // Print the table headers
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write(string.Format("{0,-" + (maxWidths[i] + 4) + "}", headers[i]));
            }

            //Console.WriteLine();

            // Print the table horizontal bar
            Console.WriteLine(new string('-', maxWidths.Sum() + 4 * headers.Length));

            // Print the table rows
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    Console.Write(string.Format("{0,-" + (maxWidths[i] + 4) + "}", row[i]));
                }
                Console.WriteLine();
            }
        }
    }
}
