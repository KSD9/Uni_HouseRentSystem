using HouseRent.BaseService.Domain;
using HouseRent.Enumerations.HouseEnums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseRent.RelationalServices.Domain.HouseModel
{
    public class House: BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Image URL")]
        public string ImgURL { get; set; }

        [Required]
        [Range(2, 10)]
        [Display(Name = "Number of rooms")]
        public int NumberOfRooms { get; set; }

        //The exact price of the car on the market
        [Required]
        [Range(500, 100000)]
        [Display(Name = "House price")]
        public decimal PriceOfHouse { get; set; }

        //The price of rent per day of the car
        [Required]
        [Display(Name = "Price of rent per day")]
        public decimal PriceOfRentPerDay { get; set; }

        [Required]
        [Range(5, 100)]
        [Display(Name = "Guests capacity")]
        public int GuestsCapacity { get; set; }

        [Required]
        [Range(2, 5)]
        [Display(Name = "Number of Floors")]
        public int NumberOfFloors { get; set; }

        [Required]
        [Display(Name = "House type")]
        public HouseType HouseType { get; set; }

        //The number of houses of this model in the store and its branches
        [Required]
        [Range(1, 100)]
        [Display(Name = "Avaliable houses")]
        public int Amount { get; set; }

        //The connections of this model with the other models
        public virtual ICollection<RentHouse> HouseRent { get; set; }
    }
}
