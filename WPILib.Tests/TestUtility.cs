﻿using System.Collections.Generic;
using HAL_Base;
using NUnit.Framework;

namespace WPILib.Tests
{
    [TestFixture]
    public class TestUtility : TestBase
    {
     

        private Dictionary<dynamic, dynamic> HalData()
        {
            return HAL.halData;
        }

        [Test]
        public void TestGetFPGAVersion()
        {
            Assert.AreEqual(2015, Utility.GetFPGAVersion());
        }

        [Test]
        public void TestFPGAGetRevision()
        {
            Assert.AreEqual(0, Utility.GetFPGARevision());
        }


        [Test]
        public void TestGetUserButton()
        {
            HalData()["fpga_button"] = true;
            Assert.IsTrue(Utility.GetUserButton());
        }
    }
}
