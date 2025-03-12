using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T4_PR1.Model;

namespace T4_PR1.Pages
{
    public class WaterUsagesModel : PageModel
    {
        private readonly WaterConsumManager _manager;

        public WaterUsagesModel()
        {
            _manager = new WaterConsumManager();
            Usages = new List<WaterConsum>();
            Top10Municipis = new List<WaterConsum>();
            AverageConsumByComarca = new List<dynamic>();
            SuspiciousValues = new List<WaterConsum>();
            MunicipisWithIncreasingTrend = new List<dynamic>();
        }

        public List<WaterConsum> Usages { get; set; }
        public List<WaterConsum> Top10Municipis { get; set; }
        public List<dynamic> AverageConsumByComarca { get; set; }
        public List<WaterConsum> SuspiciousValues { get; set; }
        public List<dynamic> MunicipisWithIncreasingTrend { get; set; }

        public void OnGet()
        {
            Usages = _manager.LoadUsages();
            Top10Municipis = _manager.GetTop10MunicipisWithHighestConsum();
            AverageConsumByComarca = _manager.GetAverageUsageByComarca();
            SuspiciousValues = _manager.GetSuspiciousUsageValues();
            MunicipisWithIncreasingTrend = _manager.GetMunicipisWithIncreasingUsageLast5Years();
        }
    }
}
