using HouseRent.BaseService;
using HouseRent.BaseService.Domain;
using HouseRent.Common.Interfaces;
using HouseRent.Common.PasswordHash;
using HouseRent.DataAccess.UnitOfWork;
using HouseRent.RelationalServices.Domain.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRent.UserServices
{
    public class UserService : BaseService<User>
    {
        public UserService(UnitOfWork unitOfWork, IValidation modelStateWrapper)
            : base(unitOfWork, modelStateWrapper)
        {
        }

        //Method that validates some properties using the ModelStateWrapper property in the BaseService
        public override bool Validation(User user)
        {
            if (!user.Password.Any(c => char.IsUpper(c)) || !user.Password.Any(c => char.IsLower(c)) || !user.Password.Any(c => char.IsDigit(c)))
            {
                ModelStateWrapper.Error("Password", "Password has to contain at least one uppercase, lowercase letter and a number.");
            }

            if (GetAge(user.BirthDate) < 18)
            {
                ModelStateWrapper.Error("BirthDate", "You have to be minimum 18 years old to rent a house!");
            }
            return ModelStateWrapper.IsValid;
        }

        //Calculates the age of the user using the birthDate property of the model
        public static int GetAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int numberForToday = (today.Year * 100 + today.Month) * 100 + today.Day;
            int numberForGivenBirthday = (birthDate.Year * 100 + birthDate.Month) * 100 + birthDate.Day;
            int result = (numberForToday - numberForGivenBirthday) / 10000;

            return result;
        }

        //Check if user is correct
        public User LogIn(User user)
        {
            List<User> currentUsers = GetAll(u => u.Email == user.Email);

            if (currentUsers.Count != 0)
            {
                if (CheckPassword(user, currentUsers[0]))
                {
                    return currentUsers[0];
                }
            }

            return null;
        }

        //Checks if the updated user is this user's profile
        public async Task<bool> IsUsersProfile(User user, BaseModel loggedUser)
        {
            User currentUser = await GetAsync(u => u.Id == user.Id);

            if (currentUser.Id == loggedUser.Id)
            {
                return true;
            }

            return false;
        }

        //Hashes the password
        public void HashPassword(User user)
        {
            PasswordHasher passwordHasher = new PasswordHasher();
            user.Password = passwordHasher.HashPassword(user.Password);
        }

        //Checks the password
        public bool CheckPassword(User user, User currentUser)
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            return passwordHasher.CheckPassword(user.Password, currentUser.Password);
        }

        //Check if email is exsists
        public bool IsEmailEntered(User user)
        {
            List<User> users = GetAll(u => u.Email == user.Email);

            if (users.Count == 0)
            {
                return false;
            }

            return true;
        }

        //Setting the user password to null on update
        public void SetUserPasswordOnUpdate(User user)
        {
            user.Password = string.Empty;
        }
    }
}
