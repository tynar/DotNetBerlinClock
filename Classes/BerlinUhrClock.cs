using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BerlinClock.Classes
{
    public struct BerlinUhrClock
    {
        public const int TicksPerSecond = 1;
        public const int TicksPerMinute = TicksPerSecond * 60;
        public const int TicksPerHour = TicksPerMinute * 60;

        public const int MaxSeconds = 25 * TicksPerHour - TicksPerSecond; // max hour is 24:59:59 or 25:00:00 - 1 second

        private int _ticks;

        public BerlinUhrClock(int hours, int minutes, int seconds)
        {
            _ticks = TimeToTicks(hours, minutes, seconds);
        }

        public static BerlinUhrClock Parse(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                throw new ArgumentNullException(nameof(time));
            }

            Regex regex = new Regex(@"^(?:([01]?\d|2[0-4]):([0-5]?\d):)?([0-5]?\d)$");
            //[01]?\d|2[0-4] - can start with 0 or 1 and second character can be 0-4
            //[0-5]?\d - can start from 0 to 5 and second character is digit
            //[0-5]?\d - can start from 0 to 5 and second character is digit

            var match = regex.Match(time);
            if (!match.Success)
            {
                throw new ArgumentException("Illegal time, please input in HH:mm:ss format.");
            }

            int hours = Convert.ToInt32(match.Groups[1].Value); //hour
            int minutes = Convert.ToInt32(match.Groups[2].Value); //minute
            int seconds = Convert.ToInt32(match.Groups[3].Value); //second

            return new BerlinUhrClock(hours, minutes, seconds);
        }

        private static int TimeToTicks(int hours, int minutes, int seconds)
        {
            int totalSeconds = hours * 60 * 60 + minutes * 60 + seconds;
            if (totalSeconds > MaxSeconds || totalSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(null, "Give time is too long.");
            }
            return TicksPerSecond * totalSeconds;
        }

        public int Hours
        {
            get
            {
                int hours = _ticks / TicksPerHour;
                if (hours % 24 == 0 && hours != 0) return 24; //return 24 for 24. In "Berlin Clock" it can go up to 24:59:59
                return hours % 24;
            }
        }

        public int Minutes
        {
            get { return _ticks / TicksPerMinute % 60; }
        }

        public int Seconds
        {
            get { return _ticks / TicksPerSecond % 60; }
        }

        /// <summary>
        /// Overrides ToString method. Representation of clock in Berlin Uhr format. For more description go to
        /// site https://github.com/mazhewitt/DotNetBerlinClock/blob/master/Readme.md
        /// </summary>
        /// <returns>String representation of clock in Berlin Uhr format.</returns>
        public override string ToString()
        {
            /*
                https://github.com/mazhewitt/DotNetBerlinClock/blob/master/Readme.md
                The Berlin Uhr(Clock) is a rather strange way to show the time. 
                On the top of the clock there is a yellow lamp that blinks on/ off every two seconds.
                The time is calculated by adding rectangular lamps.

                The top two rows of lamps are red. These indicate the hours of a day.In the top row there are 4 red lamps. 
                Every lamp represents 5 hours.In the lower row of red lamps every lamp represents 1 hours.
                So if two lamps of the first row and three of the seconds row are switched on that indicates 5 + 5 + 3 = 13h or 1 pm.

                The two rows of lamps at the bottom count the minutes.The first of these rows has 11 lamps, the seconds 4.
                In the first row every lamp represents 5 minutes.In this first row the 3rd, 6th and 9th lamp are red and indicate the first quarter, 
                half and last quarter of an hours.The other lamps are yellow.In the last row with 4 lamps every lamp represents 1 minutes.
            */

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetSecondsRow());
            sb.AppendLine(ComposeLampsRow(Hours / 5, 4, 'R'));
            sb.AppendLine(ComposeLampsRow(Hours % 5, 4, 'R'));
            string minuteLamps = ComposeLampsRow(Minutes / 5, 11, 'Y');
            //Replacing each third yellow lamp with red lamp
            sb.AppendLine(minuteLamps.Replace("YYY", "YYR"));
            sb.Append(ComposeLampsRow(Minutes % 5, 4, 'Y'));
            return sb.ToString();
        }

        /// <summary>
        /// String representation of seconds lamp.
        /// </summary>
        /// <returns>String containing 'Y' when seconds are even 'O' for odd.</returns>
        private string GetSecondsRow()
        {
            return Seconds % 2 == 0 ? "Y" : "O";
        }

        /// <summary>
        /// String representation of lamps in a row. Fills row with the number of turned on lamps and then fills with empty lamps remaining ones in a row.
        /// </summary>
        /// <param name="turnedOnLamps">Count of lamps to be turned on.</param>
        /// <param name="totalLampsCount">Total number of lamps in a row.</param>
        /// <param name="lampColor">Character representing lamp's color. E.g. 'Y' - yellow, 'R' - red, 'O' - off.</param>
        /// <returns></returns>
        private string ComposeLampsRow(int turnedOnLamps, int totalLampsCount, char lampColor)
        {
            return new string(lampColor, turnedOnLamps).PadRight(totalLampsCount, 'O');
        }

    }
}
