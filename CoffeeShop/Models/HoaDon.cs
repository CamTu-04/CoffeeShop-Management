using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public int Id { get; set; }

        public DateTime ThoiGian { get; set; }

        public int IdBan { get; set; }

        public decimal TongTien { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }
    }
}
