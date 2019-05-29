using System.Collections.Generic;

namespace Aries.Core
{
    /// <summary>
    ///     Controllers for clusters should represent in ARIES simulation
    /// </summary>
    public enum Controllers
    {
        LoadSharingCluster,
        StayingAliveCluster
    }

    /// <summary>
    ///     Cluster class
    /// </summary>
    public class Cluster
    {
        public Cluster()
        {
            cluster_agents = new List<string>();
            controller = Controllers.LoadSharingCluster.ToString();
        }

        public List<string> cluster_agents { get; set; }
        public string controller { get; set; }
        public int priority { get; set; }
        public int delay { get; set; }
    }
}