using System.Web.Mvc;

using HouseRent.BaseService.Domain;

using HouseRent.UserServices;

using HouseRent.DataAccess.UnitOfWork;
using HouseRent.Web.Validation;
using HouseRent.RelationalServices.Domain.UserModel;
using HouseRent.UserServices.Domain;

namespace HouseRent.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        public static BaseModel logged { get; private set; }
        public static bool IsLoggedAdmin { get; set; }

        private AdminService adminService;
        private UserServices.UserService userService;
        private UnitOfWork unitOfWork = new UnitOfWork();

        public AuthenticationController()
        {
            userService = new UserServices.UserService(unitOfWork, new ValidationDictionary(ModelState));
            adminService = new AdminService(unitOfWork, new ValidationDictionary(ModelState));
        }

        // GET: Authentication
        //User login form
        [HttpGet]
        public ActionResult UserLogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogIn(User user)
        {
            if (user.Email != null && user.Password != null)
            {
                User loggedUser = new User();

                loggedUser = userService.LogIn(user);

                if (loggedUser != null)
                {
                    Session["LogInData"] = loggedUser.Username;
                    logged = loggedUser;

                    return RedirectToAction("Index", "Home");
                }

                TempData["TextError"] = "Email or password were entered incorrectly";

                return View("UserLogIn");
            }

            return View("UserLogIn");
        }

        //Admin login form
        [HttpGet]
        public ActionResult AdminLogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogIn(Admin admin)
        {
            if (admin.Email != null && admin.Password != null)
            {
                Admin loggedAdmin = new Admin();

                loggedAdmin = adminService.LogIn(admin);

                if (loggedAdmin != null)
                {
                    Session["LogInData"] = string.Format($"{loggedAdmin.FirstName} {loggedAdmin.LastName}");
                    logged = loggedAdmin;
                    IsLoggedAdmin = true;

                    return RedirectToAction("Index", "Home");
                }

                TempData["TextError"] = "Email or password were entered incorrectly";

                return View("AdminLogIn");
            }

            return View("AdminLogIn");
        }

        //Logout button
        public ActionResult LogOut()
        {
            Session.Clear();
            IsLoggedAdmin = false;
            logged = null;

            TempData["Information"] = "You have logged out successfully!";

            return RedirectToAction("Index", "Home");
        }
    }
}