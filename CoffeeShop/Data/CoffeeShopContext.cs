using System.Data.Entity; // EF6 vì em dùng .NET 4.8
using CoffeeShop.Models;

namespace CoffeeShop.Data
{
    public class CoffeeShopContext : DbContext
    {
        public CoffeeShopContext() : base("name=CoffeeShopDB") { }

        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
