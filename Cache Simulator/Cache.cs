using System;

namespace Cache_Simulator
{
    public abstract class Cache
    {
        protected int hits;
        protected int coldMiss;
        protected int conflictMiss;

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

        public abstract int MissCollector(string[,] addressArray);
    }
}
