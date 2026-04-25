using System;

namespace Cache_Simulator
{
    // Cache is the parent class for both Acache and Bcache.
    // It stores the counters used by both cache types:
    // hits, cold misses, and conflict misses.
    public abstract class Cache
    {
        protected int hits;
        protected int coldMiss;
        protected int conflictMiss;

        // Getters and setters for the counters, used by both Acache and Bcache.
        public int GetHits()
        {
            return hits;
        }

        public int GetColdMiss()
        {
            return coldMiss;
        }

        public int GetConflictMiss()
        {
            return conflictMiss;
        }

        public void SetHits(int x)
        {
            hits = x;
        }

        public void SetColdMiss(int y)
        {
            coldMiss = y;
        }

        public void SetConflictMiss(int z)
        {
            conflictMiss = z;
        }

        
        // MissCollector is overridden in each child class because
        // Acache and Bcache handle addresses differently.
        public abstract int MissCollector(string[,] addressArray);
    }
}