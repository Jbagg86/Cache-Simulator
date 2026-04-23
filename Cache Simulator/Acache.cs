using System;
using System.Collections.Generic;

namespace Cache_Simulator
{
    // Acache simulates associativity.
    // Valid ap values:
    // 1  = Direct Mapped
    // 32 = Fully Associative
    public class Acache : Cache
    {
        private int ap;
        private string[] cacheLines;
        private HashSet<string> seenAddresses;
        private Random rand;

        public Acache(int ap)
        {
            // Only allow the valid associativity 1 or 32
            if (ap != 1 && ap != 32)
            {
                throw new ArgumentException("Ap must be 1 or 32.");
            }

            this.ap = ap;
            cacheLines = new string[32];
            seenAddresses = new HashSet<string>();
            rand = new Random();

            for (int i = 0; i < cacheLines.Length; i++)
            {
                cacheLines[i] = "";
            }
        }

        public override int MissCollector(string[,] addressArray)
        {
            for (int i = 0; i < addressArray.GetLength(0); i++)
            {
                string address = addressArray[i, 0];
                int addressValue = Convert.ToInt32(address, 2);
                bool found = false;

                // DIRECT MAPPED CACHE
                // Only check the one line the address maps to.
                if (ap == 1)
                {
                    int lineIndex = addressValue % 32;

                    if (cacheLines[lineIndex] == address)
                    {
                        found = true;
                        SetHits(GetHits() + 1);
                    }
                }

                // FULLY ASSOCIATIVE CACHE
                // Search every line because the address can be anywhere.
                else if (ap == 32)
                {
                    for (int j = 0; j < cacheLines.Length; j++)
                    {
                        if (cacheLines[j] == address)
                        {
                            found = true;
                            SetHits(GetHits() + 1);
                            break;
                        }
                    }
                }

                if (!found)
                {
                    // If the address has never been seen before, count a cold miss.
                    // If it has been seen before but is not currently in cache,
                    // count a conflict miss.
                    if (!seenAddresses.Contains(address))
                    {
                        SetColdMiss(GetColdMiss() + 1);
                        seenAddresses.Add(address);
                    }
                    else
                    {
                        SetConflictMiss(GetConflictMiss() + 1);
                    }

                    // DM Insertion
                    if (ap == 1)
                    {
                        int lineIndex = addressValue % 32;
                        cacheLines[lineIndex] = address;
                    }

                    // FA Insertion
                    else if (ap == 32)
                    {
                        bool inserted = false;

                        // First try to place the address in an empty line.
                        for (int j = 0; j < cacheLines.Length; j++)
                        {
                            if (string.IsNullOrEmpty(cacheLines[j]))
                            {
                                cacheLines[j] = address;
                                inserted = true;
                                break;
                            }
                        }

                        // If full, replace a random line.
                        if (!inserted)
                        {
                            int randomLine = rand.Next(32);
                            cacheLines[randomLine] = address;
                        }
                    }
                }
            }

            return GetHits() + GetColdMiss() + GetConflictMiss();
        }
    }
}