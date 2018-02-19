namespace commerce_bot_mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mainlogic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restaurants", "AppUserId", c => c.String());
            DropTable("dbo.PizzeriaAccounts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PizzeriaAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Description = c.String(),
                        Rating = c.Double(),
                        AverageReciept = c.Double(),
                        Distance = c.Double(),
                        AppUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Restaurants", "AppUserId");
        }
    }
}
