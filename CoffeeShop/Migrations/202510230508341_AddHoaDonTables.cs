namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHoaDonTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietHoaDon",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdHoaDon = c.Int(nullable: false),
                        IdSanPham = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HoaDon",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThoiGian = c.DateTime(nullable: false),
                        IdBan = c.Int(nullable: false),
                        TongTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TrangThai = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HoaDon");
            DropTable("dbo.ChiTietHoaDon");
        }
    }
}
