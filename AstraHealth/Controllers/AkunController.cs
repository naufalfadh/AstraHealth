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
            /*if (akunModel.akn_role == "admin")
            {
                return RedirectToAction("Index", "Pasien");
            }*/
            return View();
        }

        [HttpPost]
        public IActionResult Create(AkunModel akunModel)
        {

            if (ModelState.IsValid)
            {
                _akunRepository.insertData(akunModel);
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                return RedirectToAction("Index");
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
                newAkunModel.akn_password = akunModel.akn_password;
                newAkunModel.akn_role = akunModel.akn_role;
                newAkunModel.akn_status = akunModel.akn_status;

                _akunRepository.updateData(newAkunModel);
                TempData["SuccessMessage"] = "PasienModel berhasil diupdate.";
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


            var response = new { success = false, message = "Gagal menghapus pasienModel." };
            try
            {
                if (id != null)
                {
                    _akunRepository.deleteData(id);
                    response = new { success = true, message = "Berhasil menghapus data." };
                }
                else
                {
                    response = new { success = false, message = "Akun tidak ditemukan" };
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
