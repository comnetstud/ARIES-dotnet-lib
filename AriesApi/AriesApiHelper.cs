using System.Collections.Generic;
using Aries.Core;
using Newtonsoft.Json;

// Aries Api helper to work with ARIES simulator
namespace Aries
{
    /// <summary>
    ///     Consumption level of Agent with values
    /// </summary>
    public enum ConsumptionLevel
    {
        Low = 200,
        Medium = 1500,
        High = 3000
    }

    /// <summary>
    ///     Simualtion mode
    /// </summary>
    public enum SimulationMode
    {
        Centralized,
        Distributed
    }

    /// <summary>
    ///     Day time
    /// </summary>
    public enum Daytime
    {
        Night = 0,
        Day = 100,
        Evening = 50
    }


    /// <summary>
    ///     AriesApiHelper class for integrating with ARIES simulator
    /// </summary>
    public class AriesApiHelper
    {
        private static readonly string CLUSTER_JSON =
            "{\r\n  \"cluster0\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT0\",\r\n      \"AGENT1\",\r\n      \"AGENT2\",\r\n      \"AGENT3\",\r\n      \"AGENT4\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster1\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT5\",\r\n      \"AGENT6\",\r\n      \"AGENT7\",\r\n      \"AGENT8\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster2\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT13\",\r\n      \"AGENT14\",\r\n      \"AGENT15\",\r\n      \"AGENT16\",\r\n      \"AGENT17\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster3\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT9\",\r\n      \"AGENT10\",\r\n      \"AGENT11\",\r\n      \"AGENT12\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster4\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT39\",\r\n      \"AGENT18\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster5\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT19\",\r\n      \"AGENT20\",\r\n      \"AGENT21\",\r\n      \"AGENT22\",\r\n      \"AGENT23\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster6\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT24\",\r\n      \"AGENT25\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster7\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT26\",\r\n      \"AGENT27\",\r\n      \"AGENT28\",\r\n      \"AGENT29\",\r\n      \"AGENT30\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  },\r\n  \"cluster8\": {\r\n    \"cluster_agents\": [\r\n      \"AGENT31\",\r\n      \"AGENT32\",\r\n      \"AGENT33\",\r\n      \"AGENT34\",\r\n      \"AGENT35\",\r\n      \"AGENT36\",\r\n      \"AGENT37\",\r\n      \"AGENT38\"\r\n    ],\r\n    \"controller\": \"LoadSharingCluster\",\r\n    \"priority\": 1,\r\n    \"delay\": 0\r\n  }\r\n}";

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Converters = new List<JsonConverter> {new AriesDateTimeConverter()}
        };

        // Store AriesApi connector
        private readonly AriesApi _ariesApi;

        // Constructor AriesApiHelper
        public AriesApiHelper(AriesApi ariesApi)
        {
            this._ariesApi = ariesApi;
            simulation = ariesApi.GetSimulation();
        }

        // Simulation is loaded when AriesHelper initialized
        public Simulation simulation { get; set; }

        /// <summary>
        ///     Retrieve current simulation step from ARIES
        /// </summary>
        /// <returns>Simulation step object</returns>
        public SimulationStep GetCurrentStep()
        {
            return _ariesApi.GetSimulationStep();
        }

        /// <summary>
        ///     Retrieve all Agents from ARIES
        /// </summary>
        /// <returns>List of all Agents</returns>
        public List<Agent> GetAllAgents()
        {
            var simulationStep = _ariesApi.GetSimulationStep();
            return _ariesApi.GetAgents(simulationStep._id);
        }

        /// <summary>
        ///     Retrieve Agent by name from simulation
        /// </summary>
        /// <param name="AgentId">Agent Id</param>
        /// <returns>Agent object</returns>
        public Agent GetAgent(string AgentId)
        {
            var agents = GetAllAgents();
            foreach (var agent in agents)
                if (agent.name == AgentId)
                    return agent;
            return null;
        }

        /// <summary>
        /// Retrieve current cluster configuration
        /// </summary>
        /// <returns>Dictionary of clusters</returns>
        public Dictionary<string, Cluster> GetClusters()
        {
            ClusterContainer clusterContainer = _ariesApi.GetClusters(simulation._id);
            if (clusterContainer == null)
            {
                return new Dictionary<string, Cluster>();
            }

            return clusterContainer.clusters;
        }

