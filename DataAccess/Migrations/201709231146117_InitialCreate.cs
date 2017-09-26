namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameLobbies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LobbyName = c.String(),
                        Password = c.String(),
                        Creator = c.String(),
                        CreatorElo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Elo = c.Int(nullable: false),
                        CurrentConnectionId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlayerModels");
            DropTable("dbo.GameLobbies");
        }
    }
}
