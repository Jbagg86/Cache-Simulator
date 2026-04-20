// See https://aka.ms/new-console-template for more information
using Cache_Simulator;
using System;
using System.IO;

namespace Cache_Simulator
{
    public class Manager
    {
        public static void Main(string[] args)
        {
            //Main method to run the cache simulator and tests"*/
            /* string fileName = "input.txt";

             if (!File.Exists(fileName))
             {
                 Console.WriteLine("File not found.");
                 return;
             }

             string[,] addressArray = ReadTraceFile(fileName);

             Console.WriteLine("Cache Simulator");
             Console.WriteLine("----------------");

             Console.Write("Enter A for Acache (Associativity) or B for Bcache (Block Size): ");
             char choice = char.ToUpper(Console.ReadLine()[0]);

             Cache cacheObject;

             if (choice == 'A')
             {
                 Console.Write("Enter Ap (1 = Direct Mapped, 32 = Fully Associative): ");
                 int ap = int.Parse(Console.ReadLine());

                 Console.WriteLine();
                 Console.WriteLine("Running Associativity Cache Simulation...");
                 Console.WriteLine("------------------------------------------");

                 cacheObject = new Acache(ap);
                 cacheObject.MissCollector(addressArray);

                 PrintResults(cacheObject, "Acache", ap);
             }
             else
             {
                 Console.Write("Enter Bp, 1 = 1 block, 4 = 4 block): ");
                 int bp = int.Parse(Console.ReadLine());

                 Console.WriteLine();
                 Console.WriteLine("Running Block Size Cache Simulation...");
                 Console.WriteLine("--------------------------------------");

                 cacheObject = new Bcache(bp);
                 cacheObject.MissCollector(addressArray);

                 PrintResults(cacheObject, "Bcache", bp);
             }
  

        public static string[,] ReadTraceFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            string[,] addressArray = new string[lines.Length, 2];

            for (int i = 0; i < lines.Length; i++)
            {
                addressArray[i, 0] = lines[i].Trim();
                addressArray[i, 1] = "0";   // flag bit
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
        }*/

            /* Unit Test for Acache*/

            string[,] testArray =
            {
                { "0000001", "0" },   // 1  -> cold miss
                { "0000010", "0" },   // 2  -> cold miss
                { "0000001", "0" },   // 1  -> hit
                { "0100001", "0" },   // 33 -> cold miss, replaces line 1
                { "0000001", "0" }    // 1  -> conflict miss, line 1 was replaced by line 33
            };
            
            Acache cache = new Acache(1);
            cache.MissCollector(testArray);

            Console.WriteLine("UNIT TEST - Acache");
            Console.WriteLine("------------------");
            Console.WriteLine("Expected Hits: 1");
            Console.WriteLine("Actual Hits: " + cache.GetHits());
            Console.WriteLine("Expected Cold Misses: 3");
            Console.WriteLine("Actual Cold Misses: " + cache.GetColdMiss());
            Console.WriteLine("Expected Conflict Misses: 1");
            Console.WriteLine("Actual Conflict Misses: " + cache.GetConflictMiss());
            


            /* Integration Test for Manager + Acache */
            /* string fileName = "input2.txt";

             if (!File.Exists(fileName))
             {
                 Console.WriteLine("Integration test file not found.");
                 return;
             }

             string[,] addressArray = ReadTraceFile(fileName);

             Acache cache = new Acache(1);
             cache.MissCollector(addressArray);

             Console.WriteLine("INTEGRATION TEST - Manager + Acache");
             Console.WriteLine("-----------------------------------");
             Console.WriteLine("Expected Hits: 1");
             Console.WriteLine("Actual Hits: " + cache.GetHits());
             Console.WriteLine("Expected Cold Misses: 3");
             Console.WriteLine("Actual Cold Misses: " + cache.GetColdMiss());
             Console.WriteLine("Expected Conflict Misses: 1");
             Console.WriteLine("Actual Conflict Misses: " + cache.GetConflictMiss());

             bool passed =
                 cache.GetHits() == 1 &&
                 cache.GetColdMiss() == 3 &&
                 cache.GetConflictMiss() == 1;

             Console.WriteLine("Status: " + (passed ? "PASS" : "FAIL"));
         }

         public static string[,] ReadTraceFile(string fileName)
         {
             string[] lines = File.ReadAllLines(fileName);
             string[,] addressArray = new string[lines.Length, 2];

             for (int i = 0; i < lines.Length; i++)
             {
                 addressArray[i, 0] = lines[i].Trim();
                 addressArray[i, 1] = "0";
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
            */

        }
    }
}