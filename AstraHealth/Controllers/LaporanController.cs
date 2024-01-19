using AstraHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelectPdf;

namespace AstraHealth.Controllers
{
    public class LaporanController : Controller
    {
        private readonly Laporan _laporanRepository;

        public LaporanController(IConfiguration configuration)
        {
            _laporanRepository = new Laporan(configuration);
        }

        public IActionResult LaporanDiagnosaSakit()
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

            return View(_laporanRepository.laporanDiagnosaSakit(null, null));
        }
        
        public IActionResult LaporanProdiDanDepartemen()
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

            return View(_laporanRepository.laporanProdiDanDepartemen(null, null));
        }

        public IActionResult LaporanPemakaianObat()
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

            return View(_laporanRepository.laporanPemakaianObat(null, null));
        }

        public IActionResult LaporanKecelakaanKerjaDanRujukan()
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

            return View(_laporanRepository.laporanKecelakaanKerjaDanRujukan(null, null));
        }

        [HttpPost]
        public IActionResult UpdateDiagnosaSakitData([FromBody] DateRangeModel dateRange)
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

            string dari = dateRange.Dari.ToString("yyyy-MM-dd");
            string sampai = dateRange.Sampai.ToString("yyyy-MM-dd");

            var data = _laporanRepository.laporanDiagnosaSakit(dari, sampai);

            return Json(data);
        }

        [HttpPost]
        public IActionResult UpdateProdiDepartemenData([FromBody] DateRangeModel dateRange)
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

            string dari = dateRange.Dari.ToString("yyyy-MM-dd");
            string sampai = dateRange.Sampai.ToString("yyyy-MM-dd");

            var data = _laporanRepository.laporanProdiDanDepartemen(dari, sampai);

            return Json(data);
        }

        [HttpPost]
        public IActionResult UpdatePemakaianObatData([FromBody] DateRangeModel dateRange)
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

            string dari = dateRange.Dari.ToString("yyyy-MM-dd");
            string sampai = dateRange.Sampai.ToString("yyyy-MM-dd");

            var data = _laporanRepository.laporanPemakaianObat(dari, sampai);

            return Json(data);
        }

        [HttpPost]
        public IActionResult UpdateRujukanKecelakaanKerjaData([FromBody] DateRangeModel dateRange)
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

            string dari = dateRange.Dari.ToString("yyyy-MM-dd");
            string sampai = dateRange.Sampai.ToString("yyyy-MM-dd");

            var data = _laporanRepository.laporanKecelakaanKerjaDanRujukan(dari, sampai);

            return Json(data);
        }
    }
}
