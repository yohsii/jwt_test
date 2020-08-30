using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JWT_Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JWT_Test.Services;
using JWT_Test.Data;
using Microsoft.AspNetCore.Identity;

namespace JWT_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private AuthenticationService authenticationService;

        private UserManager<User> userManager;
        public HomeController(ILogger<HomeController> logger,AuthenticationService authService,UserManager<User> userManager)
        {
            _logger = logger;
            this.authenticationService = authService;
            this.userManager = userManager; 
        }

        public async Task<IActionResult> CreateUser() {

            var user = new User
            {
                Email = "darkezmo@hotmail.com",
                UserName = "darkezmo@hotmail.com"
            };
            var result = await userManager.CreateAsync(user, "ChangeThis1!");
            if (!result.Succeeded)
            {
                string message = string.Join(" ", result.Errors.Select(x => x.Description));
                return Content(message);
            }

            //await userManager.AddToRolesAsync(user, new string[] { "arole" });

            return Content("user added");
        }

        public IActionResult Login(string username,string password) {
            var user = authenticationService.Authenticate(username,password);

            if (user == null)
                return Json(new { success=false});

            return Json(new {success=true,user=user});

        }

        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="arole2")]
        public IActionResult Test() {

            return Json(new { message="success!"});
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
    }
}
