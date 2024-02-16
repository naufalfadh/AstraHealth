using AstraHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AstraHealth.Controllers
{
    public class AkunController : Controller
    {
        private readonly Akun _akunRepository;

        public AkunController(IConfiguration configuration)
        {
            _akunRepository = new Akun(configuration);
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

            return View(_akunRepository.getAllData());
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
        public IActionResult Create(AkunModel akunModel)
        {
            if (ModelState.IsValid)
            {
                AkunModel existAkun = _akunRepository.getData(akunModel.akn_id);
                if (existAkun != null)
                {
                    TempData["ErrorMessage"] = "Data dengan NPK tersebut sudah ada";
                    return RedirectToAction("Create");
                }
                else
                {
                    akunModel.akn_password = _akunRepository.HashPassword(akunModel.akn_password);
                    _akunRepository.insertData(akunModel);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            }
            return View(akunModel);
        }

        [HttpGet]
        public IActionResult Edit(string id)
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
                return RedirectToAction("Index", "Dashboard");
            }

            AkunModel AkunModel = _akunRepository.getData(id);
            if (AkunModel == null)
            {
                return NotFound();
            }
            return View(AkunModel);
        }

        [HttpPost]
        public IActionResult Edit(AkunModel akunModel)
        {

            if (ModelState.IsValid)
            {
                AkunModel newAkunModel = _akunRepository.getData(akunModel.akn_id);
                if (newAkunModel == null)
                {
                    return NotFound();
                }

                newAkunModel.akn_id = akunModel.akn_id;
                newAkunModel.akn_nama = akunModel.akn_nama;
                newAkunModel.akn_password = _akunRepository.HashPassword(akunModel.akn_password);
                newAkunModel.akn_role = akunModel.akn_role;
                newAkunModel.akn_status = akunModel.akn_status;

                _akunRepository.updateData(newAkunModel);
                TempData["SuccessMessage"] = "Data berhasil diedit.";
                return RedirectToAction("Index");
            }
            return View(akunModel);
        }

        [HttpPost]
        public IActionResult Delete(string id)
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
                return RedirectToAction("Index", "Dashboard");
            }


            var response = new { success = false, message = "Gagal menghapus data." };
            try
            {
                if (id != null)
                {
                    _akunRepository.deleteData(id);
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

        [HttpPost]
        public IActionResult Activate(string id)
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
                return RedirectToAction("Index", "Dashboard");
            }


            var response = new { success = false, message = "Gagal mengaktifkan data." };
            try
            {
                if (id != null)
                {
                    _akunRepository.activateData(id);
                    response = new { success = true, message = "Akun berhasil diaktifkan." };
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

        [HttpGet]
        public IActionResult EditPassword(string id)
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

            AkunModel AkunModel = _akunRepository.getData(id);
            if (AkunModel == null)
            {
                return NotFound();
            }
            return View(AkunModel);
        }

        [HttpPost]
        public IActionResult EditPassword(AkunModel akunModel)
        {
            if (ModelState.IsValid)
            {
                AkunModel newAkunModel = _akunRepository.getData(akunModel.akn_id);
                if (newAkunModel == null)
                {
                    return NotFound();
                }

                if (_akunRepository.HashPassword(akunModel.akn_password) != newAkunModel.akn_password)
                {
                    TempData["ErrorMessage"] = "Kata sandi lama tidak valid.";
                    return View(akunModel);
                }

                newAkunModel.akn_id = akunModel.akn_id;
                newAkunModel.akn_nama = akunModel.akn_nama;
                newAkunModel.akn_password = _akunRepository.HashPassword(akunModel.akn_password_baru);
                newAkunModel.akn_role = akunModel.akn_role;
                newAkunModel.akn_status = akunModel.akn_status;

                _akunRepository.updateData(newAkunModel);
                TempData["SuccessMessage"] = "Kata sandi berhasil diganti.";
                return RedirectToAction("Index", "Dashboard");
            }
            return View(akunModel);
        }
    }
}
