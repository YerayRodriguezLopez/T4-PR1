﻿using System.Text.Json;

namespace T4_PR1.Model
{
    public class EnergeticIndicatorManager
    {
        private const string IndicadorsFilePath = "ModelData/indicadors_energetics_cat.csv";
        private const string IndicadorsJsonPath = "ModelData/indicadors_energetics_cat.json";

        public EnergeticIndicatorManager()
        {
            FileExistsOrCreateJson();
        }

        private void FileExistsOrCreateJson()
        {
            string? directory = Path.GetDirectoryName(IndicadorsJsonPath);
            if (!Directory.Exists(directory))
            {
                if (directory != null) Directory.CreateDirectory(directory);
            }

            if (!File.Exists(IndicadorsJsonPath))
            {
                File.WriteAllText(IndicadorsJsonPath, "[]");
            }
        }

        public List<EnergeticIndicator> LoadIndicators()
        {
            var indicators = new List<EnergeticIndicator>();

            try
            {
                if (File.Exists(IndicadorsFilePath))
                {
                    var lines = File.ReadAllLines(IndicadorsFilePath).Skip(1);

                    foreach (string line in lines)
                    {
                        var parts = line.Split(',');
                        if (parts.Length < 40) continue;

                        try
                        {
                            var indicator = new EnergeticIndicator
                            {
                                Data = parts[0],
                                PBEE_Hidroelectr = ParseDouble(parts[1]),
                                PBEE_Carbo = ParseDouble(parts[2]),
                                PBEE_GasNat = ParseDouble(parts[3]),
                                PBEE_FuelOil = ParseDouble(parts[4]),
                                PBEE_CiclComb = ParseDouble(parts[5]),
                                PBEE_Nuclear = ParseDouble(parts[6]),
                                CDEEBC_ProdBruta = ParseDouble(parts[7]),
                                CDEEBC_ConsumAux = ParseDouble(parts[8]),
                                CDEEBC_ProdNeta = ParseDouble(parts[9]),
                                CDEEBC_ConsumBomb = ParseDouble(parts[10]),
                                CDEEBC_ProdDisp = ParseDouble(parts[11]),
                                CDEEBC_TotVendesXarxaCentral = ParseDouble(parts[12]),
                                CDEEBC_SaldoIntercanviElectr = ParseDouble(parts[13]),
                                CDEEBC_DemandaElectr = ParseDouble(parts[14]),
                                CDEEBC_TotalEBCMercatRegulat = parts[15],
                                CDEEBC_TotalEBCMercatLliure = parts[16],
                                FEE_Industria = ParseDouble(parts[17]),
                                FEE_Terciari = ParseDouble(parts[18]),
                                FEE_Domestic = ParseDouble(parts[19]),
                                FEE_Primari = ParseDouble(parts[20]),
                                FEE_Energetic = ParseDouble(parts[21]),
                                FEEI_ConsObrPub = ParseDouble(parts[22]),
                                FEEI_SiderFoneria = ParseDouble(parts[23]),
                                FEEI_Metalurgia = ParseDouble(parts[24]),
                                FEEI_IndusVidre = ParseDouble(parts[25]),
                                FEEI_CimentsCalGuix = ParseDouble(parts[26]),
                                FEEI_AltresMatConstr = ParseDouble(parts[27]),
                                FEEI_QuimPetroquim = ParseDouble(parts[28]),
                                FEEI_ConstrMedTrans = ParseDouble(parts[29]),
                                FEEI_RestaTransforMetal = ParseDouble(parts[30]),
                                FEEI_AlimBegudaTabac = ParseDouble(parts[31]),
                                FEEI_TextilConfecCuirCalcat = ParseDouble(parts[32]),
                                FEEI_PastaPaperCartro = ParseDouble(parts[33]),
                                FEEI_AltresIndus = ParseDouble(parts[34]),
                                DGGN_PuntFrontEnagas = ParseDouble(parts[35]),
                                DGGN_DistrAlimGNL = ParseDouble(parts[36]),
                                DGGN_ConsumGNCentrTerm = ParseDouble(parts[37]),
                                CCAC_GasolinaAuto = ParseDouble(parts[38]),
                                CCAC_GasoilA = ParseDouble(parts[39])
                            };

                            indicators.Add(indicator);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing line: {ex.Message}");
                        }
                    }
                }

                if (File.Exists(IndicadorsJsonPath))
                {
                    string jsonContent = File.ReadAllText(IndicadorsJsonPath);
                    var jsonIndicators = JsonSerializer.Deserialize<List<EnergeticIndicator>>(jsonContent);
                    if (jsonIndicators != null)
                    {
                        indicators.AddRange(jsonIndicators);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading energy indicators: {ex.Message}");
            }

            return indicators;
        }

        private double ParseDouble(string value)
        {
            int nigga = 0;
            if (double.TryParse(value.Replace('.', ','), out double result))
            {
                return result;
            }
            return nigga;
        }

        public void SaveIndicator(EnergeticIndicator indicator)
        {
            try
            {
                var existingIndicators = new List<EnergeticIndicator>();

                if (File.Exists(IndicadorsJsonPath))
                {
                    string jsonContent = File.ReadAllText(IndicadorsJsonPath);
                    var deserializedIndicators = JsonSerializer.Deserialize<List<EnergeticIndicator>>(jsonContent);
                    if (deserializedIndicators != null)
                    {
                        existingIndicators = deserializedIndicators;
                    }
                }

                existingIndicators.Add(indicator);

                string updatedJson = JsonSerializer.Serialize(existingIndicators, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(IndicadorsJsonPath, updatedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving energy indicator: {ex.Message}");
            }
        }

        public List<EnergeticIndicator> GetRecordsWithProdNetaGreaterThan3000()
        {
            var indicators = LoadIndicators();

            return indicators
                .Where(i => i.CDEEBC_ProdNeta > 3000)
                .OrderBy(i => i.CDEEBC_ProdNeta)
                .ToList();
        }

        public List<EnergeticIndicator> GetRecordsWithGasolinaGreaterThan100()
        {
            var indicators = LoadIndicators();

            return indicators
                .Where(i => i.CCAC_GasolinaAuto > 100)
                .OrderByDescending(i => i.CCAC_GasolinaAuto)
                .ToList();
        }

        public List<dynamic> GetAverageProdNetaPerYear()
        {
            var indicators = LoadIndicators();

            return indicators
                .GroupBy(i => i.GetYear())
                .Where(g => g.Key > 0)
                .Select(g => new
                {
                    Any = g.Key,
                    AverageProdNeta = g.Average(i => i.CDEEBC_ProdNeta)
                })
                .OrderBy(item => item.Any)
                .ToList<dynamic>();
        }

        public List<EnergeticIndicator> GetRecordsWithHighDemandAndLowProduction()
        {
            var indicators = LoadIndicators();

            return indicators
                .Where(i => i is { CDEEBC_DemandaElectr: > 4000, CDEEBC_ProdDisp: < 300 })
                .ToList();
        }
    }
}
