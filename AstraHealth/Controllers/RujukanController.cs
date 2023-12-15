using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AstraHealth.Models;
using System.Reflection;

namespace AstraHealth.Controllers
{
    public class RujukanController : Controller
    {
        private readonly Rujukan _rujukanRepository;

        public RujukanController(IConfiguration configuration)
        {
            _rujukanRepository = new Rujukan(configuration);
        }

        public IActionResult Index()
        {
            AkunModel rujukanModel = new AkunModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                rujukanModel = JsonConvert.DeserializeObject<AkunModel>(serializedModel);
            }

            return View(_rujukanRepository.getAllData());
        }

        [HttpGet]
        public IActionResult Create(string id_anamnesa)
        {
            AkunModel akunnModel = new AkunModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                akunnModel = JsonConvert.DeserializeObject<AkunModel>(serializedModel);
            }

            RujukanModel rujukanModel = new RujukanModel();
            rujukanModel.rjk_id_anamnesa = id_anamnesa;

            return View(rujukanModel);
        }

        [HttpPost]
        public IActionResult Create(RujukanModel rujukanModel)
        {
            if (ModelState.IsValid)
            {
                _rujukanRepository.insertData(rujukanModel);
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                return RedirectToAction("Index", "Pasien");
            }
            return View(rujukanModel);
        }

    }
}
