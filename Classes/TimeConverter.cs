using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BerlinClock.Classes;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        public string convertTime(string aTime)
        {
            BerlinUhrClock clock = BerlinUhrClock.Parse(aTime);
            return clock.ToString();
        }
    }
}
