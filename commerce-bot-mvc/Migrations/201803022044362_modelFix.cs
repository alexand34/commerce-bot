namespace commerce_bot_mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderOrderItems", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderOrderItems", "OrderItem_Id", "dbo.OrderItems");
            DropIndex("dbo.OrderOrderItems", new[] { "Order_Id" });
            DropIndex("dbo.OrderOrderItems", new[] { "OrderItem_Id" });
            AddColumn("dbo.OrderItems", "Order_Id", c => c.Int());
            CreateIndex("dbo.OrderItems", "Order_Id");
            AddForeignKey("dbo.OrderItems", "Order_Id", "dbo.Orders", "Id");
            DropTable("dbo.OrderOrderItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderOrderItems",
                c => new
                    {
                        Order_Id = c.Int(nullable: false),
                        OrderItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_Id, t.OrderItem_Id });
            
            DropForeignKey("dbo.OrderItems", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "Order_Id" });
            DropColumn("dbo.OrderItems", "Order_Id");
            CreateIndex("dbo.OrderOrderItems", "OrderItem_Id");
            CreateIndex("dbo.OrderOrderItems", "Order_Id");
            AddForeignKey("dbo.OrderOrderItems", "OrderItem_Id", "dbo.OrderItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderOrderItems", "Order_Id", "dbo.Orders", "Id", cascadeDelete: true);
        }
    }
}
