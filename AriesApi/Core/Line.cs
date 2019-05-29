namespace Aries.Core
{
    /// <summary>
    ///     Line class
    /// </summary>
    public class Line
    {
        public string name { get; set; }
        public double resistance { get; set; }
        public double reactance { get; set; }
        public double shunt_resistance { get; set; }
        public double shunt_reactance { get; set; }
    }
}