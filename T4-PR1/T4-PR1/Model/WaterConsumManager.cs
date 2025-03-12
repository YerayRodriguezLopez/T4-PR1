using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using CsvHelper;

namespace T4_PR1.Model
{
    public class WaterConsumManager
    {
        private const string WaterUsageFilePath = "ModelData/consum_aigua_cat_per_comarques.csv";
        private const string WaterUsageXmlPath = "ModelData/consum_aigua_cat_per_comarques.xml";

        public WaterConsumManager()
        {
            FileExistsOrCreateXml();
        }

        private void FileExistsOrCreateXml()
        {
            string? directory = Path.GetDirectoryName(WaterUsageXmlPath);
            if (!Directory.Exists(directory))
            {
                if (directory != null) Directory.CreateDirectory(directory);
            }

            if (!File.Exists(WaterUsageXmlPath))
            {
                var doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Consums")
                );
                doc.Save(WaterUsageXmlPath);
            }
        }

        public List<WaterConsum> LoadUsages()
        {
            var usages = new List<WaterConsum>();

            try
            {
                if (File.Exists(WaterUsageFilePath))
                {
                    using var reader = new StreamReader(WaterUsageFilePath);
                    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                    csv.Read();
                    csv.ReadHeader();

                    csv.Context.TypeConverterOptionsCache.GetOptions<double>().CultureInfo = new CultureInfo("ca-ES");

                    while (csv.Read())
                    {
                        try
                        {
                            var consum = new WaterConsum
                            {
                                Any = csv.GetField<int>(0),
                                CodiComarca = csv.GetField<int>(1),
                                Comarca = csv.GetField<string>(2) ?? string.Empty,
                                Poblacio = csv.GetField<int>(3),
                                DomesticXarxa = csv.GetField<int>(4),
                                ActivitatsEconomiquesIFontsPropies = csv.GetField<int>(5),
                                Total = csv.GetField<int>(6),
                                ConsumDomesticPerCapita = csv.GetField<double>(7)
                            };

                            usages.Add(consum);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                if (File.Exists(WaterUsageXmlPath))
                {
                    var doc = XDocument.Load(WaterUsageXmlPath);
                    if (doc.Root == null) return usages;
                    foreach (var element in doc.Root.Elements("Consum"))
                    {
                        var consum = new WaterConsum
                        {
                            Any = int.Parse(element.Element("Any")?.Value ?? "0"),
                            CodiComarca = int.Parse(element.Element("CodiComarca")?.Value ?? "0"),
                            Comarca = element.Element("Comarca")?.Value ?? string.Empty,
                            Poblacio = int.Parse(element.Element("Poblacio")?.Value ?? "0"),
                            DomesticXarxa = int.Parse(element.Element("DomesticXarxa")?.Value ?? "0"),
                            ActivitatsEconomiquesIFontsPropies = int.Parse(element.Element("ActivitatsEconomiques")?.Value ?? "0"),
                            Total = int.Parse(element.Element("Total")?.Value ?? "0"),
                            ConsumDomesticPerCapita = double.Parse(element.Element("ConsumPerCapita")?.Value.Replace('.', ',') ?? "0")
                        };

                        usages.Add(consum);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading water consumption data: {ex.Message}");
            }

            return usages;
        }

        public void SaveUsage(WaterConsum usage)
        {
            try
            {
                XDocument doc;
                if (File.Exists(WaterUsageXmlPath))
                {
                    doc = XDocument.Load(WaterUsageXmlPath);
                }
                else
                {
                    doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),
                        new XElement("Consums")
                    );
                }

                var newUsage = new XElement("Consum",
                    new XElement("Any", usage.Any),
                    new XElement("CodiComarca", usage.CodiComarca),
                    new XElement("Comarca", usage.Comarca),
                    new XElement("Poblacio", usage.Poblacio),
                    new XElement("DomesticXarxa", usage.DomesticXarxa),
                    new XElement("ActivitatsEconomiques", usage.ActivitatsEconomiquesIFontsPropies),
                    new XElement("Total", usage.Total),
                    new XElement("ConsumPerCapita", usage.ConsumDomesticPerCapita.ToString().Replace(',', '.'))
                );

                doc.Root?.Add(newUsage);
                doc.Save(WaterUsageXmlPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving water consumption: {ex.Message}");
            }
        }

        public List<WaterConsum> GetTop10MunicipisWithHighestConsum()
        {
            var usages = LoadUsages();

            if (!usages.Any())
            {
                Debug.WriteLine("No data was loaded from CSV or XML.");
                return new List<WaterConsum>();
            }

            int lastYear = usages.Max(c => c.Any);

            return usages
                .Where(c => c.Any == lastYear)
                .OrderByDescending(c => c.Total)
                .Take(10)
                .ToList();
        }

        public List<dynamic> GetAverageUsageByComarca()
        {
            var usages = LoadUsages();

            return usages
                .GroupBy(c => c.Comarca)
                .Select(g => new
                {
                    Comarca = g.Key,
                    AverageConsum = g.Average(c => c.Total)
                })
                .OrderByDescending(c => c.AverageConsum)
                .ToList<dynamic>();
        }

        public List<WaterConsum> GetSuspiciousUsageValues()
        {
            var usages = LoadUsages();

            return usages
                .Where(c => c.Total >= 1000000)
                .ToList();
        }

        public List<dynamic> GetMunicipisWithIncreasingUsageLast5Years()
        {
            var usages = LoadUsages();

            if (!usages.Any())
            {
                Debug.WriteLine("No data was loaded from CSV or XML.");
                return new List<dynamic>();
            }

            int lastYear = usages.Max(c => c.Any);
            int firstYear = lastYear - 4;

            var result = new List<dynamic>();

            var municipisByComarca = usages
                .Where(c => c.Any >= firstYear && c.Any <= lastYear)
                .GroupBy(c => c.Comarca);

            foreach (var comarca in municipisByComarca)
            {
                var yearlyData = comarca
                    .OrderBy(c => c.Any)
                    .ToList();

                if (yearlyData.Count == 5)
                {
                    bool isIncreasing = true;
                    for (int i = 1; i < yearlyData.Count; i++)
                    {
                        if (yearlyData[i].Total <= yearlyData[i - 1].Total)
                        {
                            isIncreasing = false;
                            break;
                        }
                    }

                    if (isIncreasing)
                    {
                        result.Add(new
                        {
                            Comarca = comarca.Key,
                            ConsumsPerYear = yearlyData.Select(c => new { c.Any, c.Total }).ToList()
                        });
                    }
                }
            }

            return result;
        }
    }
}
