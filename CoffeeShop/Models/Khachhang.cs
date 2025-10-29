using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Thư viện cho các DataAnnotations
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Đảm bảo namespace khớp với dự án của bạn
namespace CoffeeShop.Models
{
    public class KhachHang
    {
        // Constructor để khởi tạo danh sách Hóa đơn, tránh lỗi null
        public KhachHang()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        [Key] // Đánh dấu đây là Khóa chính (Primary Key)
        [StringLength(20)] // Giới hạn độ dài mã khách hàng, ví dụ: "KH0001"
        public string MaKhachHang { get; set; }

        [Required(ErrorMessage = "Tên khách hàng là bắt buộc")] // Bắt buộc phải có
        [StringLength(100)]
        public string TenKhachHang { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; } // Địa chỉ có thể không bắt buộc

        [StringLength(100)]
        public string Email { get; set; } // Email có thể không bắt buộc

        public int DiemTichLuy { get; set; } // Dùng cho chương trình khách hàng thân thiết

        // --- Thiết lập quan hệ ---
        // Một Khách Hàng có thể có nhiều Hóa Đơn
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}