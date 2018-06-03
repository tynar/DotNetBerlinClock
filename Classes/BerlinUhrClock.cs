using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerlinClock.Classes
{
    public class BerlinUhrClock
    {
        private int hours, minutes, seconds;

        public int Hours
        {
            get { return hours; }
        }

        public int Minutes
        {
            get { return minutes; }
        }

        public int Seconds
        {
            get { return seconds; }
        }

        public BerlinUhrClock(string aTime)
        {
            var splitted = aTime.Split(':');
            ValidateArgument(splitted);

            ParseHours(splitted[0]);
            ParseMinutes(splitted[1]);
            ParseSeconds(splitted[2]);
        }

        private static void ValidateArgument(string[] times)
        {
            if (times == null || times.Length == 0) throw new ArgumentNullException(nameof(times));
            if (times.Length != 3)
            {
                throw new ArgumentException("Given time cannot parsed, please use format HH:mm:ss, e.g. 14:04:20.");
            }
        }

        private void ParseHours(string hours)
        {
            int hoursTemp;
            if (!int.TryParse(hours, out hoursTemp))
            {
                throw new ArgumentException("Hours cannot be parsed.");
            }

            if (hoursTemp < 0 || hoursTemp > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(hours));
            }
            this.hours = hoursTemp;
        }

        private void ParseMinutes(string minutes)
        {
            int minutesTemp;
            if (!int.TryParse(minutes, out minutesTemp))
            {
                throw new ArgumentException("Minutes cannot be parsed.");
            }

            if (minutesTemp < 0 || minutesTemp > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes));
            }
            this.minutes = minutesTemp;
        }

        private void ParseSeconds(string seconds)
        {
            int secondsTemp;
            if (!int.TryParse(seconds, out secondsTemp))
            {
                throw new ArgumentException("Seconds cannot be parsed.");
            }

            if (secondsTemp < 0 || secondsTemp > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds));
            }
            this.seconds = secondsTemp;
        }

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
            sb.AppendLine(ComposeLampsRow(hours / 5, 4, 'R'));
            sb.AppendLine(ComposeLampsRow(hours % 5, 4, 'R'));
            string minuteLamps = ComposeLampsRow(minutes / 5, 11, 'Y');
            //Replacing each third yellow lamp with red lamp
            sb.AppendLine(minuteLamps.Replace("YYY", "YYR"));
            sb.Append(ComposeLampsRow(minutes % 5, 4, 'Y'));
            return sb.ToString();
        }

        private string GetSecondsRow()
        {
            return seconds % 2 == 0 ? "Y" : "O";
        }

        private string ComposeLampsRow(int turnedOnLamps, int totalLampsCount, char lampColor)
        {
            return new string(lampColor, turnedOnLamps).PadRight(totalLampsCount, 'O');
        }

    }
}
