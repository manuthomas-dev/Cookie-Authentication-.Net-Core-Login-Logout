using ExpenseManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManagerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExpenseManagerDBContext DBContext;

        public HomeController(ExpenseManagerDBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string username, string password, string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;

            var Result = ( from A in DBContext.UserLogins.Where(x => x.Username == username && x.UserPassword == password)
                                 join B in DBContext.Users on A.UserId equals B.UserId
                                 select new { A.UserId, A.Username, B.FirstName, B.LastName, B.Photo, B.Dob}).FirstOrDefault();
            if (Result != null)
            {
                var claims = new List<Claim> {
                    new Claim("Username",username),
                    new Claim(ClaimTypes.NameIdentifier, Result.UserId.ToString()),
                    new Claim(ClaimTypes.Name, Result.FirstName + ' ' + Result.LastName),
                    new Claim(ClaimTypes.DateOfBirth, Result.Dob.ToString("MM-dd-yyyy")),
                    new Claim("ImageUrl", Result.Photo)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal, new AuthenticationProperties { IsPersistent=true });

                return Redirect(ReturnUrl);
            }
            TempData["Error"] = "Error. Username or Password is invalid";
            return View("login");
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
