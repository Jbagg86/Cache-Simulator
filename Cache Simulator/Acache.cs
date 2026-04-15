using Cache_Simulator;
using System;
using System.Collections.Generic;

namespace Cache_Simulator
{
    public class Acache : Cache
    {
        private int ap;
        private string[] cacheLines;
        private HashSet<string> seenAddresses;
        private Random rand;

        public Acache(int ap)
        {
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
                bool found = false;

                // Search cache
                for (int j = 0; j < cacheLines.Length; j++)
                {
                    if (cacheLines[j] == address)
                    {
                        found = true;
                        SetHits(GetHits() + 1);
                        break;
                    }
                }

                if (!found)
                {
                    // Determine cold or conflict miss
                    if (!seenAddresses.Contains(address))
                    {
                        SetColdMiss(GetColdMiss() + 1);
                        seenAddresses.Add(address);
                    }
                    else
                    {
                        SetConflictMiss(GetConflictMiss() + 1);
                    }

                    int addressValue = Convert.ToInt32(address, 2);

                    // DIRECT MAPPED CACHE
                    if (ap == 1)
                    {
                        int lineIndex = addressValue % 32;
                        cacheLines[lineIndex] = address;
                    }

                    // FULLY ASSOCIATIVE CACHE
                    else if (ap == 32)
                    {
                        bool inserted = false;

                        for (int j = 0; j < cacheLines.Length; j++)
                        {
                            if (string.IsNullOrEmpty(cacheLines[j]))
                            {
                                cacheLines[j] = address;
                                inserted = true;
                                break;
                            }
                        }

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