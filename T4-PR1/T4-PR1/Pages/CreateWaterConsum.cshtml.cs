using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T4_PR1.Model;

namespace T4_PR1.Pages
{
    public class AddWaterUsageModel : PageModel
    {
        private readonly WaterConsumManager _manager;

        public AddWaterUsageModel()
        {
            _manager = new WaterConsumManager();
            UsageInputModel = new WaterUsageInputModel();
        }

        [BindProperty]
        public WaterUsageInputModel UsageInputModel { get; set; }

        public class WaterUsageInputModel
        {
            [Required(ErrorMessage = "L'any �s obligatori")]
            [Range(1900, 3000, ErrorMessage = "L'any ha de ser v�lid")]
            public int Any { get; set; } = 2024;

            [Required(ErrorMessage = "El codi de comarca �s obligatori")]
            [Range(1, int.MaxValue, ErrorMessage = "El codi de comarca ha de ser un n�mero positiu")]
            public int CodiComarca { get; set; }

            [Required(ErrorMessage = "El nom de la comarca �s obligatori")]
            public string Comarca { get; set; } = string.Empty;

            [Required(ErrorMessage = "La poblaci� �s obligat�ria")]
            [Range(1, int.MaxValue, ErrorMessage = "La poblaci� ha de ser un n�mero positiu")]
            public int Poblacio { get; set; }

            [Required(ErrorMessage = "El consum dom�stic de xarxa �s obligatori")]
            [Range(0, int.MaxValue, ErrorMessage = "El consum dom�stic ha de ser un n�mero positiu")]
            public int DomesticXarxa { get; set; }

            [Required(ErrorMessage = "El consum d'activitats econ�miques �s obligatori")]
            [Range(0, int.MaxValue, ErrorMessage = "El consum d'activitats econ�miques ha de ser un n�mero positiu")]
            public int ActivitatsEconomiquesIFontsPropies { get; set; }

            [Required(ErrorMessage = "El consum total �s obligatori")]
            [Range(0, int.MaxValue, ErrorMessage = "El consum total ha de ser un n�mero positiu")]
            public int Total { get; set; }

            [Required(ErrorMessage = "El consum dom�stic per c�pita �s obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El consum dom�stic per c�pita ha de ser un n�mero positiu")]
            public double ConsumDomesticPerCapita { get; set; }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usage = new WaterConsum
            {
                Any = UsageInputModel.Any,
                CodiComarca = UsageInputModel.CodiComarca,
                Comarca = UsageInputModel.Comarca,
                Poblacio = UsageInputModel.Poblacio,
                DomesticXarxa = UsageInputModel.DomesticXarxa,
                ActivitatsEconomiquesIFontsPropies = UsageInputModel.ActivitatsEconomiquesIFontsPropies,
                Total = UsageInputModel.Total,
                ConsumDomesticPerCapita = UsageInputModel.ConsumDomesticPerCapita
            };

            _manager.SaveUsage(usage);

            return RedirectToPage("./WaterUsages");
        }
    }
}
