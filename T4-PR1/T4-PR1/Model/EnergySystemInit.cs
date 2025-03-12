namespace T4_PR1.Model
{
    public class EnergySystemInit
    {
        /// <summary>
        /// Defaults to solar if it can not find a matching value
        /// </summary>
        /// <param name="type">Checks the value and returns a new object</param>
        /// <returns></returns>
        public static Simulation CreateEnergySystem(string type)
        {
            return type.ToLower() switch
            {
                "solar" => new SolarEnergy(),
                "wind" => new WindEnergy(),
                "hydro" => new HydroEnergy(),
                _ => new SolarEnergy()
            };
        }

    }
}
