using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRent.Web.ViewModels
{
    public class AdminProfileViewModel
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }

        public AdminProfileViewModel(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}