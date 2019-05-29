using System.Collections.Generic;

namespace Aries.Core
{
    public class ClusterContainer
    {
        public string _id { get; set; }
        public string simulation_id { get; set; }
        public string simulation_step { get; set; }
        public Dictionary<string, Cluster> clusters { get; set; }
        public bool is_applied { get; set; }
    }
}