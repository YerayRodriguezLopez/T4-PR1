using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T4_PR1.Model;

namespace T4_PR1.Pages
{
    public class SimulationsModel : PageModel
    {
        private readonly SimulationManager _fileManager;

        public SimulationsModel()
        {
            _fileManager = new SimulationManager();
            Simulations = new List<Simulation>();
        }

        public List<Simulation> Simulations { get; set; }

        public void OnGet()
        {
            Simulations = _fileManager.LoadSimulations();
        }
    }
}
