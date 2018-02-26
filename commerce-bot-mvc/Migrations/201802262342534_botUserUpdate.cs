namespace commerce_bot_mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class botUserUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotUsers", "toId", c => c.String());
            AddColumn("dbo.BotUsers", "toName", c => c.String());
            AddColumn("dbo.BotUsers", "fromId", c => c.String());
            AddColumn("dbo.BotUsers", "fromName", c => c.String());
            AddColumn("dbo.BotUsers", "serviceUrl", c => c.String());
            AddColumn("dbo.BotUsers", "channelId", c => c.String());
            AddColumn("dbo.BotUsers", "conversationId", c => c.String());
            AddColumn("dbo.Orders", "OrderState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderState");
            DropColumn("dbo.BotUsers", "conversationId");
            DropColumn("dbo.BotUsers", "channelId");
            DropColumn("dbo.BotUsers", "serviceUrl");
            DropColumn("dbo.BotUsers", "fromName");
            DropColumn("dbo.BotUsers", "fromId");
            DropColumn("dbo.BotUsers", "toName");
            DropColumn("dbo.BotUsers", "toId");
        }
    }
}
