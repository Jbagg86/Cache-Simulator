using System;
using System.IO;

namespace Cache_Simulator
{
    // Manager is the normal simulator driver.
    // This version reads addresses from an input file.
    public class Manager
    {
        // Main method to run the cache simulator
        public static void Main(string[] args)
        {
            string fileName = "traces.txt";

            if (!File.Exists(fileName))
            {
                Console.WriteLine("Error: Input file not found.");
                return;
            }
            // Read the trace file and store addresses in a 2D array
            string[,] addressArray = ReadTraceFile(fileName);

            Console.WriteLine("Cache Simulator");
            Console.WriteLine("----------------");

            Console.Write("Enter A for Acache (Associativity) or B for Bcache (Block Size): ");
            string? choiceInput = Console.ReadLine();

            // Validate user input for cache type selection
            if (string.IsNullOrWhiteSpace(choiceInput))
            {
                Console.WriteLine("Error: You must enter A or B.");
                return;
            }
            
            char choice = char.ToUpper(choiceInput[0]);

            // Declare a Cache variable to hold the selected cache simulation object
            Cache cacheObject;

            // Validate user input and run the appropriate cache simulation
            if (choice == 'A')
            {
                Console.Write("Enter Ap (1 = Direct Mapped, 32 = Fully Associative): ");
                string? apInput = Console.ReadLine();

                // Validate user input for associativity selection
                if (!int.TryParse(apInput, out int ap) || (ap != 1 && ap != 32))
                {
                    Console.WriteLine("Error: Ap must be 1 or 32.");
                    return;
                }
                
                Console.WriteLine();
                Console.WriteLine("Running Associativity Cache Simulation...");
                Console.WriteLine("-----------------------------------------");

                cacheObject = new Acache(ap);
                cacheObject.MissCollector(addressArray);
                PrintResults(cacheObject, "Acache", ap);
            }
            
            else if (choice == 'B')
            {
                Console.Write("Enter Bp (1, 2, or 4): ");
                string? bpInput = Console.ReadLine();

                // Validate user input for block size selection
                if (!int.TryParse(bpInput, out int bp) || (bp != 1 && bp != 2 && bp != 4))
                {
                    Console.WriteLine("Error: Bp must be 1, 2, or 4.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Running Block Size Cache Simulation...");
                Console.WriteLine("--------------------------------------");

                cacheObject = new Bcache(bp);
                cacheObject.MissCollector(addressArray);
                PrintResults(cacheObject, "Bcache", bp);
            }
            else
            {
                Console.WriteLine("Error: Invalid choice. Enter A or B.");
                return;
            }
        }
        //Reads the trace file and returns a 2D array of addresses and their initial status.
        public static string[,] ReadTraceFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            // Count only non-empty lines
            int validCount = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    validCount++;
                }
            }
            // Create a 2D array to hold valid addresses and their initial status (0 for not accessed)
            string[,] addressArray = new string[validCount, 2];

            int row = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    addressArray[row, 0] = lines[i].Trim();
                    addressArray[row, 1] = "0";
                    row++;
                }
            }

            return addressArray;
        }
        // Prints the results of the cache simulation, including hits, cold misses, and conflict misses.
        public static void PrintResults(Cache cacheObject, string mode, int parameter)
        {
            Console.WriteLine();
            Console.WriteLine("Simulation Results");
            Console.WriteLine("------------------");
            Console.WriteLine("Mode: " + mode);
            Console.WriteLine("Parameter: " + parameter);
            Console.WriteLine();

            Console.WriteLine("Hits: " + cacheObject.GetHits());
            Console.WriteLine("Cold Misses: " + cacheObject.GetColdMiss());
            Console.WriteLine("Conflict Misses: " + cacheObject.GetConflictMiss());
        }
    }
}