﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HAL_Simulator;
using NUnit.Framework;

namespace WPILib.Tests
{
    [TestFixture(0)]
    [TestFixture(18)]
    [TestFixture(53)]
    public class TestPowerDistributionPanel : TestBase
    {
        private int m_module;

        public TestPowerDistributionPanel(int module)
        {
            m_module = module;
        }

        public Dictionary<dynamic, dynamic> PDPData(int module)
        {
            return SimData.HalData["pdp"][module];
        }

        public PowerDistributionPanel GetPDP(int module)
        {
            return new PowerDistributionPanel(m_module);
        }

        [Test]
        public void TestPDP0InitializedByDefault()
        {
            SimData.ResetHALData(false);
            Assert.AreEqual(1, SimData.HalData["pdp"].Count);
        }

        [TestCase(1)]
        [TestCase(53)]
        public void TestCreatePDPModule(int module)
        {
            SimData.ResetHALData(false);

        }

        [Test]
        public void TestCreatePDPLowerLimit()
        {
            SimData.ResetHALData(false);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                PowerDistributionPanel pdp = new PowerDistributionPanel(-1);
                pdp.GetCurrent(0);
            });
        }

        [Test]
        public void TestCreatePDPUpperLimit()
        {
            SimData.ResetHALData(false);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                PowerDistributionPanel pdp = new PowerDistributionPanel(PDPModules);
                pdp.GetCurrent(0);
            });
        }

        [Test]
        public void TestPDPGetVoltage()
        {
            var pdp = GetPDP(m_module);
            PDPData(m_module)["voltage"] = 3.14;
            Assert.AreEqual(3.14, pdp.GetVoltage());
        }

        [Test]
        public void TestPDPGetTemperature()
        {
            var pdp = GetPDP(m_module);
            PDPData(m_module)["temperature"] = 90;
            Assert.AreEqual(90, pdp.GetTemperature());
        }

        [Test]
        public void TestPDPGetCurrent([Range(0, 15)]int channel)
        {
            var pdp = GetPDP(m_module);
            PDPData(m_module)["current"][channel] = channel*3;
            Assert.AreEqual(channel *3, pdp.GetCurrent(channel));
        }

        [TestCase(-1)]
        [TestCase(16)]
        public void TestPDPGetCurrentLimits(int channel)
        {
            var pdp = GetPDP(m_module);
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                pdp.GetCurrent(channel);
            });
        }

        [Test]
        public void TestPDPGetTotalCurrent()
        {
            var pdp = GetPDP(m_module);
            PDPData(m_module)["total_current"] = 42;
            Assert.AreEqual(42, pdp.GetTotalCurrent());
        }

        [Test]
        public void TestPDPGetTotalPower()
        {
            var pdp = GetPDP(m_module);
            PDPData(m_module)["total_power"] = 42;
            Assert.AreEqual(42, pdp.GetTotalPower());
        }

        [Test]
        public void TestPDPGetTotalEnergy()
        {
            var pdp = GetPDP(m_module);
            PDPData(m_module)["total_energy"] = 42;
            Assert.AreEqual(42, pdp.GetTotalEnergy());
        }

        [Test]
        public void TestPDPResetTotalEnergy()
        {
            var pdp = GetPDP(m_module);
            PDPData(m_module)["total_energy"] = 42;
            Assert.AreEqual(42, pdp.GetTotalEnergy());
            pdp.ResetTotalEnergy();
            Assert.AreEqual(0, pdp.GetTotalEnergy());
        }

        [Test]
        public void TestPDPClearStickyFaults()
        {
            var pdp = GetPDP(m_module);
            Assert.DoesNotThrow(() =>
            {
                pdp.ClearStickyFaults();
            });
        }
    }
}
