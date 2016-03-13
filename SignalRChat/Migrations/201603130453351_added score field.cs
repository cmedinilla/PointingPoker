namespace SignalRChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedscorefield : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        ConnectionId = c.String(),
                        RoomId = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
