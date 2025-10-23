using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("ChiTietHoaDon")]
    public class ChiTietHoaDon
    {
        [Key]
        public int Id { get; set; }

        public int IdHoaDon { get; set; }

        public int IdSanPham { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }
    }
}
