using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PamojaClassroomAdminModule.Models;
using PamojaClassroomAdminModule.Repository.IRepository;
using System.Threading.Tasks;

namespace PamojaClassroomAdminModule.Controllers
{
    [Authorize]
    public class SchoolTypeController : Controller
    {
        private readonly IAdminUserControlsRepository _adminUserControlsRepository;
        public SchoolTypeController(IAdminUserControlsRepository adminUserControlsRepository)
        {
            _adminUserControlsRepository = adminUserControlsRepository;
        }
        public IActionResult Index()
        {

            return View(new AdminControlsInput() { });
        }

        public async Task<IActionResult> GetAllSchoolTypes()
        {
            return Json(new { data = await _adminUserControlsRepository.GetAllAsync(SD.InterestUrl, HttpContext.Session.GetString("JWToken")) });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            AdminControlsInput obj = new AdminControlsInput();
            if (id == null)
            {
                //this will be true for create/insert
                return View(obj);
            }

            //Flow will come here for update
            obj = await _adminUserControlsRepository.GetAsync(SD.InterestUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(AdminControlsInput adminControlsInput)
        {
            if (ModelState.IsValid)
            {
                if (adminControlsInput.Id == 0)
                {
                    await _adminUserControlsRepository.CreateAsync(SD.InterestUrl, adminControlsInput, HttpContext.Session.GetString("JWToken"));

                }
                else
                {
                    await _adminUserControlsRepository.UpdateAsync(SD.InterestUrl + adminControlsInput.Id, adminControlsInput, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(adminControlsInput);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _adminUserControlsRepository.DeleteAsync(SD.InterestUrl, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Deleted Successfuly" });
            }

            return Json(new { success = true, message = "Delete Not Successful" });
        }

    }
}