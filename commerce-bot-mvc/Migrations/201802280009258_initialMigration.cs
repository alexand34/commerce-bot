namespace commerce_bot_mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BotUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessengerId = c.String(),
                        UserName = c.String(),
                        Language = c.Int(),
                        toId = c.String(),
                        toName = c.String(),
                        fromId = c.String(),
                        fromName = c.String(),
                        serviceUrl = c.String(),
                        channelId = c.String(),
                        conversationId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        DestinationAddress = c.String(),
                        DeliveryPrice = c.Double(),
                        Price = c.Double(),
                        RestaurantId = c.Int(),
                        OrderState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId)
                .ForeignKey("dbo.BotUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FoodId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Foods", t => t.FoodId, cascadeDelete: true)
                .Index(t => t.FoodId);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FoodCategoryId = c.Int(nullable: false),
                        RestaurantId = c.Int(nullable: false),
                        DishName = c.String(),
                        Price = c.Double(nullable: false),
                        Portion = c.String(),
                        DishDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FoodCategories", t => t.FoodCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.FoodCategoryId)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.FoodCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FoodCategoryName = c.String(),
                        FoodCategoryFrenchName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RestaurantName = c.String(),
                        RestaurantAddress = c.String(),
                        CategoryId = c.Int(),
                        AverageReceipt = c.Double(),
                        Rating = c.Double(),
                        AppUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        ImgName = c.String(),
                        FrenchCategoryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrderOrderItems",
                c => new
                    {
                        Order_Id = c.Int(nullable: false),
                        OrderItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_Id, t.OrderItem_Id })
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.OrderItems", t => t.OrderItem_Id, cascadeDelete: true)
                .Index(t => t.Order_Id)
                .Index(t => t.OrderItem_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Orders", "UserID", "dbo.BotUsers");
            DropForeignKey("dbo.OrderOrderItems", "OrderItem_Id", "dbo.OrderItems");
            DropForeignKey("dbo.OrderOrderItems", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "FoodId", "dbo.Foods");
            DropForeignKey("dbo.Foods", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Orders", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Restaurants", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Foods", "FoodCategoryId", "dbo.FoodCategories");
            DropIndex("dbo.OrderOrderItems", new[] { "OrderItem_Id" });
            DropIndex("dbo.OrderOrderItems", new[] { "Order_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Restaurants", new[] { "CategoryId" });
            DropIndex("dbo.Foods", new[] { "RestaurantId" });
            DropIndex("dbo.Foods", new[] { "FoodCategoryId" });
            DropIndex("dbo.OrderItems", new[] { "FoodId" });
            DropIndex("dbo.Orders", new[] { "RestaurantId" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropTable("dbo.OrderOrderItems");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Categories");
            DropTable("dbo.Restaurants");
            DropTable("dbo.FoodCategories");
            DropTable("dbo.Foods");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Orders");
            DropTable("dbo.BotUsers");
        }
    }
}
