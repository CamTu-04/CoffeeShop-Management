namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateSanPhamModel : DbMigration
    {
        public override void Up()
        {
            // Không cần đổi tên cột vì đã ánh xạ bằng [Column("Id")] trong model
        }

        public override void Down()
        {
            // Không cần rollback tên cột
        }
    }
}
