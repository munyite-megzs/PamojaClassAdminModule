using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PamojaClassroomAdminModule.Models;
using PamojaClassroomAdminModule.Repository.IRepository;
using PamojaClassroomAdminModule.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PamojaClassroomAdminModule.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManipulation _userRepository;

        public AdminController(RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager,
                                IUserManipulation adminUserControlsRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userRepository = adminUserControlsRepository;
        }

        public IActionResult ListUsers()
        {

            return View(new AdminControlsInput() { });
        }

        [HttpGet]
        public IActionResult GetListUsers()
        {
            var users = _userManager.Users.Where(a => a.Module == "AdminModule");
            var userList = new List<UsersViewModel>();

            foreach (var user in users)
            {
                var usersViewModel = new UsersViewModel
                {
                    //Name, Email, IsVerified,EmailConfirmed 
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IsVerified =  user.IsVerified == true ? "Yes" : "No",
                    EmailConfirmed = user.EmailConfirmed == true  ? "Yes" : "No"
                };
                userList.Add(usersViewModel);
            }
           
             return Json(new { data = userList });
            //return View(users);
        } 


        public async Task<IActionResult> Approve(string id)
        {
            var usermanipulation = new UserManipulation()
            {
                Id = id
            };
            await _userRepository.UpdateAsync(SD.UserManipulationPath + 1, usermanipulation, HttpContext.Session.GetString("JWToken"));

            return RedirectToAction("ListUsers");// View("GetListUsers");

        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var usermanipulation = new UserManipulation()
            {
                Id = id
            };
            //await _userRepository.DeleteAsync(SD.GradesTaughtUrl, id, DeleteUserAsync, HttpContext.Session.GetString("JWToken"));

            var status = await _userRepository.DeleteUserAsync(SD.UserManipulationPath + 1, usermanipulation, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Deleted Successfuly" });
            }

            return Json(new { success = true, message = "Delete Not Successful" });

        }

        [HttpGet]
        public IActionResult CreateRole()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return RedirectToAction("");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");
            }
        }


        [HttpPost]
        public async Task<IActionResult>CreateRole(CreateRoleViewModel createRole)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRole.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
            }

            
            return View(createRole);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);

        }  

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
           var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return BadRequest();
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
           
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = "Role cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = editRoleViewModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(editRoleViewModel);
            }




        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = "This role cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
         
            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = "Role Not Found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;



                //check whether a user is a meber of the service
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {

                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    //remove user from role
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }

            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }

    }
}