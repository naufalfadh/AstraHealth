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
        public IActionResult Create()
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
            /*if (rujukanModel.akn_role == "admin")
            {
                return RedirectToAction("Index", "Pasien");
            }*/
            return View();
        }

        [HttpPost]
        public IActionResult Create(RujukanModel rujukanModel)
        {

            if (ModelState.IsValid)
            {
                _rujukanRepository.insertData(rujukanModel);
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                return RedirectToAction("Index");
            }
            return View(rujukanModel);
        }

    }
}
