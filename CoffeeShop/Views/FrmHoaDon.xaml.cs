using CoffeeShop.Data;
using CoffeeShop.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeShop.Views
{
    public partial class FrmHoaDon : Window
    {
        private int idBan;
        private HoaDon hoaDon;
        private ObservableCollection<ChiTietHoaDonView> chiTietTam;

        public FrmHoaDon(int idBan)
        {
            InitializeComponent();
            this.idBan = idBan;
            chiTietTam = new ObservableCollection<ChiTietHoaDonView>();
            lvChiTietHoaDon.ItemsSource = chiTietTam;

            LoadDanhMuc();
            LoadBanVaHoaDon();
        }

        private void LoadBanVaHoaDon()
        {
            using (var db = new CoffeeShopContext())
            {
                var ban = db.Bans.Find(idBan);
                if (ban == null)
                {
                    MessageBox.Show("Không tìm thấy bàn!");
                    Close();
                    return;
                }

                txtTenBan.Text = $"Bàn: {ban.TenBan}";

                // Nếu có hóa đơn đang mở (TrangThai = "Đang mở") thì load nó
                hoaDon = db.HoaDons.FirstOrDefault(h => h.IdBan == idBan && h.TrangThai == "Đang mở");

                if (hoaDon != null)
                {
                    // Load chi tiết hóa đơn đang mở (join để lấy TenSanPham)
                    var chiTietCu = (from ct in db.ChiTietHoaDons
                                     join sp in db.SanPhams on ct.IdSanPham equals sp.MaSanPham
                                     where ct.IdHoaDon == hoaDon.Id
                                     select new ChiTietHoaDonView
                                     {
                                         IdSanPham = ct.IdSanPham,
                                         TenSanPham = sp.TenSanPham,
                                         SoLuong = ct.SoLuong,
                                         DonGia = ct.DonGia
                                     }).ToList();

                    chiTietTam.Clear();
                    foreach (var item in chiTietCu)
                        chiTietTam.Add(item);
                }
                else
                {
                    // Nếu bàn trống thì tạo hóa đơn mới (tạo và lưu vào DB ngay để có Id)
                    hoaDon = new HoaDon
                    {
                        IdBan = idBan,
                        ThoiGian = DateTime.Now,
                        TrangThai = "Đang mở",
                        TongTien = 0
                    };

                    db.HoaDons.Add(hoaDon);
                    ban.TrangThai = "Đang phục vụ";
                    db.SaveChanges(); // -> đảm bảo hoaDon.Id có giá trị
                }

                CapNhatTongTien();
            }
        }

        private void LoadDanhMuc()
        {
            using (var db = new CoffeeShopContext())
            {
                cbDanhMuc.ItemsSource = db.DanhMucs.ToList();
                cbDanhMuc.DisplayMemberPath = "TenDanhMuc";
                if (cbDanhMuc.Items.Count > 0) cbDanhMuc.SelectedIndex = 0;
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
                    lstSanPham.DisplayMemberPath = "TenSanPham";
                }
            }
        }

        // Khi nhấn "Thêm món" -> thêm vào cả in-memory và DB (nếu đã có hoaDon.Id)
        private void btnThemMon_Click(object sender, RoutedEventArgs e)
        {
            var sanPham = lstSanPham.SelectedItem as SanPham;
            if (sanPham == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!");
                return;
            }

            int soLuong = 1;
            int.TryParse(txtSoLuong.Text, out soLuong);
            if (soLuong <= 0) soLuong = 1;

            // cập nhật in-memory collection
            var chiTiet = chiTietTam.FirstOrDefault(c => c.IdSanPham == sanPham.MaSanPham);
            if (chiTiet != null)
            {
                chiTiet.SoLuong += soLuong;
            }
            else
            {
                chiTiet = new ChiTietHoaDonView
                {
                    IdSanPham = sanPham.MaSanPham,
                    TenSanPham = sanPham.TenSanPham,
                    SoLuong = soLuong,
                    DonGia = sanPham.Gia.GetValueOrDefault(0) // phòng trường hợp nullable
                };
                chiTietTam.Add(chiTiet);
            }

            // ghi/ cập nhật vào DB ngay để đảm bảo khi tạm ẩn hoặc chuyển bàn vẫn còn
            using (var db = new CoffeeShopContext())
            {
                // lấy hóa đơn từ DB để tránh conflict
                var hdDb = db.HoaDons.Find(hoaDon.Id);
                if (hdDb == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn (DB). Vui lòng thử lại.");
                    return;
                }

                // tìm chi tiết trong DB
                var ctDb = db.ChiTietHoaDons.FirstOrDefault(c => c.IdHoaDon == hdDb.Id && c.IdSanPham == sanPham.MaSanPham);
                if (ctDb != null)
                {
                    ctDb.SoLuong += soLuong;
                    // giữ DonGia (hoặc cập nhật nếu cần)
                }
                else
                {
                    db.ChiTietHoaDons.Add(new ChiTietHoaDon
                    {
                        IdHoaDon = hdDb.Id,
                        IdSanPham = sanPham.MaSanPham,
                        SoLuong = soLuong,
                        DonGia = sanPham.Gia.GetValueOrDefault(0)
                    });
                }

                // cập nhật tổng tiền hóa đơn trong DB
                hdDb.TongTien = db.ChiTietHoaDons
                                    .Where(x => x.IdHoaDon == hdDb.Id)
                                    .AsEnumerable() // để tính trên client (decimal)
                                    .Sum(x => x.DonGia * x.SoLuong);

                db.SaveChanges();
            }

            lvChiTietHoaDon.Items.Refresh();
            CapNhatTongTien();
        }

        // Xóa món: xóa cả in-memory và DB
        private void btnXoaMon_Click(object sender, RoutedEventArgs e)
        {
            var item = lvChiTietHoaDon.SelectedItem as ChiTietHoaDonView;
            if (item == null)
            {
                MessageBox.Show("Hãy chọn món cần xóa!");
                return;
            }

            if (MessageBox.Show($"Xóa {item.TenSanPham} khỏi hóa đơn?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            // remove in-memory
            chiTietTam.Remove(item);

            // remove in DB
            using (var db = new CoffeeShopContext())
            {
                var ctDb = db.ChiTietHoaDons.FirstOrDefault(c => c.IdHoaDon == hoaDon.Id && c.IdSanPham == item.IdSanPham);
                if (ctDb != null)
                {
                    db.ChiTietHoaDons.Remove(ctDb);
                }

                // cập nhật tổng tiền hóa đơn
                var hdDb = db.HoaDons.Find(hoaDon.Id);
                if (hdDb != null)
                {
                    hdDb.TongTien = db.ChiTietHoaDons
                                     .Where(x => x.IdHoaDon == hdDb.Id)
                                     .AsEnumerable()
                                     .Sum(x => x.DonGia * x.SoLuong);
                }

                db.SaveChanges();
            }

            lvChiTietHoaDon.Items.Refresh();
            CapNhatTongTien();
        }

        private void CapNhatTongTien()
        {
            decimal tong = chiTietTam.Sum(c => c.ThanhTien);
            txtTongTien.Text = $"{tong:N0} đ";
        }

        // Thanh toán: đánh dấu hóa đơn "Đã thanh toán" và đóng
        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            if (chiTietTam.Count == 0)
            {
                MessageBox.Show("Hóa đơn trống! Vui lòng chọn món trước khi thanh toán.");
                return;
            }

            using (var db = new CoffeeShopContext())
            {
                var hd = db.HoaDons.Find(hoaDon.Id);
                if (hd == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn trong cơ sở dữ liệu!");
                    return;
                }

                hd.TongTien = chiTietTam.Sum(c => c.ThanhTien);
                hd.TrangThai = "Đã thanh toán";

                // (Chú ý) Chi tiết đã được lưu từng bước khi thêm/xóa, nên ở đây chỉ cần đảm bảo DB đúng
                // cập nhật lại tổng tiền (just in case)
                db.SaveChanges();

                var ban = db.Bans.Find(idBan);
                if (ban != null)
                {
                    ban.TrangThai = "Trống";
                }

                db.SaveChanges();
            }

            MessageBox.Show("Thanh toán thành công!");
            this.Close();
        }

        // Tạm ẩn: chỉ ẩn cửa sổ, dữ liệu đã nằm ở DB nên khi mở lại sẽ load
        private void btnTamAn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }

    // Lớp hỗ trợ hiển thị chi tiết hóa đơn (không ảnh hưởng model thật)
    public class ChiTietHoaDonView
    {
        public int IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => DonGia * SoLuong;
    }
}
