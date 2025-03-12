namespace T4_PR1.Model
{
    public class WindEnergy : Simulation
    {
        public override string ParameterName => "Velocitat del vent (m/s)";
        private double _windSpeed;
        public override double ParameterValue
        {
            get => _windSpeed;
            set => _windSpeed = value;
        }

        public WindEnergy()
        {
            Type = "Sistema Eòlic";
        }

        public void CalculateEnergy()
        {
            GeneratedEnergy = Math.Pow(ParameterValue, 3) * Ratio;
        }
    }
}