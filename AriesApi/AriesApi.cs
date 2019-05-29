using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Aries.Core;
using Aries.Rest;
using Newtonsoft.Json;
using RestSharp;

namespace Aries
{
    /// <summary>
    ///     Custom DateTime converter for JSON
    /// </summary>
    internal class AriesDateTimeConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var rawDate = (string) reader.Value;
            DateTime date;

            // First try to parse the date string as is (in case it is correctly formatted)
            if (DateTime.TryParseExact(rawDate, "yyyy-MM-dd H:mm:ss.mmmmmm", new CultureInfo("de-DE"),
                DateTimeStyles.None, out date)) return date;

            // It's not a date after all, so just return the default value
            if (objectType == typeof(DateTime?))
                return null;

            return DateTime.MinValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     Provided interface for connection to REST API of ARIES simulator
    /// </summary>
    public class AriesApi
    {
        // ARIES REST API endpoints
        private static readonly string SIMULATION_URI = "api/aries/simulation/active";
        private static readonly string SIMULATION_STEP_URI = "api/aries/simulationstep/active";
        private static readonly string AGENT_BY_SIMULATION_STEP_URI = "api/aries/agent/simulationstep/{0}";
        public static readonly string CLUSTER_URI_BY_SIMULATION_ID = "api/aries/cluster/{0}";
        public static readonly string CLUSTER_URI = "api/aries/cluster";
        public static readonly string STATE_URI = "api/aries/state";

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Converters = new List<JsonConverter> {new AriesDateTimeConverter()}
        };

        private readonly UriBuilder _uriBuilder;

        public AriesApi(string host, int port)
        {
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            WebRequest.DefaultWebProxy = null;
            _uriBuilder = new UriBuilder
            {
                Scheme = "http",
                Host = host,
                Port = port
            };
        }

        /// <summary>
        ///     Retrieve Simulation
        /// </summary>
        /// <returns>Simulation</returns>
        public Simulation GetSimulation()
        {
            var resp = GetData(SIMULATION_URI);
            return JsonConvert.DeserializeObject<Simulation>(resp.data, _settings);
        }

        /// <summary>
        ///     Retrieve current simulation step
        /// </summary>
        /// <returns>Simulation step</returns>
        public SimulationStep GetSimulationStep()
        {
            var resp = GetData(SIMULATION_STEP_URI);
            return JsonConvert.DeserializeObject<SimulationStep>(resp.data, _settings);
        }

        /// <summary>
        ///     Retrieve all agents by simulation step id
        /// </summary>
        /// <param name="simulationStepId">Simulation step id</param>
        /// <returns></returns>
        public List<Agent> GetAgents(string simulationStepId)
        {
            var resp = GetData(string.Format(AGENT_BY_SIMULATION_STEP_URI, simulationStepId));
            var respDict =
                JsonConvert.DeserializeObject<Dictionary<string, List<Agent>>>(resp.data, _settings);
            respDict.TryGetValue("agents", out var agents);
            return agents;
        }

        /// <summary>
        ///     Retrieve all agents by simulation step id
        /// </summary>
        /// <param name="simulationId">Simulation step id</param>
        /// <returns></returns>
        public ClusterContainer GetClusters(string simulationId)
        {
            var resp = GetData(string.Format(CLUSTER_URI_BY_SIMULATION_ID, simulationId));
            var respDict =
                JsonConvert.DeserializeObject<ClusterContainer>(resp.data, _settings);
            return respDict;
        }

        /// <summary>
        ///     Post changes of Agent states
        /// </summary>
        /// <param name="states">Dictionary: key is AgentId, value is a State object</param>
        /// <returns>If everything goes fine return True, otherwise False</returns>
        public bool PostState(Dictionary<string, State> states)
        {
            return PostData(STATE_URI, states);
        }

        /// <summary>
        ///     Post cluster configuration
        /// </summary>
        /// <param name="clusters">Dictionary: key is ClusterId, value is a Cluster object</param>
        /// <returns>If everything goes fine return True, otherwise False</returns>
        public bool PostCluster(Dictionary<string, Cluster> clusters)
        {
            return PostData(CLUSTER_URI, clusters);
        }

        /// <summary>
        ///     Helper method to send object as Post to ARIES REST API
        /// </summary>
        /// <param name="url">Endpoint</param>
        /// <param name="obj">Object (will be serialized by method itself)</param>
        /// <returns>If everything goes fine return True, otherwise False</returns>
        public bool PostData(string url, object obj)
        {
            return PostData(url, JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        /// <summary>
        ///     Helper method to send file as Post to ARIES REST API
        /// </summary>
        /// <param name="url">Endpoint</param>
        /// <param name="obj">File path with JSON data</param>
        /// <returns>If everything goes fine return True, otherwise False</returns>
        public bool PostFile(string url, string filepath)
        {
            try
            {
                if (!File.Exists(filepath))
                    throw new FileNotFoundException();
                return PostData(url, File.ReadAllText(filepath));
            }
            catch (FileNotFoundException)
            {
            }

            return false;
        }

        /// <summary>
        ///     Helper method to send data as Post to ARIES REST API
        /// </summary>
        /// <param name="url">Endpoint</param>
        /// <param name="data">JSON String</param>
        /// <returns>If everything goes fine return True, otherwise False</returns>
        public bool PostData(string url, string data)
        {
            var client = new RestClient();
            client.BaseUrl = _uriBuilder.Uri;

            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.ResponseStatus == ResponseStatus.Completed;
        }

        /// <summary>
        ///     Helper method to retrieve data as Get requst from ARIES REST API
        /// </summary>
        /// <param name="url">Endpoint</param>
        /// <returns>JSendReponse object with data</returns>
        private JSendResponse GetData(string url)
        {
            var client = new RestClient();
            client.BaseUrl = _uriBuilder.Uri;
            var request = new RestRequest(url, Method.GET);
            var resp = client.Execute<JSendResponse>(request);
            if (resp.StatusCode == HttpStatusCode.OK) return resp.Data;

            return new JSendResponse {status = "fail", data = null};
        }
    }
}