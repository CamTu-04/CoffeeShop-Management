using System.Linq;
using CoffeeShop.Data;
using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public class AuthService
    {
        public static NhanVien Login(string username, string password, string requiredRole)
        {
            using (var context = new CoffeeShopContext())
            {
                return context.NhanViens
                    .FirstOrDefault(nv => nv.TenDangNhap == username &&
                                          nv.MatKhau == password &&
                                          nv.ChucVu == requiredRole);
            }
        }
    }
}
