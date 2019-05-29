using System;
using System.Collections.Generic;
using System.Threading;
using Aries.Core;
using Aries.Test;
using NUnit.Framework;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace Aries.MockServer.Test
{
    [SetUpFixture]
    public class RunWebServerFixture : IDisposable
    {
        private WebServer server;

        public const string HOST = "localhost";
        public const int PORT = 12345;

        public RunWebServerFixture()
        {
            Console.WriteLine("Constructing NamespaceInitializer .");
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            server = new WebServer($"http://{HOST}:{PORT}/", RoutingStrategy.Regex);
            server.RegisterModule(new WebApiModule());
            server.Module<WebApiModule>().RegisterController<AriesWebController>();
            var runTask = server.RunAsync();

            Console.WriteLine("Setting up NamespaceInitializer .");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Console.WriteLine("Tearing down NamespaceInitializer .");
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing NamespaceInitializer .");
        }
    }

    [TestFixture]
    public class TestForMockServerAriesApi
    {
        private AriesApi ariesApi;

        [SetUp]
        public void TestInitialize()
        {
            ariesApi = new AriesApi(RunWebServerFixture.HOST, RunWebServerFixture.PORT);
            TestHelper.CleaunUpResult("state");
            TestHelper.CleaunUpResult("cluster");
        }

        [Test]
        public void TestSimulation()
        {
            Simulation simulation = ariesApi.GetSimulation();
            Assert.AreEqual(simulation.agents.Count, 2);
            Assert.AreEqual(simulation.lines.Count, 3);
            Assert.AreEqual(simulation.nodes.Count, 4);
            Assert.AreEqual(simulation.paths.Count, 2);
            Assert.IsNotNull(simulation.start_time);
            Assert.IsNotNull(simulation.solver);
        }

        [Test]
        public void TestSimulationStep()
        {
            SimulationStep simulationStep = this.ariesApi.GetSimulationStep();
            
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["B0"].real, 3),
                Math.Round(10.736046501091344, 3));
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["B0"].imag, 3),
                Math.Round(-0.03351327202899204, 3));

            Assert.AreEqual(Math.Round(simulationStep.simulation_result["B1"].real, 3),
                Math.Round(4.298619745304598, 3));
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["B1"].imag, 3),
                Math.Round(-0.012355199796137507, 3));

            Assert.AreEqual(Math.Round(simulationStep.simulation_result["B2"].real, 3),
                Math.Round(6.437426755786747, 3));
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["B2"].imag, 3),
                Math.Round(-0.02115807223285453, 3));

            Assert.AreEqual(Math.Round(simulationStep.simulation_result["AGENT0"].real, 3),
                Math.Round(4.298619745304598, 3));
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["AGENT0"].imag, 3),
                Math.Round(-0.012355199796137507, 3));

            Assert.AreEqual(Math.Round(simulationStep.simulation_result["AGENT1"].real, 3),
                Math.Round(6.437426755786747, 3));
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["AGENT1"].imag, 3),
                Math.Round(-0.02115807223285453, 3));

            Assert.AreEqual(Math.Round(simulationStep.simulation_result["power_from_main"].real, 3),
                Math.Round(2469.2906952510093, 3));
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["power_from_main"].imag, 3),
                Math.Round(7.70805256666817, 3));

            Assert.AreEqual(Math.Round(simulationStep.simulation_result["distribution_loss"].real, 3),
                Math.Round(30.306661228036212, 3));
            Assert.AreEqual(Math.Round(simulationStep.simulation_result["distribution_loss"].imag, 3),
                Math.Round(0.0, 3));
            
            Assert.IsNotNull(simulationStep.agents_states, "Agent states is not initialized");
            
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].impedance.resistance, 3),
                Math.Round(52.9, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].impedance.reactance, 3),
                Math.Round(12.1231233, 3));

            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].inject_power.active_power, 3),
                Math.Round(433.1234, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].inject_power.reactive_power, 3),
                Math.Round(1234.12344, 3));

            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].demand_power.active_power, 3),
                Math.Round(344.234, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].demand_power.reactive_power, 3),
                Math.Round(2345.6236, 3));
            
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].battery_power.active_power, 3),
                Math.Round(-236.65, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT0"].battery_power.reactive_power, 3),
                Math.Round(3565.656256, 3));
            
            
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].impedance.resistance, 3),
                Math.Round(152.9, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].impedance.reactance, 3),
                Math.Round(112.1231233, 3));

            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].inject_power.active_power, 3),
                Math.Round(1433.1234, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].inject_power.reactive_power, 3),
                Math.Round(11234.12344, 3));

            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].demand_power.active_power, 3),
                Math.Round(1344.234, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].demand_power.reactive_power, 3),
                Math.Round(12345.6236, 3));
            
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].battery_power.active_power, 3),
                Math.Round(-1236.65, 3));
            Assert.AreEqual(Math.Round(simulationStep.agents_states["AGENT1"].battery_power.reactive_power, 3),
                Math.Round(13565.656256, 3));
            
        }

        [Test]
        public void TestAgent()
        {
            SimulationStep simulationStep = this.ariesApi.GetSimulationStep();
            List<Agent> agents = ariesApi.GetAgents(simulationStep._id);
        }

        [Test]
        public void TestStates()
        {
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

            Console.Write(TestHelper.ReadResult("state"));
        }

        [Test]
        public void TestClusters()
        {
            Cluster cluster = new Cluster();
            cluster.cluster_agents = new List<string>();
            cluster.cluster_agents.Add("AGENT0");
            cluster.controller = "LoadSharingCluster";
            cluster.priority = 1;
            cluster.delay = 0;

            Dictionary<string, Cluster> clusters = new Dictionary<string, Cluster>();
            clusters.Add("cluster0", cluster);

            Assert.IsTrue(this.ariesApi.PostCluster(clusters));

            Console.Write(TestHelper.ReadResult("cluster"));
        }
    }
}
//}