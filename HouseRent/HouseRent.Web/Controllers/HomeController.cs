using HouseRent.NotificationServices;
using HouseRent.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseRent.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(EmailSendingViewModel emailSendingViewModel)
        {
            EmailSender emailSender = new EmailSender();

            if (emailSendingViewModel.Body == null || emailSendingViewModel.From == null || emailSendingViewModel.Subject == null)
            {
                TempData["Email"] = "You have to enter all the needed information for sending an email!";
                return View("Contact");
            }

            emailSender.SendMail(emailSendingViewModel.From, emailSendingViewModel.Subject, emailSendingViewModel.Body);
            TempData["Email"] = "You have send the email successfully!";

            return View("Contact");
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EmptyCarAmount()
        {
            return View();
        }

        [HttpGet]
        public ActionResult News()
        {
            return View();
        }
    }
}