using System.ComponentModel.DataAnnotations;

namespace T4_PR1.Model
{
    public abstract class Simulation
    {
        public DateTime SimulationDate { get; set; } = DateTime.Now;
        public string Type { get; set; } = string.Empty;
        public double Ratio { get; set; }
        public double GeneratedEnergy { get; set; }
        public double CostPerKWh { get; set; }
        public double PricePerKWh { get; set; }
        public double TotalCost => GeneratedEnergy * CostPerKWh;
        public double TotalPrice => GeneratedEnergy * PricePerKWh;

        public abstract string ParameterName { get; }
        public abstract double ParameterValue { get; set; }
    }
}
