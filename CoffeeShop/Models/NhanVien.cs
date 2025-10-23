using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class NhanVien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TenDangNhap { get; set; }

        [Required]
        public string MatKhau { get; set; }

        [Required]
        public string HoTen { get; set; }

        [Required]
        public string ChucVu { get; set; } // "PhucVu" hoặc "Admin"
    }
}
