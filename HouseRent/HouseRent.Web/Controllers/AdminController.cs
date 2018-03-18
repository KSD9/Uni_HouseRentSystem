using HouseRent.DataAccess.UnitOfWork;
using HouseRent.UserServices.Domain;
using HouseRent.UserServices;
using HouseRent.Web.Authorization;
using HouseRent.Web.Validation;
using HouseRent.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HouseRent.Web.Controllers
{
    public class AdminController : Controller
    {
        private AdminService adminService;

        public AdminController()
        {
            adminService = new AdminService(new UnitOfWork(), new ValidationDictionary(ModelState));
        }

        //GET: Admin
        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Index()
        {
            return View(await adminService.GetAllAsync());
        }

        [HttpGet]
        public ActionResult Profile()
        {
            int id = AuthenticationController.logged.Id;
            Admin admin = adminService.Get(id);
            AdminProfileViewModel adminProfileViewModel = new AdminProfileViewModel(admin.FirstName, admin.LastName, admin.Email);

            return View(adminProfileViewModel);
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Details(int id)
        {
            return View(await adminService.GetAsync(a => a.Id == id));
        }

        //Gets the details for the logged admin
        [HttpGet, AuthorizationActionFilter(true)]
        public ActionResult AdminDetails()
        {
            Admin admin = adminService.Get(AuthenticationController.logged.Id);

            if (admin == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(admin);
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Admin admin)
        {
            //Calls the validation method in the service
            adminService.Validation(admin);

            if (ModelState.IsValid)
            {
                if (adminService.IsEmailEntered(admin))
                {
                    TempData["EmailAdmin"] = "Email is alredy used, please choose another one!";
                    return View("Create");
                }

                adminService.HashPassword(admin);
                adminService.Insert(admin);
                await adminService.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Create");
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Update(int id)
        {
            Admin admin = await adminService.GetAsync(a => a.Id == id);
            adminService.SetAdminPasswordOnUpdate(admin);

            return View(admin);
        }
        [HttpPost]
        public async Task<ActionResult> Update(Admin admin)
        {
            //Calls the validation method in the service
            adminService.Validation(admin);

            if (ModelState.IsValid)
            {
                adminService.HashPassword(admin);
                adminService.Update(admin);
                await adminService.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Update");
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await adminService.GetAsync(a => a.Id == id));
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Admin admin)
        {
            adminService.Delete(admin);

            await adminService.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}