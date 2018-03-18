using HouseRent.DataAccess.UnitOfWork;
using HouseRent.HouseService;
using HouseRent.RelationalServices.Domain.HouseModel;
using HouseRent.RelationalServices.Domain.UserModel;
using HouseRent.Web.Authorization;
using HouseRent.Web.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HouseRent.Web.Controllers
{
    public class HouseRentController : Controller
    {
        private HouseRentService houseRentService;

        public HouseRentController()
        {
            houseRentService = new HouseRentService(new UnitOfWork(), new ValidationDictionary(ModelState));
        }

        // GET: CarRent
        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Index()
        {
            return View("ListAllHouseRents", await houseRentService.GetAllAsync());
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public async Task<ActionResult> Details(int id)
        {
            RentHouse houseRent = houseRentService.Get(id);
            //If payment does not exsists
            if (houseRent == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!AuthenticationController.IsLoggedAdmin)
            {
                //If this is not this user's payment
                if (!await houseRentService.IsUsersCarRent(houseRent, AuthenticationController.logged))
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(houseRent);
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public async Task<ActionResult> ListCurrentUserHouseRents()
        {
            return View("Index", await houseRentService.GetAllAsync(p => p.UserId == AuthenticationController.logged.Id));
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RentHouse houseRent, int id)
        {
            houseRent.HouseId = id;

            if (houseRentService.CheckIfCarAmountIsZero(id))
            {
                return RedirectToAction("EmptyCarAmount", "Home");
            }

            if (houseRentService.CheckIfMainPaymentIsNotSet((User)AuthenticationController.logged))
            {
                TempData["Payment"] = "You have to set a main payment method";

                return View("Create");
            }

            //Calls the validation method in the service
            houseRentService.Validation(houseRent);

            if (ModelState.IsValid)
            {
                houseRent.UserId = AuthenticationController.logged.Id;

                houseRentService.CalculateTotalPrice(houseRent);

                houseRentService.ReduceTheNumberOfAvaliableCars(houseRent);

                houseRentService.Insert(houseRent);

                await houseRentService.SaveChangesAsync();

                if (AuthenticationController.IsLoggedAdmin)
                {
                    return RedirectToAction("ListCurrentAdminHouseRents");
                }

                return RedirectToAction("ListCurrentUserHouseRents");
            }
            return View("Create");
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Update(int id)
        {
            TempData["HouseRentId"] = id;
            return View(await houseRentService.GetAsync(cr => cr.Id == id));
        }
        [HttpPost]
        public async Task<ActionResult> Update(RentHouse houseRent)
        {
            //Calls the validation method in the service
            houseRentService.Validation(houseRent);

            //Sets the HouseRent to the original owner properties after update
            houseRentService.SetHouseRentPropertiesOnUpdate(houseRent, int.Parse(TempData["HouseRentId"].ToString()));

            if (ModelState.IsValid)
            {
                houseRentService.Update(houseRent);

                await houseRentService.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Update");
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await houseRentService.GetAsync(cr => cr.Id == id));
        }
        [HttpPost]
        public async Task<ActionResult> Delete(RentHouse houseRent)
        {
            houseRentService.Delete(houseRent);

            await houseRentService.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
