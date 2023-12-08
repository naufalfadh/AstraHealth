using AstraHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AstraHealth.Controllers
{
    public class KeperluanMedisController : Controller
    {
        private readonly KeperluanMedis _medisRepository;

        public KeperluanMedisController(IConfiguration configuration)
        {
            _medisRepository = new KeperluanMedis(configuration);
        }

        public IActionResult Index()
        {
            KeperluanMedisModel keperluanMedisModel = new KeperluanMedisModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                keperluanMedisModel = JsonConvert.DeserializeObject<KeperluanMedisModel>(serializedModel);
            }

            return View(_medisRepository.getAllData());
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
        public IActionResult Create(KeperluanMedisModel keperluanMedisModel)
        {

            if (ModelState.IsValid)
            {
                _medisRepository.insertData(keperluanMedisModel);
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                return RedirectToAction("Index");
            }
            return View(keperluanMedisModel);
        }

        [HttpGet]
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
                return RedirectToAction("Index", "Dashboard");
            }

            KeperluanMedisModel KeperluanMedisModel = _medisRepository.getData(id);
            if (KeperluanMedisModel == null)
            {
                return NotFound();
            }
            return View(KeperluanMedisModel);
        }

        [HttpPost]
        public IActionResult Edit(KeperluanMedisModel keperluanMedisModel)
        {

            if (ModelState.IsValid)
            {
                KeperluanMedisModel newKeperluanMedisModel = _medisRepository.getData(keperluanMedisModel.kpm_id);
                if (newKeperluanMedisModel == null)
                {
                    return NotFound();
                }

                newKeperluanMedisModel.kpm_id = keperluanMedisModel.kpm_id;
                newKeperluanMedisModel.kpm_catatan = keperluanMedisModel.kpm_catatan;

                _medisRepository.updateData(newKeperluanMedisModel);
                TempData["SuccessMessage"] = "Keperluan KeperluanMedis berhasil diupdate.";
                return RedirectToAction("Index");
            }
            return View(keperluanMedisModel);
        }

        [HttpPost]
        public IActionResult Accept(int id)
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


            var response = new { success = false, message = "Gagal menghapus pasien Model." };
            try
            {
                if (id != null)
                {
                    _medisRepository.acceptData(id);
                    response = new { success = true, message = "Berhasil menerima data." };
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
        public IActionResult Reject(int id)
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


            var response = new { success = false, message = "Gagal menghapus pasien Model." };
            try
            {
                if (id != null)
                {
                    _medisRepository.rejectData(id);
                    response = new { success = true, message = "Berhasil menolak data." };
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
