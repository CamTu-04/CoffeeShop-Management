using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        [Column("Id")] // ánh xạ đúng với cột trong SQL
        public int MaSanPham { get; set; }

        [Required]
        [StringLength(100)]
        public string TenSanPham { get; set; }

        public decimal? Gia { get; set; } // nullable vì cột cho phép null

        public int? MaDanhMuc { get; set; } // nullable vì cột cho phép null

        [ForeignKey("MaDanhMuc")]
        public virtual DanhMuc DanhMuc { get; set; }

        public string MoTa { get; set; } // không có trong bảng hiện tại, có thể bỏ hoặc thêm vào DB
    }
}
