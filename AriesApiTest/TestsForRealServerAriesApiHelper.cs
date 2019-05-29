using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aries.Core;
using Aries.Test;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;

namespace Aries.RealServer.Test
{
    [TestFixture]
    public class TestsForRealServerAriesApiHelper
    {
        private AriesApi ariesApi;
        private AriesApiHelper ariesApiHelper;

        private WebServer server;

//        private readonly string CLUSTER = "cluster.json";
        
        private readonly string HOST = "localhost";
        private readonly int PORT = 8080;
        private bool isServerExists;

        [SetUp]
        public void InitServer()
        {
            server = new WebServer(String.Format("http://{0}:{1}/", HOST, PORT), RoutingStrategy.Regex);
            isServerExists = TestHelper.RemoteFileExists(String.Format("http://{0}:{1}/", HOST, PORT));
            ariesApi = new AriesApi(HOST, PORT);
            ariesApiHelper = new AriesApiHelper(ariesApi);
        }

        [Test]
        public void TestAriesApiHelperObject()
        {
            Assert.IsTrue(isServerExists);
            Assert.IsNotNull(ariesApiHelper.simulation);
            Assert.IsNotNull(ariesApiHelper.simulation.clusters);
            Assert.IsNotNull(ariesApiHelper.simulation.agents);
            Assert.IsNotNull(ariesApiHelper.simulation.paths);
            Assert.IsNotNull(ariesApiHelper.simulation.nodes);
            Assert.IsNotNull(ariesApiHelper.simulation.lines);
        }

        [Test]
        public void TestGetCurrentStep()
        {
            Assert.IsTrue(isServerExists);
            SimulationStep simulationStep = ariesApiHelper.GetCurrentStep();
            Assert.IsNotNull(simulationStep);
            Assert.IsNotNull(simulationStep.simulation_result, "Simulation result is not initialized");
            Assert.AreEqual(7, simulationStep.simulation_result.Count);
            Assert.IsNotNull(simulationStep.agents_states, "Agent states is not initialized");
            Assert.AreEqual(2, simulationStep.agents_states.Count);
        }

        [Test]
        public void TestGetAllAgents()
        {
            Assert.IsTrue(isServerExists);
            List<Agent> agents = ariesApiHelper.GetAllAgents();
            Assert.IsNotNull(agents);
            Assert.AreEqual(2, agents.Count);
        }

        [Test]
        public void TestGetAgent()
        {
            Assert.IsTrue(isServerExists);
            Agent agent = ariesApiHelper.GetAgent("AGENT0");
            Assert.IsNotNull(agent);
        }

        [Test]
        public void TestSwitchBatteryOn()
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SwitchBatteryOn("AGENT0");
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestSwitchBatteryOff()
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SwitchBatteryOff("AGENT0");
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestSetGlobalBatteryConusmption()
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SetGlobalBatteryConsumption(100);
        }

        [Test]
        public void TestSwitchPVPannelOn()
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SwitchPVPannelOn("AGENT0");
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestSwitchPVPannelOff()
        {
            Assert.IsTrue(isServerExists);
            PVPanelState pvPanel = new PVPanelState
            {
                active = 1
            };
            Assert.IsNull(pvPanel.solar_irradiance);

            ariesApiHelper.SwitchPVPannelOff("AGENT0");
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test, TestCaseSource(typeof(TestDataClass), "TestSetAgentConsumption")]
        public void TestSetAgentConsumption(string agentId, ConsumptionLevel level)
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SetAgentConsumption(agentId, level);
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetAgentsConsumption(ConsumptionLevel level)
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SetAgentsConsumption(new List<string>() { "AGENT0", "AGENT1" }, level);
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetAgentsConsumptionWithNull(ConsumptionLevel level)
        {
            var ex = Assert.Throws<NullReferenceException>(() => ariesApiHelper.SetAgentsConsumption(null, level));
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetAgentsConsumptionWithEmptyList(ConsumptionLevel level)
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SetAgentsConsumption(new List<string>() { }, level);
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetGlobalConsumption(ConsumptionLevel level)
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SetGlobalConsumption(level);
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestAssignDefaultClusters()
        {
            Assert.IsTrue(isServerExists);
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestSwitchOnCentralizedMode()
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SwitchOnCentralizedMode();
        }
////
//        [Test]
//        public void TestSwitchOnDistributedMode()
//        {
//            Assert.IsTrue(isServerExists);
//            ariesApiHelper.SwitchOnDistributedMode(TestHelper.GetBinPath() + CLUSTER);
//        }
//
        [Test, TestCaseSource(typeof(TestDataClass), "TestSetDaytime")]
        public void TestSetDaytime(Daytime daytime)
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SetDaytime(daytime);
            // Check Post data result
            // Web server should store it somewhere
        }


        [TestCase(0)]
        [TestCase(20)]
        [TestCase(40)]
        [TestCase(60)]
        [TestCase(80)]
        [TestCase(100)]
        public void TestSetWindSpeed(float windspeed)
        {
            Assert.IsTrue(isServerExists);
            ariesApiHelper.SetWindSpeed(windspeed);
            // Check Post data result
            // Web server should store it somewhere
        }
    }
}