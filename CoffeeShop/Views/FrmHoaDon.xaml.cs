// 1. THAY THẾ "CoffeeShop" BẰNG TÊN PROJECT CỦA BẠN
using CoffeeShop.Models; // Giả sử các Model nằm ở đây
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CoffeeShop.Views
{
    /// <summary>
    /// Interaction logic for FrmHoaDon.xaml
    /// </summary>
    public partial class FrmHoaDon : Window
    {
        // 2. THAY THẾ "CoffeeShopContext" BẰNG TÊN CONTEXT CỦA BẠN
        private readonly CoffeeShopContext _context;
        private string _placeholderText = "Tìm kiếm theo mã HĐ, tên nhân viên...";

        public FrmHoaDon()
        {
            InitializeComponent();
            _context = new CoffeeShopContext(); // Khởi tạo context
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Tải dữ liệu chính
        private void LoadData()
        {
            try
            {
                // 3. THAY THẾ CÁC TÊN DBSET "HoaDons", "NhanVien", "KhachHang", "ChiTietHoaDons", "SanPham"
                var hoaDonList = _context.HoaDons
                    .Include(hd => hd.NhanVien)      // Tải thông tin Nhân viên
                    .Include(hd => hd.KhachHang)     // Tải thông tin Khách hàng
                    .Include(hd => hd.ChiTietHoaDons) // Tải danh sách Chi tiết
                        .ThenInclude(ct => ct.SanPham) // Trong Chi tiết, tải luôn thông tin Sản phẩm
                    .AsNoTracking()
                    .OrderByDescending(hd => hd.NgayLap)
                    .ToList();

                dgHoaDon.ItemsSource = hoaDonList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Lọc dữ liệu (kết hợp cả tìm kiếm và chọn ngày)
        private void FilterData()
        {
            try
            {
                string searchText = txtTimKiem.Text.ToLower().Trim();
                DateTime? selectedDate = dpLocNgay.SelectedDate;

                // 4. THAY THẾ TÊN DBSET "HoaDons" VÀ CÁC MODEL
                var query = _context.HoaDons
                    .Include(hd => hd.NhanVien)
                    .Include(hd => hd.KhachHang)
                    .Include(hd => hd.ChiTietHoaDons)
                        .ThenInclude(ct => ct.SanPham)
                    .AsNoTracking();

                // Lọc theo text tìm kiếm
                if (!string.IsNullOrEmpty(searchText) && searchText != _placeholderText.ToLower())
                {
                    query = query.Where(hd =>
                        hd.MaHoaDon.ToLower().Contains(searchText) ||
                        (hd.NhanVien != null && hd.NhanVien.TenNV.ToLower().Contains(searchText)) ||
                        (hd.KhachHang != null && hd.KhachHang.TenKH.ToLower().Contains(searchText))
                    );
                }

                // Lọc theo ngày
                if (selectedDate.HasValue)
                {
                    query = query.Where(hd => hd.NgayLap.Date == selectedDate.Value.Date);
                }

                dgHoaDon.ItemsSource = query.OrderByDescending(hd => hd.NgayLap).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lọc dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Xử lý Sự kiện

        // Khi chọn một hóa đơn, bật nút In
        private void dgHoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnInHoaDon.IsEnabled = (dgHoaDon.SelectedItem != null);

            // Ghi chú: Việc hiển thị chi tiết (bên phải) được xử lý tự động
            // bằng Data Binding trong XAML, bao gồm cả bảng Chi tiết sản phẩm.
        }

        // Mở form Bán hàng để tạo Hóa đơn mới
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            // 5. THAY THẾ "FrmBanHang" BẰNG TÊN FORM BÁN HÀNG CỦA BẠN
            // Thông thường, Hóa đơn được tạo từ một form Bán hàng (POS)
            var frmBanHang = new FrmBanHang(); // Giả sử bạn có form FrmBanHang
            frmBanHang.ShowDialog();

            // Sau khi form Bán hàng đóng, tải lại danh sách hóa đơn
            LoadData();
        }

        // Xóa hóa đơn
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            // 6. THAY THẾ "HoaDon"
            if (dgHoaDon.SelectedItem is HoaDon selectedHoaDon)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa Hóa đơn '{selectedHoaDon.MaHoaDon}' không? " +
                                             "Thao tác này không thể hoàn tác.",
                                             "Xác nhận xóa",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // 7. THAY THẾ TÊN DBSET "HoaDons", "ChiTietHoaDons"

                        // Phải tìm lại Hóa đơn trong context để xóa
                        var hoaDonToDelete = _context.HoaDons
                            .Include(hd => hd.ChiTietHoaDons) // Phải xóa các chi tiết trước
                            .SingleOrDefault(hd => hd.MaHoaDon == selectedHoaDon.MaHoaDon);

                        if (hoaDonToDelete != null)
                        {
                            // Xóa các chi tiết (dòng con)
                            _context.ChiTietHoaDons.RemoveRange(hoaDonToDelete.ChiTietHoaDons);

                            // Xóa hóa đơn (dòng cha)
                            _context.HoaDons.Remove(hoaDonToDelete);

                            _context.SaveChanges();

                            MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        MessageBox.Show("Không thể xóa hóa đơn này. Có thể có dữ liệu liên quan khác.\nChi tiết: " + ex.InnerException?.Message, "Lỗi ràng buộc", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Tải lại dữ liệu
        private void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            txtTimKiem.Text = _placeholderText;
            txtTimKiem.Foreground = Brushes.Gray;
            dpLocNgay.SelectedDate = null;
            LoadData();
        }

        // Xử lý in (hiện tại chỉ là thông báo)
        private void btnInHoaDon_Click(object sender, RoutedEventArgs e)
        {
            if (dgHoaDon.SelectedItem is HoaDon selectedHoaDon)
            {
                MessageBox.Show($"Đang chuẩn bị in hóa đơn {selectedHoaDon.MaHoaDon}...", "Thông báo in", MessageBoxButton.OK, MessageBoxImage.Information);
                // Đây là nơi bạn sẽ thêm logic in ấn thực tế
            }
        }

        // Lọc khi gõ text
        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) // Chỉ lọc khi window đã load xong
            {
                FilterData();
            }
        }

        // Lọc khi chọn ngày
        private void dpLocNgay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                FilterData();
            }
        }

        #endregion

        #region Xử lý Placeholder cho ô tìm kiếm

        private void txtTimKiem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiem.Text == _placeholderText)
            {
                txtTimKiem.Text = "";
                txtTimKiem.Foreground = Brushes.Black;
            }
        }

        private void txtTimKiem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = _placeholderText;
                txtTimKiem.Foreground = Brushes.Gray;
            }
        }

        #endregion
    }
}