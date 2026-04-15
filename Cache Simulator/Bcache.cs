using Cache_Simulator;
using System;
using System.Collections.Generic;

namespace Cache_Simulator
{
    public class Bcache : Cache
    {
        private int bp;
        private string[,] cacheBlocks;
        private HashSet<string> seenAddresses;
        private Random rand;

        public Bcache(int bp)
        {
            this.bp = bp;
            seenAddresses = new HashSet<string>();
            rand = new Random();

            if (bp == 1)
            {
                cacheBlocks = new string[32, 1];
            }
            else
            {
                cacheBlocks = new string[8, 4];
            }

            for (int i = 0; i < cacheBlocks.GetLength(0); i++)
            {
                for (int j = 0; j < cacheBlocks.GetLength(1); j++)
                {
                    cacheBlocks[i, j] = "";
                }
            }
        }

        public override int MissCollector(string[,] addressArray)
        {
            for (int i = 0; i < addressArray.GetLength(0); i++)
            {
                string address = addressArray[i, 0];

                if (IsInCache(address))
                {
                    SetHits(GetHits() + 1);
                }
                else
                {
                    if (!seenAddresses.Contains(address))
                    {
                        SetColdMiss(GetColdMiss() + 1);
                        seenAddresses.Add(address);
                    }
                    else
                    {
                        SetConflictMiss(GetConflictMiss() + 1);
                    }

                    if (bp == 1)
                    {
                        InsertSingleBlock(address);
                    }
                    else if (bp == 4)
                    {
                        InsertPrefetchedBlock(address);
                    }
                }
            }

            return GetHits() + GetColdMiss() + GetConflictMiss();
        }

        private bool IsInCache(string address)
        {
            for (int r = 0; r < cacheBlocks.GetLength(0); r++)
            {
                for (int c = 0; c < cacheBlocks.GetLength(1); c++)
                {
                    if (cacheBlocks[r, c] == address)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void InsertSingleBlock(string address)
        {
            int addressValue = Convert.ToInt32(address, 2);
            int row = addressValue % 32;
            cacheBlocks[row, 0] = address;
        }

        private void InsertPrefetchedBlock(string address)
        {
            int addressValue = Convert.ToInt32(address, 2);

            // Align starting address to a 4-address block
            int blockStart = (addressValue / 4) * 4;
            int row = (blockStart / 4) % 8;

            for (int c = 0; c < 4; c++)
            {
                string blockAddress = Convert.ToString(blockStart + c, 2).PadLeft(address.Length, '0');
                cacheBlocks[row, c] = blockAddress;

                if (!seenAddresses.Contains(blockAddress))
                {
                    seenAddresses.Add(blockAddress);
                }
            }
        }
    }
}