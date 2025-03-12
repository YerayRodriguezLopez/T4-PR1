namespace T4_PR1.Model
{
    public class HydroEnergy : Simulation
    {
        public override string ParameterName => "Cabal d'aigua (m³/s)";
        private double _waterFlow;
        public override double ParameterValue
        {
            get => _waterFlow;
            set => _waterFlow = value;
        }

        public HydroEnergy()
        {
            Type = "Sistema Hidroelèctric";
        }

        public void CalculateEnergy()
        {
            GeneratedEnergy = ParameterValue * 9.8 * Ratio;
        }
    }
}