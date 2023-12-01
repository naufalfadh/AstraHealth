using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AstraHealth.Models;
using System.Reflection;

namespace AstraHealth.Controllers
{
    public class RujukanController : Controller
    {
        private readonly Rujukan _rujukanRepository;

        public RujukanController(IConfiguration configuration)
        {
            _rujukanRepository = new Rujukan(configuration);
        }

        public IActionResult Index()
        {
            RujukanModel rujukanModel = new RujukanModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                rujukanModel = JsonConvert.DeserializeObject<RujukanModel>(serializedModel);
            }

            /*if (rujukanModel.akn_role == "admin")
            {
                return RedirectToAction("Index", "Rujukan");
            }*/
          
            return View(_rujukanRepository.getAllData());
        }


        [HttpGet]
        public IActionResult Create()
        {
            RujukanModel rujukanModel = new RujukanModel();

            string serializedModel = HttpContext.Session.GetString("Identity");

            if (serializedModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                rujukanModel = JsonConvert.DeserializeObject<RujukanModel>(serializedModel);
            }
            /*if (rujukanModel.akn_role == "admin")
            {
                return RedirectToAction("Index", "Rujukan");
            }*/
          return View();
        }

      
    }
}
