using AesEncryption.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AesEncryption.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index( SecurityEntity securityEntity)
        {
            return View(securityEntity);
        }
        public IActionResult Generatekey()
        {
            SecurityEntity detailsEntity = new SecurityEntity();
            detailsEntity.AESKey = InfoSec.GenerateKey();

            return View("Index", detailsEntity);
        }
        public IActionResult Encrypt(SecurityEntity details)
        {
            string IVKey = "";
            details.CipherText = InfoSec.Encrypt(details.PlainText, details.AESKey, out IVKey);
            details.AESIVKey = IVKey;

            return RedirectToAction("Index", details);
        }
        public IActionResult Decrypt(SecurityEntity details)
        {
            string PlainText = "";
            details.CipherToPlainText = InfoSec.Decrypt(details.CipherText, details.AESKey, details.AESIVKey);
            

            return RedirectToAction("Index", details);
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
    }
}
