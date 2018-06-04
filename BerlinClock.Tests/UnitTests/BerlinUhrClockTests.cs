using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BerlinClock.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BerlinClock.Tests.UnitTests
{
    [TestClass]
    public class BerlinUhrClockTests
    {
        [TestMethod]
        public void Constructor_With_Time_Argument_Success()
        {
            try
            {
                BerlinUhrClock.Parse("00:00:00");
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }

        [TestMethod]
        public void Throws_Exception_On_Null_Or_Empty_Time_Parsing()
        {
            Assert.ThrowsException<ArgumentNullException>(() => BerlinUhrClock.Parse(null));
            Assert.ThrowsException<ArgumentNullException>(() => BerlinUhrClock.Parse(""));
        }

        [TestMethod]
        public void Throws_Exception_On_Valid_Time_Parsing()
        {
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("a:b:c"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("1:2"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("1:2:"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("1::"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("1:"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("::"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("::"));

            //out of bounds check
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("25:0:0"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("-25:0:0"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("-5:0:0"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("-5:00:00"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("5:-00:00"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("5:00:-00"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("-0:00:00"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("00:-00:00"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("00:00:-00"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("00:-01:00"));
            Assert.ThrowsException<ArgumentException>(() => BerlinUhrClock.Parse("00:01:-01"));
        }

        [TestMethod]
        public void Correctly_Parses_Time()
        {
            BerlinUhrClock clock = BerlinUhrClock.Parse("0:01:02");

            Assert.AreEqual(0, clock.Hours);
            Assert.AreEqual(1, clock.Minutes);
            Assert.AreEqual(2, clock.Seconds);

            clock = BerlinUhrClock.Parse("14:20:20");

            Assert.AreEqual(14, clock.Hours);
            Assert.AreEqual(20, clock.Minutes);
            Assert.AreEqual(20, clock.Seconds);

            clock = BerlinUhrClock.Parse("24:59:59");

            Assert.AreEqual(24, clock.Hours);
            Assert.AreEqual(59, clock.Minutes);
            Assert.AreEqual(59, clock.Seconds);
        }

        [TestMethod]
        public void Correctly_Converts_ToString()
        {
            BerlinUhrClock clock = BerlinUhrClock.Parse("00:00:00");
            Assert.AreEqual("Y\r\nOOOO\r\nOOOO\r\nOOOOOOOOOOO\r\nOOOO", clock.ToString());

            clock = BerlinUhrClock.Parse("00:00:01");
            Assert.AreEqual("O\r\nOOOO\r\nOOOO\r\nOOOOOOOOOOO\r\nOOOO", clock.ToString());

            clock = BerlinUhrClock.Parse("24:00:00");
            Assert.AreEqual("Y\r\nRRRR\r\nRRRR\r\nOOOOOOOOOOO\r\nOOOO", clock.ToString());

            clock = BerlinUhrClock.Parse("23:59:59");
            Assert.AreEqual("O\r\nRRRR\r\nRRRO\r\nYYRYYRYYRYY\r\nYYYY", clock.ToString());

            clock = BerlinUhrClock.Parse("13:17:01");
            Assert.AreEqual("O\r\nRROO\r\nRRRO\r\nYYROOOOOOOO\r\nYYOO", clock.ToString());
        }
    }
}
