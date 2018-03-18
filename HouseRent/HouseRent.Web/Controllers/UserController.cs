using HouseRent.DataAccess.UnitOfWork;
using HouseRent.RelationalServices.Domain.UserModel;
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
    public class UserController : Controller
    {
        private UserServices.UserService userService;

        public UserController()
        {
            userService = new UserServices.UserService(new UnitOfWork(), new ValidationDictionary(ModelState));
        }

        // GET: User
        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Index()
        {
            return View(await userService.GetAllAsync());
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public ActionResult Details(int id)
        {
            User user = userService.Get(id);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(user);
        }

        //Gets the details for the logged user
        [HttpGet, AuthorizationActionFilter(true)]
        public ActionResult UserDetails()
        {
            User user = userService.Get(AuthenticationController.logged.Id);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            UserProfileViewModel userProfileViewModel = new UserProfileViewModel(
                user.Username, user.FirstName,
                user.LastName, user.Email,
                user.BirthDate, user.PhoneNumber,
                user.AdditionalInformation);

            return View(userProfileViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            //Calls the validation method in the service
            userService.Validation(user);

            if (ModelState.IsValid)
            {
                if (userService.IsEmailEntered(user))
                {
                    TempData["EmailUser"] = "Email is alredy used, please choose another one!";
                    return View("Create");
                }

                userService.HashPassword(user);
                userService.Insert(user);

                await userService.SaveChangesAsync();
                TempData["Information"] = "You have successfully registed! Now please log in.";

                return RedirectToAction("Index", "Home");
            }

            return View("Create");
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public async Task<ActionResult> Update(int id)
        {
            User user = userService.Get(id);

            userService.SetUserPasswordOnUpdate(user);

            //If user does not exsists
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!AuthenticationController.IsLoggedAdmin)
            {
                //If this is not this user's profile
                if (!await userService.IsUsersProfile(user, AuthenticationController.logged))
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Update(User user)
        {
            //Calls the validation method in the service
            userService.Validation(user);

            if (ModelState.IsValid)
            {
                userService.HashPassword(user);
                userService.Update(user);

                await userService.SaveChangesAsync();

                if (AuthenticationController.IsLoggedAdmin)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("UserDetails");
            }

            return View("Update");
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await userService.GetAsync(u => u.Id == id));
        }
        [HttpPost]
        public async Task<ActionResult> Delete(User user)
        {
            userService.Delete(user);

            await userService.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}