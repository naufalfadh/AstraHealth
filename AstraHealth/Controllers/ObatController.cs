using AstraHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AstraHealth.Controllers
{
    public class ObatController : Controller
    {
        private readonly Obat _obatRepository;

        public ObatController(IConfiguration configuration)
        {
            _obatRepository = new Obat(configuration);
        }

        public IActionResult Index()
        {
            ObatModel obatModel = new ObatModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                obatModel = JsonConvert.DeserializeObject<ObatModel>(serializedModel);
            }

            return View(_obatRepository.getAllData());
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
            return View(_obatRepository.getAllData());
        }

        [HttpPost]
        public IActionResult Create(ObatModel obatModel)
        {

            if (ModelState.IsValid)
            {
                _obatRepository.insertData(obatModel);
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                return RedirectToAction("Index");
            }
            return View(obatModel);
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

            ObatModel ObatModel = _obatRepository.getData(id);
            if (ObatModel == null)
            {
                return NotFound();
            }
            return View(ObatModel);
        }

        [HttpPost]
        public IActionResult Edit(ObatModel obatModel)
        {

            if (ModelState.IsValid)
            {
                ObatModel newObatModel = _obatRepository.getData(obatModel.kpo_id);
                if (newObatModel == null)
                {
                    return NotFound();
                }

                newObatModel.kpo_id = obatModel.kpo_id;
                newObatModel.kpo_catatan = obatModel.kpo_catatan;

                _obatRepository.updateData(newObatModel);
                TempData["SuccessMessage"] = "Keperluan Obat berhasil diupdate.";
                return RedirectToAction("Index");
            }
            return View(obatModel);
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
                    _obatRepository.acceptData(id);
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
                    _obatRepository.rejectData(id);
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
