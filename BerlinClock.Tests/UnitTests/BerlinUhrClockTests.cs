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
                BerlinUhrClock clock = new BerlinUhrClock("00:00:00");
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_Exception_On_More_Than_2_Delimiters()
        {
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("1:2:3:4");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(ArgumentException));
                Assert.AreEqual(ex.Message, "Given time cannot parsed, please use format HH:mm:ss, e.g. 14:04:20.");
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_Exception_On_Less_Than_2_Delimiters()
        {
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("1:2");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
                Assert.AreEqual(ex.Message, "Given time cannot parsed, please use format HH:mm:ss, e.g. 14:04:20.");
                throw;
            }
        }

        [TestMethod]
        public void On_2_Delimiters_Success()
        {
            BerlinUhrClock clock = new BerlinUhrClock("1:2:3");
            clock = new BerlinUhrClock("01:02:03");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_Exception_On_Not_Integer_Hour()
        {
            BerlinUhrClock clock = new BerlinUhrClock("hour:2:3");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Throws_Exception_On_Out_Of_Bounds_Hour()
        {
            //Max hour is 24, upper hours are 4 * 5 = 20, lower hours are 4, total = 24
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("25:2:3");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentOutOfRangeException);
                Assert.IsTrue(e.Message.Contains("hours"));
                throw;
            }

            //Min hour is 0
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("-1:2:3");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentOutOfRangeException);
                Assert.IsTrue(e.Message.Contains("hours"));
                throw;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_Exception_On_Not_Integer_Minute()
        {
            BerlinUhrClock clock = new BerlinUhrClock("10:minute:3");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Throws_Exception_On_Out_Of_Bounds_Minute()
        {
            //Max minute is 59, upper minutes are 11 * 5 = 55, lower minutes are 4, total = 59
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("10:60:3");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentOutOfRangeException);
                Assert.IsTrue(e.Message.Contains("minutes"));
                throw;
            }

            //Min minute is 0
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("10:-2:3");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentOutOfRangeException);
                Assert.IsTrue(e.Message.Contains("minutes"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_Exception_On_Not_Integer_Second()
        {
            BerlinUhrClock clock = new BerlinUhrClock("10:0:sec");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Throws_Exception_On_Out_Of_Bounds_Second()
        {
            //Max seconds are 59
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("10:06:79");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentOutOfRangeException);
                Assert.IsTrue(e.Message.Contains("seconds"));
                throw;
            }

            //Min second is 0
            try
            {
                BerlinUhrClock clock = new BerlinUhrClock("10:06:-7");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentOutOfRangeException);
                Assert.IsTrue(e.Message.Contains("seconds"));
                throw;
            }
        }

        [TestMethod]
        public void Correctly_Parses_Time()
        {
            BerlinUhrClock clock = new BerlinUhrClock("0:01:02");

            Assert.AreEqual(0, clock.Hours);
            Assert.AreEqual(1, clock.Minutes);
            Assert.AreEqual(2, clock.Seconds);

            clock = new BerlinUhrClock("14:20:20");

            Assert.AreEqual(14, clock.Hours);
            Assert.AreEqual(20, clock.Minutes);
            Assert.AreEqual(20, clock.Seconds);

            clock = new BerlinUhrClock("24:59:59");

            Assert.AreEqual(24, clock.Hours);
            Assert.AreEqual(59, clock.Minutes);
            Assert.AreEqual(59, clock.Seconds);
        }

        [TestMethod]
        public void Correctly_Converts_ToString()
        {
            BerlinUhrClock clock = new BerlinUhrClock("00:00:00");
            Assert.AreEqual("Y\r\nOOOO\r\nOOOO\r\nOOOOOOOOOOO\r\nOOOO", clock.ToString());

            clock = new BerlinUhrClock("00:00:01");
            Assert.AreEqual("O\r\nOOOO\r\nOOOO\r\nOOOOOOOOOOO\r\nOOOO", clock.ToString());

            clock = new BerlinUhrClock("24:00:00");
            Assert.AreEqual("Y\r\nRRRR\r\nRRRR\r\nOOOOOOOOOOO\r\nOOOO", clock.ToString());

            clock = new BerlinUhrClock("23:59:59");
            Assert.AreEqual("O\r\nRRRR\r\nRRRO\r\nYYRYYRYYRYY\r\nYYYY", clock.ToString());

            clock = new BerlinUhrClock("13:17:01");
            Assert.AreEqual("O\r\nRROO\r\nRRRO\r\nYYROOOOOOOO\r\nYYOO", clock.ToString());
        }
    }
}
