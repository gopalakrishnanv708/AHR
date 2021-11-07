using AHR.DBContext;
using Microsoft.AspNetCore.Mvc;
using AHR.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace AHR.Controllers
{
    public class AdminController : Controller
    {
        private AdminDBContext adminDBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AccountDBContext accountDBContext;
        private readonly IWebHostEnvironment hostEnvironment;
        private DataTable imgTable;
        private DataTable vidTable;
        Income income;

        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = new HttpContextAccessor();

            adminDBContext = new AdminDBContext(_httpContextAccessor);

            accountDBContext = new AccountDBContext();

            hostEnvironment = webHostEnvironment;

            imgTable = new DataTable("udtt_image");

            vidTable = new DataTable("udtt_video");

            income = new Income();
        }


        [Authorize(Roles = "A")]
        [Route("/Admin/Income")]
        [HttpGet]
        public IActionResult Income()
        {
            ViewBag.message = adminDBContext.GetPaymentMethod();

            ViewBag.email = adminDBContext.GetUserEmail();

            return View("Income");
        }

        [Authorize(Roles = "A")]
        [Route("/Admin/Income")]
        [HttpPost]
        public async Task<IActionResult> PostIncome(Income income)
        {
            ViewBag.message = adminDBContext.GetPaymentMethod();

            ViewBag.email = adminDBContext.GetUserEmail();

            if (ModelState.IsValid)
            {
                if (accountDBContext.ValidateUser(income.DonorEmail))
                {
                    imgTable.Columns.Add("image_name", typeof(string));
                    imgTable.Columns.Add("image_type", typeof(string));

                    if (income.Imagefile != null)
                    {
                        foreach (var img in income.Imagefile)
                        {
                            string wwwRootPath = hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(img.FileName);
                            string extension = Path.GetExtension(img.FileName);

                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                            string path = Path.Combine(wwwRootPath + "/images/user_images/", fileName);

                            imgTable.Rows.Add(fileName, "IM");

                            using (var filestream = new FileStream(path, FileMode.Create))
                            {
                                await img.CopyToAsync(filestream);
                            }
                        }
                    }

                    if (adminDBContext.UpdateIncome(income, imgTable))
                    {
                        ModelState.Clear();
                        TempData["Success"] = "Income updated succesfully!";
                        return View("Income");
                    }
                    else
                    {
                        TempData["Error"] = "Error Occured. Try again.";
                        return View("Income");
                    }
                }
                else
                {
                    TempData["error_user"] = "Donor Name is not present";
                    return View("Income");
                }

            }
            else
            {
                return View("Income");
            }

        }

        [Authorize(Roles = "A")]
        [Route("/Admin/Expense")]
        [HttpGet]
        public IActionResult Expense()
        {
            ViewBag.familyName = adminDBContext.GetFamilyName();
            return View();
        }

        [Authorize(Roles = "A")]
        [Route("/Admin/Expense")]
        [HttpPost]
        public async Task<IActionResult> PostExpense(Expense expense)
        {
            ViewBag.familyName = adminDBContext.GetFamilyName();

            if (ModelState.IsValid)
            {
                imgTable.Columns.Add("image_name", typeof(string));
                imgTable.Columns.Add("image_type", typeof(string));

                if (expense.Imagefile != null)
                {
                    foreach (var img in expense.Imagefile)
                    {
                        string wwwRootPath = hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(img.FileName);
                        string extension = Path.GetExtension(img.FileName);

                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                        string path = Path.Combine(wwwRootPath + "/images/user_images/", fileName);

                        imgTable.Rows.Add(fileName, "IM");

                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await img.CopyToAsync(filestream);
                        }
                    }
                }

                if (adminDBContext.UpdateExpense(expense, imgTable))
                {
                    ModelState.Clear();
                    TempData["Success"] = "Expense updated succesfully!";
                    return View("Expense");
                }
                else
                {
                    TempData["Error"] = "Error Occured. Try again.";
                    return View("Expense");
                }
            }
            else
            {
                return View("Expense");
            }
        }

        [Authorize(Roles = "A")]
        [Route("/Admin/Activity")]
        [HttpGet]
        public IActionResult Activity()
        {
            ViewBag.familyName = adminDBContext.GetFamilyName();
            return View();
        }

        [Authorize(Roles = "A")]
        [Route("/Admin/Activity")]
        [HttpPost]
        public async Task<IActionResult> PostActivity(AhrActivity ahrActivity)
        {
            ViewBag.familyName = adminDBContext.GetFamilyName();

            if (ModelState.IsValid)
            {
                imgTable.Columns.Add("image_name", typeof(string));
                imgTable.Columns.Add("image_type", typeof(string));

                vidTable.Columns.Add("video_name", typeof(string));
                vidTable.Columns.Add("video_type", typeof(string));

                if (ahrActivity.Imagefile != null)
                {
                    foreach (var img in ahrActivity.Imagefile)
                    {
                        string wwwRootPath = hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(img.FileName);
                        string extension = Path.GetExtension(img.FileName);

                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                        string path = Path.Combine(wwwRootPath + "/images/user_images/", fileName);

                        imgTable.Rows.Add(fileName, "IM");

                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await img.CopyToAsync(filestream);
                        }
                    }
                }

                if (ahrActivity.Videofile != null)
                {
                    foreach (var vid in ahrActivity.Videofile)
                    {
                        string wwwRootPath = hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(vid.FileName);
                        string extension = Path.GetExtension(vid.FileName);

                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                        string path = Path.Combine(wwwRootPath + "/video/", fileName);

                        vidTable.Rows.Add(fileName, "IM");

                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await vid.CopyToAsync(filestream);
                        }
                    }
                }


                if (adminDBContext.UpdateActivity(ahrActivity, imgTable, vidTable))
                {
                    ModelState.Clear();
                    TempData["Success"] = "Activity updated succesfully!";
                    return View("Activity");
                }
                else
                {
                    TempData["Error"] = "Error Occured. Try again.";
                    return View("Activity");
                }
            }
            else
            {
                TempData["error_user"] = "Donor Name is not present";
                return View("Activity");
            }

        }

        [Authorize(Roles = "A")]
        [Route("/Admin/AddUser")]
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [Authorize(Roles = "A")]
        [Route("/Admin/AddUser")]
        [HttpPost]
        public IActionResult PostAddUser(AddUser addUser)
        {
            if (ModelState.IsValid)
            {
                if (adminDBContext.AddUser(addUser))
                {
                    ModelState.Clear();
                    TempData["Success"] = "User Created succesfully!";
                    return View("AddUser");
                }
                else
                {
                    TempData["Error"] = "Email Id Already Present. Please try with different Mail Id. If Error persists, Please contact administrator.";
                    return View("AddUser");
                }


            }
            else
            {
                return View("AddUser");
            }

        }
    }
}
