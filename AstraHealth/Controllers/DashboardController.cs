using AstraHealth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AstraHealth.Controllers
{
    public class DashboardController : Controller
    {
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

            return View();
        }
    }
}
