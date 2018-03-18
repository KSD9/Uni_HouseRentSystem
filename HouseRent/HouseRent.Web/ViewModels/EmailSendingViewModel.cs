using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRent.Web.ViewModels
{
    public class EmailSendingViewModel
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}