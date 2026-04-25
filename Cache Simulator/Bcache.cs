using System;
using System.Collections.Generic;

namespace Cache_Simulator
{
    // Bcache simulates different block sizes with prefetching.
    // Valid bp values:
    // 1 = single address block
    // 2 = prefetch 2 addresses
    // 4 = prefetch 4 addresses
    public class Bcache : Cache
    {
        private int bp;
        private string[,] cacheBlocks;
        private HashSet<string> seenAddresses;

        public Bcache(int bp)
        {
            // Only allow the valid block sizes 1,2, or 4
            if (bp != 1 && bp != 2 && bp != 4)
            {
                throw new ArgumentException("Bp must be 1, 2, or 4.");
            }

            this.bp = bp;
            seenAddresses = new HashSet<string>();

            // Total cache size is 32 addresses.
            // So the number of rows depends on the block size.
            if (bp == 1)
            {
                cacheBlocks = new string[32, 1];
            }
            else if (bp == 2)
            {
                cacheBlocks = new string[16, 2];
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
        // MissCollector processes each address in the input array and updates the hit/miss counters.
        public override int MissCollector(string[,] addressArray)
        {
            for (int i = 0; i < addressArray.GetLength(0); i++)
            {
                string address = addressArray[i, 0];

                if (string.IsNullOrWhiteSpace(address))
                {
                    continue;
                }

                if (IsInCache(address))
                {
                    SetHits(GetHits() + 1);
                }
                else
                {
                    // First time seen = cold miss
                    // Seen before but not in cache = conflict miss
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
                    else
                    {
                        // For block sizes 2 and 4, prefetch the whole block.
                        InsertPrefetchedBlock(address);
                    }
                }
            }

            return GetHits() + GetColdMiss() + GetConflictMiss();
        }
        // Checks if the given address is currently stored in the cache.
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
        // For block size 1, simply insert the single address into the correct row.
        private void InsertSingleBlock(string address)
        {
            int addressValue = Convert.ToInt32(address, 2);
            int row = addressValue % 32;
            cacheBlocks[row, 0] = address;
        }
        // For block sizes 2 and 4, calculate the starting address of the block and insert all addresses in the block.
        private void InsertPrefetchedBlock(string address)
        {
            int addressValue = Convert.ToInt32(address, 2);

            if (string.IsNullOrWhiteSpace(address))
            {
                return;
            }

            // Find the starting address of the block.
            // Example:
            // if bp = 4 and address = 6, block start = 4
            // so the block loaded is 4, 5, 6, 7
            int blockStart = (addressValue / bp) * bp;

            // Choose the cache row based on the block number.
            int row = (blockStart / bp) % cacheBlocks.GetLength(0);

            for (int c = 0; c < bp; c++)
            {
                string blockAddress = Convert.ToString(blockStart + c, 2).PadLeft(address.Length, '0');
                cacheBlocks[row, c] = blockAddress;

                // Prefetched addresses count as seen addresses.
                // This helps later misses become conflict misses instead of cold misses.
                if (!seenAddresses.Contains(blockAddress))
                {
                    seenAddresses.Add(blockAddress);
                }
            }
        }
    }
}