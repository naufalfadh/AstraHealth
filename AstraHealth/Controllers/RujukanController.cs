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
            AkunModel akunModel = new AkunModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                akunModel = JsonConvert.DeserializeObject<AkunModel>(serializedModel);
            }

            RujukanModel rujukanModel = new RujukanModel();

            RujukanModel rujukanModels = new RujukanModel();
            rujukanModels = _rujukanRepository.getAnamnesaData(id_anamnesa);

            rujukanModel.rjk_id_anamnesa = id_anamnesa;
            rujukanModel.anm_id_pasien = rujukanModels.anm_id_pasien;
            rujukanModel.anm_nama_pasien = rujukanModels.anm_nama_pasien;
            rujukanModel.anm_prodi_atau_departemen = rujukanModels.anm_prodi_atau_departemen;
            rujukanModel.anm_diagnosa= rujukanModels.anm_diagnosa;

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
