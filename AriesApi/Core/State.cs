namespace Aries.Core
{
    /// <summary>
    ///     State class
    /// </summary>
    public class State
    {
        public double? power_rating { get; set; }
        public double? power_factor { get; set; }
        public double? incoming_power { get; set; }
        public BatteryState battery { get; set; }
        public ElectricalVehicleState electrical_vehicle { get; set; }
        public PVPanelState pv_panel { get; set; }
        public WaterTankState water_tank { get; set; }
        public WindGeneratorState wind_generator { get; set; }
    }

    /// <summary>
    ///     Battery State class
    /// </summary>
    public class BatteryState : EnergyBufferState
    {
    }

    /// <summary>
    ///     Electricle Vehicle State class
    /// </summary>
    public class ElectricalVehicleState : EnergyBufferState
    {
        public double? power_supplier { get; set; }
    }

    /// <summary>
    ///     PVPanel State class
    /// </summary>
    public class PVPanelState : GridElementState
    {
        public double? solar_irradiance { get; set; }
        public double? heating_contribution { get; set; }
    }

    /// <summary>
    ///     WaterTank State class
    /// </summary>
    public class WaterTankState : GridElementState
    {
    }

    /// <summary>
    ///     WindGenerator State class
    /// </summary>
    public class WindGeneratorState : GridElementState
    {
        public double? wind_speed { get; set; }
        public double? heating_contribution { get; set; }
    }

    /// <summary>
    ///     Abstract class for battery and electrical vehicle states
    /// </summary>
    public abstract class EnergyBufferState : GridElementState
    {
        public double? status { get; set; }
        public double? contribution_active { get; set; }
        public double? contribution_reactive { get; set; }
    }

    /// <summary>
    ///     Abstract class general for all Agent state elements
    /// </summary>
    public abstract class GridElementState
    {
        public int? active { get; set; }
    }
}