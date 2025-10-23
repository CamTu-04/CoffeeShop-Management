namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncSanPhamModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SanPham", "MaDanhMuc", "dbo.DanhMuc");
            DropIndex("dbo.SanPham", new[] { "MaDanhMuc" });
            AlterColumn("dbo.SanPham", "Gia", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.SanPham", "MaDanhMuc", c => c.Int());
            CreateIndex("dbo.SanPham", "MaDanhMuc");
            AddForeignKey("dbo.SanPham", "MaDanhMuc", "dbo.DanhMuc", "MaDanhMuc");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SanPham", "MaDanhMuc", "dbo.DanhMuc");
            DropIndex("dbo.SanPham", new[] { "MaDanhMuc" });
            AlterColumn("dbo.SanPham", "MaDanhMuc", c => c.Int(nullable: false));
            AlterColumn("dbo.SanPham", "Gia", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.SanPham", "MaDanhMuc");
            AddForeignKey("dbo.SanPham", "MaDanhMuc", "dbo.DanhMuc", "MaDanhMuc", cascadeDelete: true);
        }
    }
}
