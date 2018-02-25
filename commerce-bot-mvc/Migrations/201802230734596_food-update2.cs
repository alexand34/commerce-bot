namespace commerce_bot_mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foodupdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Foods", "RestaurantId_Id", "dbo.Restaurants");
            DropIndex("dbo.Foods", new[] { "RestaurantId_Id" });
            RenameColumn(table: "dbo.Foods", name: "RestaurantId_Id", newName: "RestaurantId");
            AlterColumn("dbo.Foods", "RestaurantId", c => c.Int(nullable: false));
            CreateIndex("dbo.Foods", "RestaurantId");
            AddForeignKey("dbo.Foods", "RestaurantId", "dbo.Restaurants", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Foods", "RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Foods", new[] { "RestaurantId" });
            AlterColumn("dbo.Foods", "RestaurantId", c => c.Int());
            RenameColumn(table: "dbo.Foods", name: "RestaurantId", newName: "RestaurantId_Id");
            CreateIndex("dbo.Foods", "RestaurantId_Id");
            AddForeignKey("dbo.Foods", "RestaurantId_Id", "dbo.Restaurants", "Id");
        }
    }
}
