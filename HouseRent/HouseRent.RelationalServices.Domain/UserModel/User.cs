using HouseRent.BaseService.Domain;
using HouseRent.RelationalServices.Domain.HouseModel;
using HouseRent.RelationalServices.Domain.PaymentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HouseRent.RelationalServices.Domain.UserModel
{
   public class User : BaseModel
    {
        //The name that is going to be shown in the application
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        //Some additional information about the user
        [Display(Name = "Additional information")]
        public string AdditionalInformation { get; set; }

        //The connections of this model with the other models
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<RentHouse> HouseRents { get; set; }
    }
}
