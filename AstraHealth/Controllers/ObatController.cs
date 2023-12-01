using AstraHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AstraHealth.Controllers
{
    public class ObatController : Controller
    {
        private readonly Obat _obatRepository;

        public ObatController(IConfiguration configuration)
        {
            _obatRepository = new Obat(configuration);
        }

        public IActionResult Index()
        {
            ObatModel obatModel = new ObatModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                obatModel = JsonConvert.DeserializeObject<ObatModel>(serializedModel);
            }

            return View(_obatRepository.getAllData());
        }
    }
}
