using AstraHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Globalization;
using System.IO;

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

        [HttpGet]
        public IActionResult Create()
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

            return View();
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
            if (akunModel.akn_role == "manajer")
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
                ObatModel newObatModel = _obatRepository.getData(obatModel.obt_id);
                if (newObatModel == null)
                {
                    return NotFound();
                }

                newObatModel.obt_id = obatModel.obt_id;
                newObatModel.obt_nama_obat = obatModel.obt_nama_obat;
                newObatModel.obt_stok = obatModel.obt_stok;
                newObatModel.obt_tanggal_kadaluarsa = obatModel.obt_tanggal_kadaluarsa;
                newObatModel.obt_satuan = obatModel.obt_satuan;

                _obatRepository.updateData(newObatModel);
                TempData["SuccessMessage"] = "Data berhasil diedit.";
                return RedirectToAction("Index");
            }
            return View(obatModel);
        }

        [HttpGet]
        public IActionResult Excel()
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
        public IActionResult Excel(IFormFile file)
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

            if (file != null && file.Length > 0)
            {
                var fileName = file.FileName;
                var tempPath = Path.Combine(Path.GetTempPath(), fileName);
                using (var stream = file.OpenReadStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            ObatModel obatModel = new ObatModel();
                            // Ambil data dari setiap kolom
                            obatModel.obt_nama_obat = worksheet.Cells[row, 2].Text;
                            obatModel.obt_stok = Convert.ToInt32(worksheet.Cells[row, 3].Text);
                            obatModel.obt_satuan = worksheet.Cells[row, 4].Text;
                            // Konversi dan tambahkan jam, menit, dan detik
                            string tanggalText = worksheet.Cells[row, 5].Text;

                            if (DateTime.TryParseExact(tanggalText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime tanggalKadaluarsa))
                            {
                                obatModel.obt_tanggal_kadaluarsa = tanggalKadaluarsa.Date; // Hapus informasi waktu
                            }
                            else
                            {
                                // Tangani kesalahan konversi
                                Console.WriteLine($"Error converting date for row {row}: {tanggalText}");
                                continue; // Lewati iterasi ini dan lanjutkan ke baris berikutnya
                            }

                            obatModel.obt_status = "aktif";

                            ObatModel existObat = _obatRepository.getDataByNama(obatModel.obt_nama_obat);

                            if (existObat == null)
                            {
                                _obatRepository.insertData(obatModel);
                            }
                            else
                            {
                                _obatRepository.updateDataByNama(obatModel);
                            }
                            // Lakukan operasi penyimpanan data ke database atau yang lainnya
                            // (sesuai kebutuhan Anda)
                        }
                    }
                }
                TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                return RedirectToAction("Index");
            }

            // Tambahkan pernyataan pengembalian di bawah ini
            return RedirectToAction("Index");
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
                    _obatRepository.deleteData(id);
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
