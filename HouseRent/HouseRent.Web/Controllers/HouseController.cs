using HouseRent.DataAccess.UnitOfWork;
using HouseRent.RelationalServices.Domain.HouseModel;
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
    public class HouseController : Controller
    {
        private HouseService.HouseService houseService;

        public HouseController()
        {
            houseService = new HouseService.HouseService(new UnitOfWork(), new ValidationDictionary(ModelState));
        }

        // GET: Car
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await houseService.GetAllAsync());
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            House house = houseService.Get(id);

            if (house == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(house);
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(House house)
        {
            if (ModelState.IsValid)
            {
                houseService.Insert(house);
                await houseService.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Create");
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Update(int id)
        {
            return View(await houseService.GetAsync(c => c.Id == id));
        }
        [HttpPost]
        public async Task<ActionResult> Update(House house)
        {
            if (ModelState.IsValid)
            {
                houseService.Update(house);
                await houseService.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Update");
        }

        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await houseService.GetAsync(c => c.Id == id));
        }
        [HttpPost]
        public async Task<ActionResult> Delete(House house)
        {
            //Calls the SetAllReferencesToNullWhenDeleted() method in the service
            houseService.SetAllReferencesToNullWhenDeleted(house);
            houseService.Delete(house);

            await houseService.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}