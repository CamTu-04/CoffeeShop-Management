using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDanhMuc { get; set; }

        public string MoTa { get; set; }

        // Quan hệ 1 - N với SanPham
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
