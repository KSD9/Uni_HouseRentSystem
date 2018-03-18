using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using HouseRent.UserServices.Domain;
using HouseRent.RelationalServices.Domain.UserModel;
using HouseRent.RelationalServices.Domain.HouseModel;
using HouseRent.RelationalServices.Domain.PaymentModel;

namespace HouseRent.DataAccess.Context
{
    public partial class RentHouseDbContext : DbContext
    {
        public RentHouseDbContext()
            : base("name=RentHouseContext")
        {
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<House> House { get; set; }
        public DbSet<RentHouse> HouseRent { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
