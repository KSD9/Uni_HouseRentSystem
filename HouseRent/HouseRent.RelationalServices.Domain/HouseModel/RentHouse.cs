using HouseRent.BaseService.Domain;
using HouseRent.Enumerations.HouseRentEnums;
using HouseRent.RelationalServices.Domain.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HouseRent.RelationalServices.Domain.HouseModel
{
    public class RentHouse : BaseModel
    {
        [Required]
        public string Name { get; set; }

        //Some notes about the renting of the house
        public string Note { get; set; }

        //The beggining date of rent
        [Required]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        //The end date of the rent
        [Required]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        //The calculated price for the selected days
        [Required]
        [Display(Name = "Total price")]
        public decimal TotalPrice { get; set; }

        //The place from where the house is rent
        [Required]
        [Display(Name = "Rental address")]
        public Address RentAddress { get; set; }

       

        //The navigation properties
        public int UserId { get; set; }
        public int? HouseId { get; set; }

        //The connections of this model with the other models
        public virtual User User { get; set; }
        public virtual House House { get; set; }
    }
}
