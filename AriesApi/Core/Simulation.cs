using System;
using System.Collections.Generic;

namespace Aries.Core
{
    /// <summary>
    ///     Simulation class
    /// </summary>
    public class Simulation
    {
        public string _id { get; set; }
        public Dictionary<string, Agent> agents { get; set; }
        public Dictionary<string, List<Path>> paths { get; set; }
        public Dictionary<string, Line> lines { get; set; }
        public Dictionary<string, Node> nodes { get; set; }
        public Dictionary<string, Cluster> clusters { get; set; }
        public string solver { get; set; }
        public DateTime start_time { get; set; }
    }
}