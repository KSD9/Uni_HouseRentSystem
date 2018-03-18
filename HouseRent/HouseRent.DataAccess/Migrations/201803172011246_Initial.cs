namespace HouseRent.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ImgURL = c.String(),
                        NumberOfRooms = c.Int(nullable: false),
                        PriceOfHouse = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceOfRentPerDay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GuestsCapacity = c.Int(nullable: false),
                        NumberOfFloors = c.Int(nullable: false),
                        HouseType = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentHouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Note = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RentAddress = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        HouseId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Houses", t => t.HouseId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.HouseId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 20),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        AdditionalInformation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfCard = c.String(nullable: false, maxLength: 16),
                        SecurityNumber = c.String(nullable: false, maxLength: 3),
                        Date = c.DateTime(nullable: false),
                        CardType = c.Int(nullable: false),
                        CardExpiresOn = c.DateTime(nullable: false),
                        IsMainPaymentMethod = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "UserId", "dbo.Users");
            DropForeignKey("dbo.RentHouses", "UserId", "dbo.Users");
            DropForeignKey("dbo.RentHouses", "HouseId", "dbo.Houses");
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropIndex("dbo.RentHouses", new[] { "HouseId" });
            DropIndex("dbo.RentHouses", new[] { "UserId" });
            DropTable("dbo.Payments");
            DropTable("dbo.Users");
            DropTable("dbo.RentHouses");
            DropTable("dbo.Houses");
            DropTable("dbo.Admins");
        }
    }
}
