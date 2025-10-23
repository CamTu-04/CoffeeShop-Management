using CoffeeShop.Data;
using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeShop.Views
{
    public partial class FrmHoaDon : Window
    {
        private int idBan;
        private HoaDon hoaDon;
        private List<ChiTietHoaDonView> chiTietTam = new List<ChiTietHoaDonView>();

        public FrmHoaDon(int idBan)
        {
            InitializeComponent();
            this.idBan = idBan;
            LoadDanhMuc();
            LoadBanVaHoaDon();
        }

        private void LoadBanVaHoaDon()
        {
            using (var db = new CoffeeShopContext())
            {
                var ban = db.Bans.Find(idBan);
                txtTenBan.Text = $"Bàn: {ban.TenBan}";

                if (ban.TrangThai == "Trống")
                {
                    hoaDon = new HoaDon
                    {
                        IdBan = idBan,
                        ThoiGian = DateTime.Now,
                        TrangThai = "Đang mở",
                        TongTien = 0
                    };
                    db.HoaDons.Add(hoaDon);
                    ban.TrangThai = "Đang phục vụ";
                    db.SaveChanges();
                }
                else
                {
                    hoaDon = db.HoaDons.FirstOrDefault(h => h.IdBan == idBan && h.TrangThai == "Đang mở");
                }
            }
        }

        private void LoadDanhMuc()
        {
            using (var db = new CoffeeShopContext())
            {
                cbDanhMuc.ItemsSource = db.DanhMucs.ToList();
                cbDanhMuc.DisplayMemberPath = "TenDanhMuc";
            }
        }

        private void cbDanhMuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var danhMuc = cbDanhMuc.SelectedItem as DanhMuc;
            if (danhMuc != null)
            {
                using (var db = new CoffeeShopContext())
                {
                    lstSanPham.ItemsSource = db.SanPhams
                        .Where(s => s.MaDanhMuc == danhMuc.MaDanhMuc)
                        .ToList();
                }
            }
        }

        private void lstSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sanPham = lstSanPham.SelectedItem as SanPham;
            if (sanPham != null)
            {
                var chiTiet = chiTietTam.FirstOrDefault(c => c.IdSanPham == sanPham.MaSanPham);
                if (chiTiet != null)
                {
                    chiTiet.SoLuong++;
                }
                else
                {
                    chiTietTam.Add(new ChiTietHoaDonView
                    {
                        IdSanPham = sanPham.MaSanPham,
                        TenSanPham = sanPham.TenSanPham,
                        SoLuong = 1,
                        DonGia = sanPham.Gia ?? 0
                    });
                }

                CapNhatTongTien();
                lvChiTietHoaDon.ItemsSource = null;
                lvChiTietHoaDon.ItemsSource = chiTietTam;
            }
        }

        private void CapNhatTongTien()
        {
            decimal tong = chiTietTam.Sum(c => c.ThanhTien);
            txtTongTien.Text = $"{tong:N0} đ";
        }

        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new CoffeeShopContext())
            {
                var hd = db.HoaDons.Find(hoaDon.Id);
                hd.TongTien = chiTietTam.Sum(c => c.ThanhTien);
                hd.TrangThai = "Đã thanh toán";

                foreach (var item in chiTietTam)
                {
                    db.ChiTietHoaDons.Add(new ChiTietHoaDon
                    {
                        IdHoaDon = hd.Id,
                        IdSanPham = item.IdSanPham,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    });
                }

                var ban = db.Bans.Find(idBan);
                ban.TrangThai = "Trống"; // ✅ chuyển về trạng thái trống

                db.SaveChanges();
                MessageBox.Show("Thanh toán thành công!");

                this.DialogResult = true; // ✅ báo cho FrmBan biết là đã thanh toán
                this.Close(); // ✅ chỉ đóng cửa sổ gọi món
            }
        }
    }

    public class ChiTietHoaDonView
    {
        public int IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        public decimal ThanhTien => DonGia * SoLuong;
    }
}
