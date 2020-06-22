using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PamojaClassroomAdminModule.Models;
using PamojaClassroomAdminModule.Repository.IRepository;

namespace PamojaClassroomAdminModule.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository _accRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger,IAccountRepository accRepo
            , UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _accRepo = accRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var users = _userManager.Users.Where(a => a.Module != "AdminModule");

            SD.TotalUsers = users.Count();
            return View();
        }

        
        public async Task<IActionResult> LogIn()
        {
            // check whether the user has been verified first. 

            var user = await GetCurrentUser();
            if (user.IsVerified)
            {
                var objUser = new AuthoriseUser()
                {
                    UserName = User.FindFirstValue(ClaimTypes.Name)
                };
                User obj = await _accRepo.LoginAsync(SD.AccountApiPath + "authenticate", objUser);
                if (obj.Token == null)
                {
                    await _signInManager.SignOutAsync();
                    return Redirect("/Identity/Account/Login");
                }

                HttpContext.Session.SetString("JWToken", obj.Token);

                return RedirectToAction("Index");
            }
            await _signInManager.SignOutAsync();
            return Redirect("/Identity/Account/Login");

        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
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
