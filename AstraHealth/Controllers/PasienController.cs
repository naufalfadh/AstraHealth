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
            /*if (akunModel.akn_role == "admin")
            {
                return RedirectToAction("Index", "Pasien");
            }*/
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

        /*[HttpPost]
        public IActionResult Delete(int id)
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
            if (akunModel.akn_role == "admin")
            {
                return RedirectToAction("Index", "Pasien");
            }
          

            var response = new { success = false, message = "Gagal menghapus pasienModel." };
            try
            {
                if (id != null)
                {
                    _pasienRepository.deleteData(id);
                    response = new { success = true, message = "Berhasil menghapus data." };
                }
                else
                {
                    response = new { success = false, message = "PasienModel tidak ditemukan" };
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = ex.Message };
            }
            return Json(response);
        }*/

        /*[HttpGet]
        public IActionResult Edit(int id)
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
             if (akunModel.akn_role == "admin")
            {
                return RedirectToAction("Index", "Pasien");
            }
           
            PasienModel PasienModel = _pasienRepository.getData(id);
            if (PasienModel == null)
            {
                return NotFound();
            }
            return View(PasienModel);
        }*/

        /*[HttpPost]
        public IActionResult Edit(PasienModel pasienModel)
        {

            if (ModelState.IsValid)
            {
                PasienModel newPasienModel = _pasienRepository.getData(pasienModel.rgs_id);
                if (newPasienModel == null)
                {
                    return NotFound();
                }

                newPasienModel.rgs_id_pasien = pasienModel.rgs_id_pasien;
                newPasienModel.rgs_nama_pasien = pasienModel.rgs_nama_pasien;
                newPasienModel.rgs_prodi_atau_departemen = pasienModel.rgs_prodi_atau_departemen;
                newPasienModel.rgs_keluhan = pasienModel.rgs_keluhan;
                newPasienModel.rgs_tensi = pasienModel.rgs_tensi;
                newPasienModel.rgs_diagnosa = pasienModel.rgs_diagnosa;
                newPasienModel.rgs_obat = pasienModel.rgs_obat;
                newPasienModel.rgs_jumlah_obat = pasienModel.rgs_jumlah_obat;
                newPasienModel.rgs_kecelakaan_kerja = pasienModel.rgs_kecelakaan_kerja;
                newPasienModel.rgs_keterangan = pasienModel.rgs_keterangan;
                newPasienModel.rgs_tanggal = pasienModel.rgs_tanggal;
                newPasienModel.rgs_id_admin = pasienModel.rgs_id_admin;


                _pasienRepository.updateData(newPasienModel);
                TempData["SuccessMessage"] = "PasienModel berhasil diupdate.";
                return RedirectToAction("Index");
            }
            return View(pasienModel);
        }*/
    }
}
