using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("Ban")]
    public class Ban
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TenBan { get; set; }

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; } // Trống / Đang phục vụ / Đã thanh toán
    }
}
