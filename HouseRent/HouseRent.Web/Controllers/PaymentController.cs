using HouseRent.DataAccess.UnitOfWork;
using HouseRent.PaymentServices;
using HouseRent.RelationalServices.Domain.PaymentModel;
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
    public class PaymentController : Controller
    {
        private PaymentService paymentService;

        public PaymentController()
        {
            paymentService = new PaymentService(new UnitOfWork(), new ValidationDictionary(ModelState));
        }

        // GET: Payment
        [HttpGet, AuthorizationActionFilter(false)]
        public async Task<ActionResult> Index()
        {
            return View("ListAllPayments", await paymentService.GetAllAsync());
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public async Task<ActionResult> ListCurrentUserPayments()
        {
            return View("Index", await paymentService.GetAllAsync(p => p.UserId == AuthenticationController.logged.Id));
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public async Task<ActionResult> Details(int id)
        {
            Payment payment = paymentService.Get(id);
            //If payment does not exsists
            if (payment == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!AuthenticationController.IsLoggedAdmin)
            {
                //If this is not this user's payment
                if (!await paymentService.IsUsersPayment(payment, AuthenticationController.logged))
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(payment);
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Payment payment)
        {
            payment.UserId = AuthenticationController.logged.Id;

            //Calls the SetCardInputDate() method in the service
            paymentService.SetCardInputDate(payment);
            //Calls the validation method in the service
            paymentService.Validation(payment);

            if (ModelState.IsValid)
            {
                paymentService.Insert(payment);

                await paymentService.SaveChangesAsync();

                if (AuthenticationController.IsLoggedAdmin)
                {
                    return RedirectToAction("ListCurrentAdminPayments");
                }

                return RedirectToAction("ListCurrentUserPayments");
            }

            return View("Create");
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public async Task<ActionResult> Update(int id)
        {
            Payment payment = paymentService.Get(id);
            //If payment does not exsists
            if (payment == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!AuthenticationController.IsLoggedAdmin)
            {
                //If this is not this user's payment
                if (!await paymentService.IsUsersPayment(payment, AuthenticationController.logged))
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(payment);
        }
        [HttpPost]
        public async Task<ActionResult> Update(Payment payment)
        {
            payment.UserId = AuthenticationController.logged.Id;

            //Calls the SetCardInputDate() method in the service
            paymentService.SetCardInputDate(payment);
            //Calls the validation method in the service
            paymentService.Validation(payment);

            if (ModelState.IsValid)
            {
                paymentService.Update(payment);

                await paymentService.SaveChangesAsync();

                if (AuthenticationController.IsLoggedAdmin)
                {
                    return RedirectToAction("ListCurrentAdminPayments");
                }

                return RedirectToAction("ListCurrentUserPayments");
            }

            return View("Update");
        }

        [HttpGet, AuthorizationActionFilter(true)]
        public async Task<ActionResult> Delete(int id)
        {
            Payment payment = paymentService.Get(id);
            //If payment does not exsists
            if (payment == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!AuthenticationController.IsLoggedAdmin)
            {
                //If this is not this user's payment
                if (!await paymentService.IsUsersPayment(payment, AuthenticationController.logged))
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(payment);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Payment payment)
        {
            paymentService.Delete(payment);

            await paymentService.SaveChangesAsync();

            if (AuthenticationController.IsLoggedAdmin)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("ListCurrentUserPayments");
        }
    }
}