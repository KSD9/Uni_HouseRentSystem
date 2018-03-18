using HouseRent.BaseService;
using HouseRent.Common.Interfaces;
using HouseRent.DataAccess.UnitOfWork;
using HouseRent.RelationalServices.Domain.HouseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseRent.HouseService
{
   public class HouseService : BaseService<House>
    {
        public HouseService(UnitOfWork unitOfWork, IValidation modelStateWrapper)
            : base(unitOfWork, modelStateWrapper)
        {
        }

        public override bool Validation(House house)
        {
            return true;
        }

        //Setting all the references that the deleted car has to null in order not to delete the CarRent objects in the db
        public void SetAllReferencesToNullWhenDeleted(House house)
        {
            List<RentHouse> houseRents = UnitOfWork.GetRepo<RentHouse>()
                .GetAll(c => c.HouseId == house.Id)
                .Select(c =>
                {
                    c.HouseId = null;
                    return c;
                })
                .ToList();

            foreach (RentHouse houseRent in houseRents)
            {
                UnitOfWork.GetRepo<RentHouse>().Update(houseRent);
                UnitOfWork.Save();
            }
        }
    }
}
