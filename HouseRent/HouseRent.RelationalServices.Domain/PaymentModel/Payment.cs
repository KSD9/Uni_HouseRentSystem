using HouseRent.BaseService.Domain;
using HouseRent.Enumerations.PaymentEnums;
using HouseRent.RelationalServices.Domain.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HouseRent.RelationalServices.Domain.PaymentModel
{
    public class Payment : BaseModel
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [Display(Name = "Number of card")]
        public string NumberOfCard { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        [Display(Name = "Security number")]
        public string SecurityNumber { get; set; }

        //The date on which the user entered this payment method
        [Required]
        public DateTime Date { get; set; }

        //The type of the card: Visa, MasterCard ect.
        [Required]
        [Display(Name = "Card type")]
        public CardType CardType { get; set; }

        [Required]
        [Display(Name = "Card expires on")]
        public DateTime CardExpiresOn { get; set; }

        [Required]
        [Display(Name = "Is this your main payment method?")]
        public bool IsMainPaymentMethod { get; set; }

        //The navigation properties
        public int UserId { get; set; }

        //The connections of this model with the other models
        public virtual User User { get; set; }
    }
}
