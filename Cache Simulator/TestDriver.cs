using System;

namespace Cache_Simulator
{

    // TestDriver is a separate driver used only for the Acache unit test.
    // It does NOT read from a file.
    // It uses hard-coded test data so Acache can be tested in isolation
    public class TestDriver
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Acache Unit Test");
            Console.WriteLine("----------------");


            // Validate input
            int ap;
            while (true)
            {
                Console.Write("Enter Ap (1 = Direct Mapped, 32 = Fully Associative): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out ap) && (ap == 1 || ap == 32))
                {
                    break; // valid input exits loop
                }

                Console.WriteLine("Error: Ap must be 1 or 32.");
            }

            // Hard-coded test array 
            string[,] testArray =
            {
                { "00000001", "0" },
                { "00000010", "0" },
                { "00000001", "0" },
                { "00100001", "0" },
                { "00000001", "0" }
            };

            // Create Acache directly 
            Acache cache = new Acache(ap);
            cache.MissCollector(testArray);

            Console.WriteLine();
            Console.WriteLine("Results");
            Console.WriteLine("-------");
            Console.WriteLine("Hits: " + cache.GetHits());
            Console.WriteLine("Cold Misses: " + cache.GetColdMiss());
            Console.WriteLine("Conflict Misses: " + cache.GetConflictMiss());

            Console.WriteLine();

            // Expected values for direct mapped test
            if (ap == 1)
            {
                Console.WriteLine("Expected Hits: 1");
                Console.WriteLine("Expected Cold Misses: 3");
                Console.WriteLine("Expected Conflict Misses: 1");
            }
            else
            {
                Console.WriteLine("Expected Hits: 2");
                Console.WriteLine("Expected Cold Misses: 3");
                Console.WriteLine("Expected Conflict Misses: 0");
            }
        }
    }
}

