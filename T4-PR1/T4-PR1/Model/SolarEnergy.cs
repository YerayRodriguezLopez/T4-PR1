namespace T4_PR1.Model
{
    public class SolarEnergy : Simulation
    {
        public override string ParameterName => "Hores de sol";
        private double _sunHours;
        public override double ParameterValue
        {
            get => _sunHours;
            set => _sunHours = value;
        }

        public SolarEnergy()
        {
            Type = "Sistema Solar";
        }

        public void CalculateEnergy()
        {
            GeneratedEnergy = ParameterValue * Ratio;
        }
    }
}
