using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AstraHealth.Models;
using System.Reflection;

namespace AstraHealth.Controllers
{
    public class PasienController : Controller
    {
        private readonly Pasien _pasienRepository;

        public PasienController(IConfiguration configuration)
        {
            _pasienRepository = new Pasien(configuration);
        }

        public IActionResult Index()
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
          
            return View(_pasienRepository.getAllData());
        }

        [HttpGet]
        public IActionResult Create()
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
          return View();
        }

        [HttpPost]
        public IActionResult Create(PasienModel pasienModel, string submitButton)
        {
            if (ModelState.IsValid)
            {
                if (bool.TryParse(Request.Form["anm_kecelakaan_kerja_checkbox"], out bool kecelakaanKerjaCheckboxValue))
                {
                    pasienModel.anm_kecelakaan_kerja = kecelakaanKerjaCheckboxValue ? 1 : 0;
                }
                pasienModel.anm_id = _pasienRepository.getAnamnesaId();

                if (pasienModel.PemakaianObats != null && pasienModel.PemakaianObats.Count > 0)
                {
                    for (int i = 0; i < pasienModel.PemakaianObats.Count; i++)
                    {
                        int id = _pasienRepository.getPemakaianObatId() + i;
                        pasienModel.PemakaianObats[i].pmo_id = id.ToString();
                        pasienModel.PemakaianObats[i].pmo_id_anamnesa = pasienModel.anm_id;
                    }
                }

                _pasienRepository.insertData(pasienModel);
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";

                string id_anamnesa = pasienModel.anm_id;

                if (submitButton == "rujukan")
                {
                    return RedirectToAction("Create", "Rujukan", new { id_anamnesa });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View(pasienModel);
        }

    }
}
