using HouseRent.BaseService;
using HouseRent.BaseService.Domain;
using HouseRent.Common.Interfaces;
using HouseRent.DataAccess.UnitOfWork;
using HouseRent.RelationalServices.Domain.HouseModel;
using HouseRent.RelationalServices.Domain.PaymentModel;
using HouseRent.RelationalServices.Domain.UserModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseRent.HouseService
{
    public class HouseRentService : BaseService<RentHouse>
    {
        public HouseRentService(UnitOfWork unitOfWork, IValidation modelStateWrapper)
            : base(unitOfWork, modelStateWrapper)
        {
        }

        //Method that validates some properties using the ModelStateWrapper property in the BaseService
        public override bool Validation(RentHouse houseRent)
        {
            if (houseRent.StartDate.Date < DateTime.Now.Date)
            {
                ModelStateWrapper.Error("StartDate", "The start date has to be valid");
            }

            if (houseRent.EndDate.Date < DateTime.Now.Date)
            {
                ModelStateWrapper.Error("EndDate", "The end date has to be valid");
            }

            if (houseRent.StartDate.Date > houseRent.EndDate.Date)
            {
                ModelStateWrapper.Error("EndDate", "The end date has to further or equal to the start date");
            }

            return ModelStateWrapper.IsValid;
        }

        //Reduces the amount of the rented car in the database
        public void ReduceTheNumberOfAvaliableCars(RentHouse houseRent)
        {
            House house = UnitOfWork.GetRepo<House>().Get((int)houseRent.HouseId);

            house.Amount--;

            UnitOfWork.GetRepo<House>().Update(house);
            UnitOfWork.SaveWithoutValidation();
        }

        //Calculates the price of the rent
        public void CalculateTotalPrice(RentHouse houseRent)
        {
            House house = UnitOfWork.GetRepo<House>().Get((int)houseRent.HouseId);

            if (houseRent.EndDate == houseRent.StartDate)
            {
                houseRent.TotalPrice = house.PriceOfRentPerDay;
            }
            else
            {
                houseRent.TotalPrice = (house.PriceOfRentPerDay * (decimal)(houseRent.EndDate - houseRent.StartDate).TotalDays);
            }
        }

        //Checks if the showed CarRent is this user's
        public async Task<bool> IsUsersCarRent(RentHouse houseRent, BaseModel loggedUser)
        {
            RentHouse currentHouseRent = await GetAsync(c => c.Id == houseRent.Id);

            if (currentHouseRent.UserId == loggedUser.Id)
            {
                return true;
            }

            return false;
        }

        //Sets the CarRent to the original owner properties after update
        public void SetHouseRentPropertiesOnUpdate(RentHouse updatedHouseRent, int Id)
        {
            List<RentHouse> currentCarRent = GetAllNoTracking(c => c.Id == Id);

            updatedHouseRent.UserId = currentCarRent[0].UserId;
            updatedHouseRent.TotalPrice = currentCarRent[0].TotalPrice;
            updatedHouseRent.HouseId = currentCarRent[0].HouseId;
        }

        //Checks the car amount on car renting
        public bool CheckIfCarAmountIsZero(int id)
        {
            House house = UnitOfWork.GetRepo<House>().Get(id);

            if (house.Amount == 0)
            {
                return true;
            }

            return false;
        }

        //Checks the payment method of the user on car renting
        public bool CheckIfMainPaymentIsNotSet(User user)
        {
            List<Payment> payments = UnitOfWork.GetRepo<Payment>().GetAll(p => p.UserId == user.Id);

            if (payments.Count != 0)
            {
                foreach (Payment payment in payments)
                {
                    if (payment.IsMainPaymentMethod)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
