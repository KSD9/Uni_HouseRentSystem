using HouseRent.BaseService;
using HouseRent.BaseService.Domain;
using HouseRent.Common.Interfaces;
using HouseRent.DataAccess.UnitOfWork;
using HouseRent.RelationalServices.Domain.PaymentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseRent.PaymentServices
{
    public class PaymentService : BaseService<Payment>
    {
        public PaymentService(UnitOfWork unitOfWork, IValidation modelStateWrapper)
            : base(unitOfWork, modelStateWrapper)
        {
        }

        //Method that validates some properties using the ModelStateWrapper property in the BaseService
        public override bool Validation(Payment payment)
        {
            if (payment.CardExpiresOn.Date < DateTime.Now.Date)
            {
                ModelStateWrapper.Error("CardExpiresOn", "The card has to be valid");
            }

            if (payment.IsMainPaymentMethod)
            {
                if (IsMainPaymentSet(payment))
                {
                    ModelStateWrapper.Error("IsMainPaymentMethod", "You have already set a main payment method");
                }
            }

            return ModelStateWrapper.IsValid;
        }

        //Checks if the user has alredy selected a main payment method
        public bool IsMainPaymentSet(Payment payment)
        {
            List<Payment> payments;

            payments = GetAllNoTracking(p => p.UserId == payment.UserId);

            foreach (Payment currentPayment in payments)
            {
                if (currentPayment.IsMainPaymentMethod == true)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetCardInputDate(Payment payment)
        {
            payment.Date = DateTime.Now.Date;
        }

        //Checks if the updated payment is this user's payment
        public async Task<bool> IsUsersPayment(Payment payment, BaseModel loggedUser)
        {
            Payment currentPayment = await GetAsync(p => p.Id == payment.Id);

            if (currentPayment.UserId == loggedUser.Id)
            {
                return true;
            }

            return false;
        }
    }
}
