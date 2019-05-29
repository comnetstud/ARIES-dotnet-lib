using System.Collections.Generic;

namespace Aries.Core
{
    /// <summary>
    ///     SimulationStep class
    /// </summary>
    public class SimulationStep
    {
        public string _id { get; set; }
        public int simulation_step { get; set; }
        public double total_pv_power { get; set; }
        public string simulation_id { get; set; }
        public Dictionary<string, Power> simulation_result { get; set; }
        public Dictionary<string, AgentState> agents_states { get; set; }
        public bool valid { get; set; }
    }

    /// <summary>
    ///     Power result class
    /// </summary>
    public class Power
    {
        public double real { get; set; }
        public double imag { get; set; }
    }

    /// <summary>
    ///     Agent state for each Simulation Step
    /// </summary>
    public class AgentState
    {
        public Impedance impedance { get; set; }
        public ActivePower inject_power { get; set; }
        public ActivePower demand_power { get; set; }
        public ActivePower battery_power { get; set; }
    }

    /// <summary>
    ///     Impedance
    /// </summary>
    public class Impedance
    {
        public double resistance { get; set; }
        public double reactance { get; set; }
    }

    /// <summary>
    ///     Active and Reactive Power
    /// </summary>
    public class ActivePower
    {
        public double active_power { get; set; }
        public double reactive_power { get; set; }
    }
}