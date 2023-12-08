using AstraHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelectPdf;

namespace AstraHealth.Controllers
{
    public class LaporanController : Controller
    {
        private readonly Pasien _pasienRepository;

        public LaporanController(IConfiguration configuration)
        {
            _pasienRepository = new Pasien(configuration);
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

            return View(_pasienRepository.laporanDiagnosaSakit());
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

            return View(_pasienRepository.laporanProdiDanDepartemen());
        }

        public IActionResult GeneratePdf(string html)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument oPdfDocument = converter.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();

            return File(
                pdf,
                "application/pdf",
                "Laporan.pdf"
                );

        }
    }
}
