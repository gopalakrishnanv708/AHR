using AHR.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AHR.Controllers
{
    public class MembersController : Controller
    {
        private MemberDBContext adminDBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MembersController(IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = new HttpContextAccessor();
            adminDBContext = new MemberDBContext(_httpContextAccessor,webHostEnvironment);
        }

        [Authorize]
        [Route("/Member/MyFinancials")]
        [HttpGet]
        public IActionResult MyFinancials()
        {
            ViewBag.GetFundsReceived = adminDBContext.ProcessFundsReceived(adminDBContext.GetFundsReceived());

            ViewBag.GetFundsSpent = adminDBContext.ProcessFundsSpent(adminDBContext.GetFundsSpent());

            ViewData["Total"] = adminDBContext.GetFundsReceivedTotal(adminDBContext.GetFundsReceived());

            bool IsReceivedEmpty = String.IsNullOrEmpty( Convert.ToString(ViewData["Total"]));

            if (IsReceivedEmpty)
            {
                ViewData["Total"] = "0.00";
            }

            ViewData["TotalSpent"] = adminDBContext.GetFundsSpentTotal(adminDBContext.GetFundsSpent());

            bool IsSpentEmpty = String.IsNullOrEmpty(Convert.ToString(ViewData["TotalSpent"]));

            if (IsSpentEmpty)
            {
                ViewData["TotalSpent"] = "0.00";
            }

            ViewData["TotalRem"] = Convert.ToDecimal(ViewData["Total"]) - Convert.ToDecimal(ViewData["TotalSpent"]);

            return View();
        }

        [Authorize]
        [Route("/Member/FamilyDetails")]
        [HttpGet]
        public IActionResult FamilyDetails()
        {
            ViewBag.FamilyDetails = adminDBContext.ProcessFamilyDetails(adminDBContext.GetFamilyDetails());
            return View();
        }

        [Authorize]
        [Route("/Member/FamilyUpdates")]
        [HttpGet]
        public IActionResult FamilyUpdates()
        {
            ViewBag.FamilyUpdates = adminDBContext.ProcessFamilyUpdates(adminDBContext.GetFamilyActivity());
            return View();
        }
    }
}
