namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBanTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ban",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenBan = c.String(nullable: false, maxLength: 50),
                        TrangThai = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ban");
        }
    }
}
