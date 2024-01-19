using AstraHealth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AstraHealth.Controllers
{
    public class DashboardController : Controller
    {
        private readonly Laporan _laporanRepository;

        public DashboardController(IConfiguration configuration)
        {
            _laporanRepository = new Laporan(configuration);
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

            // Get the current date
            DateTime currentDate = DateTime.Now;

            // Set tanggalPertama to the first day of the current month
            DateTime tanggalPertama = new DateTime(currentDate.Year, currentDate.Month, 1);

            // Set tanggalTerakhir to the last day of the current month
            DateTime tanggalTerakhir = tanggalPertama.AddMonths(1).AddDays(-1);

            // Convert to string in the "yyyy-MM-dd" format
            string dari = tanggalPertama.ToString("yyyy-MM-dd");
            string sampai = tanggalTerakhir.ToString("yyyy-MM-dd");

            return View(_laporanRepository.getLaporan(dari, sampai));
        }

        [HttpPost]
        public IActionResult UpdateDiagnosaDiagram([FromBody] DateRangeModel dateRange)
        {
            try
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

                var data = _laporanRepository.getDistinctDiagnosa(dari, sampai);

                return Json(data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest("An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public IActionResult UpdateProdiDepartemenDiagram([FromBody] DateRangeModel dateRange)
        {
            try
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

                var data = _laporanRepository.getDistinctProdiDanDepartemen(dari, sampai);

                return Json(data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest("An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public IActionResult UpdatePemakaianObatDiagram([FromBody] DateRangeModel dateRange)
        {
            try
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

                var data = _laporanRepository.getDistinctPemakaianObat(dari, sampai);

                return Json(data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest("An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public IActionResult UpdateKecelakaanRujukanDiagram([FromBody] DateRangeModel dateRange)
        {
            try
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

                var data = _laporanRepository.getKecelakaanKerjaDanRujukan(dari, sampai);

                return Json(data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest("An error occurred while processing the request.");
            }
        }
    }
}
