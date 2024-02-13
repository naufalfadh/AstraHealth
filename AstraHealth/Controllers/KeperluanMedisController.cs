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
        public IActionResult Create(KeperluanAlatMedis keperluanAlatMedis)
        {
            if (ModelState.IsValid)
            {
                _medisRepository.insertData(keperluanAlatMedis);
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                return RedirectToAction("Index");
            }
            return View(keperluanAlatMedis);
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
                TempData["SuccessMessage"] = "Catatan berhasil ditambahkan.";
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


            var response = new { success = false, message = "Gagal menerima pengajuan." };
            try
            {
                if (id != null)
                {
                    string id_manajer = HttpContext.Session.GetString("Id");
                    _medisRepository.acceptData(id, id_manajer);
                    response = new { success = true, message = "Berhasil menerima pengajuan." };
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


            var response = new { success = false, message = "Gagal menolak pengajuan." };
            try
            {
                if (id != null)
                {
                    string id_manajer = HttpContext.Session.GetString("Id");
                    _medisRepository.rejectData(id, id_manajer);
                    response = new { success = true, message = "Berhasil menolak pengajuan." };
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
        public IActionResult Receive(int id)
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


            var response = new { success = false, message = "Gagal menkonfirmasi barang." };
            try
            {
                if (id != null)
                {
                    _medisRepository.recieveData(id);
                    response = new { success = true, message = "Barang sudah diterima." };
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
