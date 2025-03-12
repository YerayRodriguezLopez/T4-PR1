namespace T4_PR1.Model
{
    public class SimulationManager
    {
        private const string SimulationsFilePath = "ModelData/simulacions_energia.csv";

        public SimulationManager()
        {
            FileExistsOrDefault();
        }

        private void FileExistsOrDefault()
        {
            string? directory = Path.GetDirectoryName(SimulationsFilePath);
            if (!Directory.Exists(directory))
            {
                if (directory != null) Directory.CreateDirectory(directory);
            }

            if (!File.Exists(SimulationsFilePath))
            {
                using (var writer = new StreamWriter(SimulationsFilePath))
                {
                    writer.WriteLine("Data,Tipus,Parametre,Valor,Rati,EnergiaGenerada,CostPerKWh,PreuPerKWh,CostTotal,PreuTotal");
                }
            }
        }

        public List<Simulation> LoadSimulations()
        {
            var simulations = new List<Simulation>();

            try
            {
                if (File.Exists(SimulationsFilePath))
                {
                    var lines = File.ReadAllLines(SimulationsFilePath).Skip(1);

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length < 10) continue;

                        Simulation model;

                        switch (parts[1])
                        {
                            case "Sistema Solar":
                                model = new SolarEnergy();
                                break;
                            case "Sistema Eòlic":
                                model = new WindEnergy();
                                break;
                            case "Sistema Hidroelèctric":
                                model = new HydroEnergy();
                                break;
                            default:
                                continue;
                        }

                        model.SimulationDate = DateTime.Parse(parts[0]);
                        model.Type = parts[1];
                        model.ParameterValue = double.Parse(parts[3]);
                        model.Ratio = double.Parse(parts[4]);
                        model.GeneratedEnergy = double.Parse(parts[5]);
                        model.CostPerKWh = double.Parse(parts[6]);
                        model.PricePerKWh = double.Parse(parts[7]);

                        simulations.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading simulations: {ex.Message}");
            }

            return simulations;
        }

        public void SaveSimulation(Simulation simulation)
        {
            try
            {
                string line = $"{simulation.SimulationDate:yyyy-MM-dd HH:mm:ss}," +
                          $"{simulation.Type}," +
                          $"{simulation.ParameterName}," +
                          $"{simulation.ParameterValue}," +
                          $"{simulation.Ratio}," +
                          $"{simulation.GeneratedEnergy}," +
                          $"{simulation.CostPerKWh}," +
                          $"{simulation.PricePerKWh}," +
                          $"{simulation.TotalCost}," +
                          $"{simulation.TotalPrice}";

                File.AppendAllLines(SimulationsFilePath, [line]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving simulation: {ex.Message}");
            }
        }
    }
}
