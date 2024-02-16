﻿using Microsoft.AspNetCore.Mvc;
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

            PasienModel pasienModel = new PasienModel();
            pasienModel.Obats = _pasienRepository.getObatData();
            return View(pasienModel);
        }

        [HttpPost]
        public IActionResult Create(PasienModel pasienModel, string submitButton)
        {
            if (ModelState.IsValid)
            {
                pasienModel.anm_kecelakaan_kerja = Request.Form["anm_kecelakaan_kerja_checkbox"] == "on" ? 1 : 0;
                pasienModel.anm_id = _pasienRepository.getAnamnesaId();

                if (pasienModel.PemakaianObats != null && pasienModel.PemakaianObats.Count > 0)
                {
                    for (int i = 0; i < pasienModel.PemakaianObats.Count; i++)
                    {
                        ObatModel obatModel = _pasienRepository.getObatDataById(pasienModel.PemakaianObats[i].pmo_id_obat);
                        if (obatModel.obt_stok < pasienModel.PemakaianObats[i].pmo_jumlah)
                        {
                            TempData["ErrorMessage"] = "Obat " + obatModel.obt_nama_obat + " hanya memiliki stok: " + obatModel.obt_stok + " " + obatModel.obt_satuan;
                            return RedirectToAction("Create");
                        }

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



        [HttpPost]
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
            if (akunModel.akn_role == "manajer")
            {
                return RedirectToAction("Index", "Dashboard");
            }


            var response = new { success = false, message = "Gagal menghapus data." };
            try
            {
                if (id != null)
                {
                    _pasienRepository.deleteData(id);
                    response = new { success = true, message = "Data berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Data tidak ditemukan" };
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = ex.Message };
            }
            return Json(response);
        }
    }
}
