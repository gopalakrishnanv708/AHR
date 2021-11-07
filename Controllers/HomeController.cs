using AHR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AHR.DBContext;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AHR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AdminDBContext AdminDBContext;

        public HomeController(ILogger<HomeController> logger)
        {
            _httpContextAccessor = new HttpContextAccessor();
            _logger = logger;
            AdminDBContext = new AdminDBContext(_httpContextAccessor);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Member()
        {
            return View("Member");
        }

        [Route("/Home/ContactUs")]
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("/Home/ContactUs")]
        [HttpPost]
        public IActionResult PostContactUs(ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                if (AdminDBContext.UpdateInfo(contactUs))
                {
                    ModelState.Clear();
                    TempData["Success"] = "Information updated succesfully!";
                    return View("ContactUs");
                }
                else
                {
                    TempData["Error"] = "Error Occured. Please try again.";
                    return View("ContactUs");
                }
            }
            else
            {
                return View("ContactUs");
            }
            
        }

        [Authorize(Roles = "A")]
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
