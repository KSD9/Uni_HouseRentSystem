using HouseRent.BaseService;
using HouseRent.Common.Interfaces;
using HouseRent.Common.PasswordHash;
using HouseRent.DataAccess.UnitOfWork;
using HouseRent.UserServices.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseRent.UserServices
{
    public class AdminService : BaseService<Admin>
    {
        public AdminService(UnitOfWork unitOfWork, IValidation modelStateWrapper)
            : base(unitOfWork, modelStateWrapper)
        {
        }

        //Method that validates some properties using the ModelStateWrapper property in the BaseService
        public override bool Validation(Admin admin)
        {
            if (!admin.Password.Any(c => char.IsUpper(c)) || !admin.Password.Any(c => char.IsLower(c)) || !admin.Password.Any(c => char.IsDigit(c)))
            {
                ModelStateWrapper.Error("Password", "Password has to contain at least one uppercase, lowercase letter and a number.");
            }
            return ModelStateWrapper.IsValid;
        }

        //Check if admin is correct
        public Admin LogIn(Admin admin)
        {
            List<Admin> currentAdmin = GetAll(a => a.Email == admin.Email);

            if (currentAdmin.Count != 0)
            {
                if (CheckPassword(admin, currentAdmin[0]))
                {
                    return currentAdmin[0];
                }
            }

            return null;
        }

        //Check if email is exsists
        public bool IsEmailEntered(Admin admin)
        {
            List<Admin> admins = GetAll(a => a.Email == admin.Email);

            if (admins.Count == 0)
            {
                return false;
            }

            return true;
        }

        //Hashes the password
        public void HashPassword(Admin admin)
        {
            PasswordHasher passwordHasher = new PasswordHasher();
            admin.Password = passwordHasher.HashPassword(admin.Password);
        }

        //Checks the password
        public bool CheckPassword(Admin admin, Admin currentAdmin)
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            return passwordHasher.CheckPassword(admin.Password, currentAdmin.Password);
        }

        //Setting the admin password to null on update
        public void SetAdminPasswordOnUpdate(Admin admin)
        {
            admin.Password = string.Empty;
        }
    }
}