        /// <summary>
        ///     Switch on battery for Agent by AgentId
        /// </summary>
        /// <param name="agentId">Agent Id</param>
        public void SwitchBatteryOn(string agentId)
        {
            var states = new Dictionary<string, State>();
            states.Add(agentId, new State
            {
                battery = new BatteryState
                {
                    active = 1
                }
            });
            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Switch off battery for Agent by AgentId
        /// </summary>
        /// <param name="agentId">Agent Id</param>
        public void SwitchBatteryOff(string agentId)
        {
            var states = new Dictionary<string, State>();
            states.Add(agentId, new State
            {
                battery = new BatteryState
                {
                    active = 0
                }
            });
            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Set Battery consumption for all Agents
        /// </summary>
        /// <param name="percentage">Consumption level in percentage</param>
        public void SetGlobalBatteryConsumption(float percentage)
        {
            var states = new Dictionary<string, State>();
            foreach (var agent in simulation.agents.Keys)
                states.Add(agent, new State
                {
                    battery = new BatteryState
                    {
                        contribution_active = percentage / 100.0,
                        contribution_reactive = percentage / 100.0
                    }
                });
            _ariesApi.PostState(states);
        }


        /// <summary>
        ///     Switch on PVPanel for Agent by AgentId
        /// </summary>
        /// <param name="agentId">Agent Id</param>
        public void SwitchPVPannelOn(string agentId)
        {
            var states = new Dictionary<string, State>();
            states.Add(agentId, new State
            {
                pv_panel = new PVPanelState
                {
                    active = 1
                }
            });
            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Switch off PVPanel for Agent by AgentId
        /// </summary>
        /// <param name="agentId">Agent Id</param>
        public void SwitchPVPannelOff(string agentId)
        {
            var states = new Dictionary<string, State>();
            states.Add(agentId, new State
            {
                pv_panel = new PVPanelState
                {
                    active = 0
                }
            });
            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Switch off PVPanel for Agent by AgentId
        /// </summary>
        /// <param name="agentId">Agent Id</param>
        public void SwitchAllPVPannelOn()
        {
            var states = new Dictionary<string, State>();
            foreach (var agent in simulation.agents.Keys)
                states.Add(agent, new State
                {
                    pv_panel = new PVPanelState
                    {
                        active = 1
                    }
                });
            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Switch off PVPanel for Agent by AgentId
        /// </summary>
        /// <param name="agentId">Agent Id</param>
        public void SwitchAllPVPannelOff()
        {
            var states = new Dictionary<string, State>();
            foreach (var agent in simulation.agents.Keys)
                states.Add(agent, new State
                {
                    pv_panel = new PVPanelState
                    {
                        active = 0
                    }
                });
            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Set consumption level for Agent by AgentId
        /// </summary>
        /// <param name="agentId">Agentt Id</param>
        /// <param name="level">Consumption level</param>
        public void SetAgentConsumption(string agentId, ConsumptionLevel level)
        {
            var states = new Dictionary<string, State>();
            states.Add(agentId, new State
            {
                power_rating = (int) level
            });

            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Set consumption level for list of Agents
        /// </summary>
        /// <param name="agentIds">Enumeration of AgentIds</param>
        /// <param name="level">Consumption level</param>
        public void SetAgentsConsumption(IEnumerable<string> agentIds, ConsumptionLevel level)
        {
            var states = new Dictionary<string, State>();
            foreach (var agent in agentIds)
                states.Add(agent, new State
                {
                    power_rating = (int) level
                });
            if (states.Count > 0) _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Set global consumption level for all Agents
        /// </summary>
        /// <param name="level">Consumption level</param>
        public void SetGlobalConsumption(ConsumptionLevel level)
        {
            SetAgentsConsumption(simulation.agents.Keys, level);
        }

        /// <summary>
        /// Switch between Simulation mode (Distributed and Centralized)
        /// </summary>
        /// <param name="simulationMode">Simulation mode enum</param>
        public void SetSimulationMode(SimulationMode simulationMode)
        {
            switch (simulationMode)
            {
                case SimulationMode.Centralized:
                    SwitchOnCentralizedMode();
                    break;

                case SimulationMode.Distributed:

                    SwitchOnDistributedMode();
                    break;
            }
        }

        /// <summary>
        ///     Set default cluster. This is hardcoded method.
        /// </summary>
        public void SwitchOnDistributedMode()
        {
            var states = new Dictionary<string, State>();
            foreach (var agent in simulation.agents.Keys)
                states.Add(agent, new State
                {
                    incoming_power = 0,
                    battery = new BatteryState
                    {
                        active = 1,
                        contribution_active = 0,
                        contribution_reactive = 0
                    },
                    pv_panel = new PVPanelState
                    {
                        active = 1
                    }
                });
            _ariesApi.PostState(states);
            Dictionary<string, Cluster> clusters =
                JsonConvert.DeserializeObject<Dictionary<string, Cluster>>(CLUSTER_JSON, _settings);
            simulation.clusters = clusters;
            _ariesApi.PostCluster(simulation.clusters);
        }

        /// <summary>
        ///     Set default cluster. This is hardcoded method.
        /// </summary>
        public void SwitchOnCentralizedMode()
        {
            var states = new Dictionary<string, State>();
            foreach (var agent in simulation.agents.Keys)
                states.Add(agent, new State
                {
                    incoming_power = 0,
                    battery = new BatteryState
                    {
                        active = 0,
                        contribution_active = 0,
                        contribution_reactive = 0,
                        status = 1296000 
                    },
                    pv_panel = new PVPanelState
                    {
                        active = 0
                    }
                });
            _ariesApi.PostState(states);
            simulation.clusters = new Dictionary<string, Cluster>() { };
            _ariesApi.PostCluster(simulation.clusters);
        }

        /// <summary>
        ///     Add Agent to cluster
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="clusterId"></param>
        public void AddAgentToCluster(string agentId, string clusterId)
        {
            RemoveAgentFromCluster(agentId);

            if (simulation.clusters == null) simulation.clusters = new Dictionary<string, Cluster>();
            if (!simulation.clusters.ContainsKey(clusterId)) simulation.clusters.Add(clusterId, new Cluster());
            simulation.clusters[clusterId].cluster_agents.Add(agentId);
            _ariesApi.PostCluster(simulation.clusters);
        }

        /// <summary>
        ///     Remove Agent from cluster
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="clusterId"></param>
        public void RemoveAgentFromCluster(string agentId)
        {
            if (simulation.clusters == null) return;
            string removeKey = null;
            bool isUpdateCluster = false;
            foreach (var entry in simulation.clusters)
            {
                var agentIsInList = false;
                foreach (var item in entry.Value.cluster_agents)
                    if (item.Contains(agentId))
                        agentIsInList = true;
                if (agentIsInList)
                {
                    entry.Value.cluster_agents.Remove(agentId);
                    if (entry.Value.cluster_agents.Count == 0) removeKey = entry.Key;
                    isUpdateCluster = true;
                }
            }

            if (removeKey != null) simulation.clusters.Remove(removeKey);
            if(isUpdateCluster) _ariesApi.PostCluster(simulation.clusters);
        }

        /// <summary>
        ///     Change Daytime properties for all agents
        /// </summary>
        /// <param name="time">Daytime enum</param>
        public void SetDaytime(Daytime time)
        {
            var states = new Dictionary<string, State>();
            foreach (var agent in simulation.agents.Keys)
                states.Add(agent, new State
                {
                    pv_panel = new PVPanelState
                    {
                        solar_irradiance = (int) time / 100.0
                    }
                });
            _ariesApi.PostState(states);
        }

        /// <summary>
        ///     Change windspeed for all agents
        /// </summary>
        /// <param name="speed">Windspeed</param>
        /// from 10km/h to 60km/h, individual incoming power grows linearly from
        /// 0 to 5000 W. incoming_power = 100*speed - 1000
        public void SetWindSpeed(float speed)
        {
            if (speed > 0)
            {
                SwitchOnDistributedMode();
            }
            float individualIncomingPower = 0;
            if ((speed >= 10) && (speed <= 60))
            {
                individualIncomingPower = 100 * speed - 1000;
            }
            else if (speed > 60)
            {
                individualIncomingPower = 5000;
            }

            var states = new Dictionary<string, State>();
            foreach (var agent in simulation.agents.Keys)
                states.Add(agent, new State
                {
                    incoming_power = individualIncomingPower
                });
            _ariesApi.PostState(states);
        }
    }

}