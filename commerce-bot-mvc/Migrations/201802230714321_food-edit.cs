namespace commerce_bot_mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foodedit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foods", "RestaurantId_Id", c => c.Int());
            CreateIndex("dbo.Foods", "RestaurantId_Id");
            AddForeignKey("dbo.Foods", "RestaurantId_Id", "dbo.Restaurants", "Id");
            DropColumn("dbo.Foods", "RestaurantId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Foods", "RestaurantId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Foods", "RestaurantId_Id", "dbo.Restaurants");
            DropIndex("dbo.Foods", new[] { "RestaurantId_Id" });
            DropColumn("dbo.Foods", "RestaurantId_Id");
        }
    }
}
