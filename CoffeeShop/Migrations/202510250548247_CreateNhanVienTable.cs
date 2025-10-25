namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateNhanVienTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NhanVien",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MaNhanVien = c.String(),
                    HoTen = c.String(),
                    GioiTinh = c.String(),
                    DiaChi = c.String(),
                    SoDienThoai = c.String(),
                    CCCD = c.String(),
                    ChucVu = c.String(),
                    TenDangNhap = c.String(),
                    MatKhau = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.NhanVien");
        }
    }
}
