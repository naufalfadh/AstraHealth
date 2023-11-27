using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AstraHealth.Models;
using System.Diagnostics;

namespace AstraHealth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly User _userRepository;

        public HomeController(IConfiguration configuration)
        {
            _userRepository = new User(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

            [HttpPost]
            public IActionResult Login(string username, string password)
            {
                UserModel userModel = _userRepository.getDataByUsername_Password(username, password);
                if (userModel != null)
                {
                    string serializedModel = JsonConvert.SerializeObject(userModel);

                    HttpContext.Session.SetString("Identity", serializedModel);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
    }
}