using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRent.Web.ViewModels
{
    public class UserProfileViewModel
    {
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string PhoneNumber { get; private set; }
        public string AdditionalInformation { get; private set; }

        public UserProfileViewModel(string username, string firstName, string lastName, string email, DateTime birthDate, string phoneNumber, string addInfo)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            AdditionalInformation = addInfo;
        }
    }
}