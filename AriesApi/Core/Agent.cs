namespace Aries.Core
{
    /// <summary>
    ///     Agent class
    /// </summary>
    public class Agent
    {
        public string name { get; set; }
        public double voltage_rating { get; set; }
        public double power_rating { get; set; }
        public double power_factor { get; set; }
        public double incoming_power { get; set; }
        public double request_inject_power { get; set; }
        public double request_power_factor { get; set; }
        public Battery battery { get; set; }
        public PVPanel pv_panel { get; set; }
        public WindGenerator wind_generator { get; set; }
        public ElectricalVehicle electrical_vehicle { get; set; }
        public WaterTank water_tank { get; set; }
    }

    /// <summary>
    ///     Water tank class
    /// </summary>
    public class WaterTank : GridElement
    {
        public double capacity { get; set; }
        public double temp { get; set; }
    }

    /// <summary>
    ///     ElectricalVehicle class
    /// </summary>
    public class ElectricalVehicle : GridElement
    {
        public double voltage { get; set; }
        public double capacity { get; set; }
        public double status { get; set; }
        public double contribution_active { get; set; }
        public double contribution_reactive { get; set; }
        public double inverter_input_voltage { get; set; }
        public double inverter_output_voltage { get; set; }
        public double inverter_efficiency { get; set; }
        public double charge_current { get; set; }
        public double power_supplier { get; set; }
    }

    /// <summary>
    ///     WindGenerator class
    /// </summary>
    public class WindGenerator : GridElement
    {
        public double power_coefficient { get; set; }
        public double air_density { get; set; }
        public double area { get; set; }
        public double wind_speed { get; set; }
        public double battery_coupling_efficiency { get; set; }
    }

    /// <summary>
    ///     PVVPanel class
    /// </summary>
    public class PVPanel : GridElement
    {
        public double unit_area { get; set; }
        public double series { get; set; }
        public double parallels { get; set; }
        public double efficiency { get; set; }
        public double solar_irradiance { get; set; }
        public double battery_coupling_efficiency { get; set; }
        public double heating_contribution { get; set; }
    }

    /// <summary>
    ///     Battery class
    /// </summary>
    public class Battery : EnergyBuffer
    {
    }

    /// <summary>
    ///     Abstract class for Battery and ElectricleVehicle
    /// </summary>
    public abstract class EnergyBuffer : GridElement
    {
        public double voltage { get; set; }
        public double capacity { get; set; }
        public double status { get; set; }
        public double contribution_active { get; set; }
        public double contribution_reactive { get; set; }
        public double inverter_input_voltage { get; set; }
        public double inverter_output_voltage { get; set; }
        public double inverter_efficiency { get; set; }
    }

    /// <summary>
    ///     Abstract class for all Agent elements
    /// </summary>
    public abstract class GridElement
    {
        public int active { get; set; }
    }
}