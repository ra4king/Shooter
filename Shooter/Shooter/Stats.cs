using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    class Stats
    {
        public static long startTime;
        public static long endTime;
        public static int deaths;
        public static int shotsFired;

        public static int timePoints()
        {
            long timeScore = (600000 - (endTime - startTime))/10;
            if (timeScore < 0)
                timeScore = 0;
            return (int)timeScore;
        }

        public static int deathPoints()
        {
            int deathScore = 10000 - deaths * 100;
            if (deathScore < 0)
                deathScore = 0;
            return deathScore;
        }

        public static int shotsFiredPoints()
        {
            return shotsFired / 10;
        }

        public static int fedoraPoints(int fedoras)
        {
            return fedoras * 5000;
        }
    }
}
