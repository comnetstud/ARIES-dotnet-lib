using System;
using System.Collections.Generic;
using Aries.Core;
using Aries.Test;
using NUnit.Framework;

namespace Aries.RealServer.Test
{
    [TestFixture]
    public class TestForRealServerAriesApi
    {
        private AriesApi ariesApi;
        
        private static readonly string HOST = "localhost";
        private static readonly int PORT = 8080;

        private static bool isServerExists = true;

        [SetUp]
        public void ClassInit()
        {
            isServerExists = TestHelper.RemoteFileExists(String.Format("http://{0}:{1}/", HOST, PORT));
            ariesApi = new AriesApi(HOST, PORT);
        }

        [Test]
        public void TestSimulation()
        {
            Assert.IsTrue(isServerExists);
            Simulation simulation = this.ariesApi.GetSimulation();
            Assert.AreEqual(2, simulation.agents.Count, "Simulation number of Agents");
            Assert.AreEqual(3, simulation.lines.Count, "Simulation number of Lines");
            Assert.AreEqual(4, simulation.nodes.Count, "Simulation number of Nodes");
            Assert.AreEqual(2, simulation.paths.Count, "Simulation number of Paths");
            Assert.IsNotNull(simulation.start_time);
            Assert.IsNotNull(simulation.solver);
        }

        [Test]
        public void TestSimulationStep()
        {
            Assert.IsTrue(isServerExists);
            SimulationStep simulationStep = this.ariesApi.GetSimulationStep();
            Assert.IsNotNull(simulationStep.simulation_result, "Simulation result is not initialized");
            Assert.AreEqual(7, simulationStep.simulation_result.Count);
            Assert.IsNotNull(simulationStep.agents_states, "Agent states is not initialized");
            Assert.AreEqual(2, simulationStep.agents_states.Count);
        }

        [Test]
        public void TestAgent()
        {
            Assert.IsTrue(isServerExists);
            SimulationStep simulationStep = this.ariesApi.GetSimulationStep();
            List<Agent> agents = ariesApi.GetAgents(simulationStep._id);
        }

        [Test]
        public void TestStates()
        {
            Assert.IsTrue(isServerExists);
            State state = new State();
            state.power_rating = 1000;
            state.power_factor = 0.95;
            state.incoming_power = 0;
            state.battery = new BatteryState();
            state.battery.contribution_active = 0.1;
            state.battery.contribution_reactive = 0.1;
            state.battery.active = 1;

            state.electrical_vehicle = new ElectricalVehicleState();
            state.electrical_vehicle.contribution_active = 0.3;
            state.electrical_vehicle.contribution_reactive = 0.3;
            state.electrical_vehicle.power_supplier = 0.3;
            state.electrical_vehicle.active = 1;

            state.pv_panel = new PVPanelState();
            state.pv_panel.solar_irradiance = 1;
            state.pv_panel.heating_contribution = 0.3;
            state.pv_panel.active = 1;

            state.water_tank = new WaterTankState();
            state.water_tank.active = 1;

            state.wind_generator = new WindGeneratorState();
            state.wind_generator.wind_speed = 10;
            state.wind_generator.heating_contribution = 0.3;
            state.wind_generator.active = 1;

            Dictionary<string, State> states = new Dictionary<string, State>();
            states.Add("AGENT0", state);
            states.Add("AGENT1", state);
            states.Add("AGENT2", state);

            Assert.IsTrue(this.ariesApi.PostState(states));
        }

        [Test]
        public void TestClusters()
        {
            Assert.IsTrue(isServerExists);
            Cluster cluster = new Cluster();
            cluster.cluster_agents = new List<string>();
            cluster.cluster_agents.Add("AGENT0");
            cluster.controller = "LoadSharingCluster";
            cluster.priority = 1;
            cluster.delay = 0;

            Dictionary<string, Cluster> clusters = new Dictionary<string, Cluster>();
            clusters.Add("cluster0", cluster);

            Assert.IsTrue(this.ariesApi.PostCluster(clusters));
        }
    }
}