using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AstraHealth.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;

namespace AstraHealth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Akun _akunRepository;

        public HomeController(IConfiguration configuration)
        {
            _akunRepository = new Akun(configuration);
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
        public IActionResult Login(string akn_id, string akn_password)
        {
            // Pastikan bahwa username dan password valid
            if (string.IsNullOrEmpty(akn_id) || string.IsNullOrEmpty(akn_password))
            {
                // Jika username atau password tidak valid, kembali ke halaman login atau tampilkan pesan kesalahan
                TempData["ErrorMessage"] = "NPK dan kata sandi harus diisi.";
                return RedirectToAction("Index", "Home"); // Ganti "Account" dengan nama controller akun Anda
            }

            akn_password = _akunRepository.HashPassword(akn_password);

            // Dapatkan data anggota berdasarkan username dan password
            AkunModel akunModel = _akunRepository.getDataByUsernamePassword(akn_id, akn_password);

            if (akunModel == null)
            {
                // Jika data anggota tidak ditemukan, berarti kredensial salah
                TempData["ErrorMessage"] = "NPK atau kata sandi salah.";
                return RedirectToAction("Index", "Home"); // Ganti "Account" dengan nama controller akun Anda
            }
            else if (akunModel.akn_status == "tidak aktif")
            {
                TempData["ErrorMessage"] = "Akun tidak aktif, hubungi Manajer UKK untuk mengaktifkan akun kembali.";
                return RedirectToAction("Index", "Home"); // Ganti "Account" dengan nama controller akun Anda
            }

            // Jika berhasil, atur sesi
            string serializedModel = JsonConvert.SerializeObject(akunModel);
            HttpContext.Session.SetString("Identity", serializedModel);
            HttpContext.Session.SetString("Id", akunModel.akn_id);
            HttpContext.Session.SetString("Nama", akunModel.akn_nama);
            HttpContext.Session.SetString("Role", akunModel.akn_role); // Simpan peran dalam sesi

            // Arahkan ke halaman Dashboard
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            // Hapus token otentikasi di sisi klien
            HttpContext.SignOutAsync();

            // Redirect pengguna ke halaman login
            return RedirectToAction("Index", "Home");
        }
    }
}