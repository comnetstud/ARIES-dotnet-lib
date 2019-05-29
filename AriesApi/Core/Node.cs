using System.Collections.Generic;

namespace Aries.Core
{
    /// <summary>
    ///     Node class
    /// </summary>
    public class Node
    {
        public List<string> branches_in { get; set; }
        public List<string> branches_out { get; set; }
        public List<List<string>> adjacency { get; set; }
        public string agent { get; set; }
    }
}