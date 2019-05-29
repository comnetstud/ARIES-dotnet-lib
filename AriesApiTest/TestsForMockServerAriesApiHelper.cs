using System;
using System.Collections.Generic;
using Aries.Core;
using Aries.Test;
using NUnit.Framework;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace Aries.MockServer.Test
{
    [TestFixture]
    public class TestsForMockServerAriesApiHelper
    {
        private AriesApi ariesApi;
        private AriesApiHelper ariesApiHelper;

        private static WebServer server;

        private static readonly string HOST = "localhost";
        private static readonly int PORT = 1234;

        [SetUp]
        public void InitServer()
        {
            server = new WebServer(String.Format("http://{0}:{1}/", HOST, PORT), RoutingStrategy.Regex);
            server.RegisterModule(new WebApiModule());
            server.Module<WebApiModule>().RegisterController<AriesWebController>();
            var runTask = server.RunAsync();

            ariesApi = new AriesApi(HOST, PORT);
            ariesApiHelper = new AriesApiHelper(ariesApi);
        }

        [Test]
        public void TestAriesApiHelperObject()
        {
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
            SimulationStep simulationStep = ariesApiHelper.GetCurrentStep();
            Assert.IsNotNull(simulationStep);
            Assert.AreEqual(7, simulationStep.simulation_result.Count);
        }

        [Test]
        public void TestGetAllAgents()
        {
            List<Agent> agents = ariesApiHelper.GetAllAgents();
            Assert.IsNotNull(agents);
            Assert.AreEqual(2, agents.Count);
        }

        [Test]
        public void TestGetAgent()
        {
            Agent agent = ariesApiHelper.GetAgent("AGENT0");
            Assert.IsNotNull(agent);
        }

        [Test]
        public void TestSwitchBatteryOn()
        {
            ariesApiHelper.SwitchBatteryOn("AGENT0");
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestSwitchBatteryOff()
        {
            ariesApiHelper.SwitchBatteryOff("AGENT0");
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestSwitchPVPannelOn()
        {
            ariesApiHelper.SwitchPVPannelOn("AGENT0");
            // Check Post data result
            // Web server should store it somewhere
        }

        [Test]
        public void TestSwitchPVPannelOff()
        {
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
            ariesApiHelper.SetAgentConsumption(agentId, level);
            Dictionary<string, State> statesFromJSON = TestHelper.ReadState();
            State state = statesFromJSON[agentId];
            Assert.AreEqual(state.power_rating, (int)level);
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetAgentsConsumption(ConsumptionLevel level)
        {
            string agent0 = "AGENT0";
            string agent1 = "AGENT1";
            ariesApiHelper.SetAgentsConsumption(new List<string>() { agent0, agent1}, level);
            Dictionary<string, State> statesFromJSON = TestHelper.ReadState();
            State state = statesFromJSON[agent0];
            Assert.AreEqual(state.power_rating, (int)level);
            state = statesFromJSON[agent1];
            Assert.AreEqual(state.power_rating, (int)level);
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetAgentsConsumptionWithNull(ConsumptionLevel level)
        {
            Assert.Throws<NullReferenceException>(() => ariesApiHelper.SetAgentsConsumption(null, level));
            Dictionary<string, State> statesFromJSON = TestHelper.ReadState();
            Assert.AreEqual(statesFromJSON.Count, 0);
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetGlobalConsumption(ConsumptionLevel level)
        {
            ariesApiHelper.SetGlobalConsumption(level);
            Dictionary<string, State> statesFromJSON = TestHelper.ReadState();
            foreach(string agentId in ariesApiHelper.simulation.agents.Keys){
                Assert.AreEqual(statesFromJSON[agentId].power_rating, (int)level);
            }
        }

        [Test]
        public void TestAddAgentToCluster()
        {
            
        }

        [Test, TestCaseSource(typeof(TestDataClass), "TestSetDaytime")]
        public void TestSetDaytime(Daytime daytime)
        {
            ariesApiHelper.SetDaytime(daytime);
            Dictionary<string, State> statesFromJSON = TestHelper.ReadState();
            foreach (string agentId in ariesApiHelper.simulation.agents.Keys)
            {
                Assert.AreEqual(statesFromJSON[agentId].pv_panel.solar_irradiance, (int)daytime / 100.0);
            }
        }


        [TestCase(0)]
        [TestCase(20)]
        [TestCase(40)]
        [TestCase(60)]
        [TestCase(80)]
        [TestCase(100)]
        public void TestSetWindSpeed(float windspeed)
        {
            ariesApiHelper.SetWindSpeed(windspeed);
            Dictionary<string, State> statesFromJSON = TestHelper.ReadState();
            foreach (string agentId in ariesApiHelper.simulation.agents.Keys)
            {
                Assert.AreEqual(statesFromJSON[agentId].wind_generator.wind_speed, windspeed);
            }
        }

        [Test, TestCaseSource(typeof(TestDataClass), "ConsumptionLevels")]
        public void TestSetAgentsConsumptionWithEmptyList(ConsumptionLevel level)
        {
            ariesApiHelper.SetAgentsConsumption(new List<string>() { }, level);
            Dictionary<string, State> statesFromJSON = TestHelper.ReadState();
            Assert.AreEqual(statesFromJSON.Count, 0);
        }

        [Test]
        public void TestSetGlobalBatteryConusmption()
        {
            ariesApiHelper.SetGlobalBatteryConsumption(100);
        }

//        [Test]
//        public void TestSwitchOnDistributedMode()
//        {
//            ariesApiHelper.SwitchOnDistributedMode(TestHelper.GetBinPath(@"Resources\TestsForMockServerAriesHelper\cluster.json"));
//        }

        [Test]
        public void TestSwitchOnCentralizedMode()
        {
            ariesApiHelper.SwitchOnCentralizedMode();
        }

    }
}
