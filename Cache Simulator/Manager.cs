using System;
using System.IO;

namespace Cache_Simulator
{
    // Manager is the normal simulator driver.
    // This version reads addresses from an input file.
    public class Manager
    {
        public static void Main(string[] args)
        {
            string fileName = "traces.txt";

            if (!File.Exists(fileName))
            {
                Console.WriteLine("Error: Input file not found.");
                return;
            }

            string[,] addressArray = ReadTraceFile(fileName);

            Console.WriteLine("Cache Simulator");
            Console.WriteLine("----------------");

            Console.Write("Enter A for Acache (Associativity) or B for Bcache (Block Size): ");
            string? choiceInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(choiceInput))
            {
                Console.WriteLine("Error: You must enter A or B.");
                return;
            }

            char choice = char.ToUpper(choiceInput[0]);

            Cache cacheObject;

            if (choice == 'A')
            {
                Console.Write("Enter Ap (1 = Direct Mapped, 32 = Fully Associative): ");
                string? apInput = Console.ReadLine();

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