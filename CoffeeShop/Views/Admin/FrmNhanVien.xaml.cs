using CoffeeShop.Data;
using CoffeeShop.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeShop.Views.Admin
{
    public partial class FrmNhanVien : Window
    {
        private NhanVien selectedNhanVien;

        public FrmNhanVien()
        {
            InitializeComponent();
            LoadNhanVien();
        }

        private void LoadNhanVien()
        {
            try
            {
                using (var db = new CoffeeShopContext())
                {
                    var danhSach = db.NhanViens.ToList();
                    dgNhanVien.ItemsSource = danhSach;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên:\n" + (ex.InnerException?.Message ?? ex.Message),
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            cbGioiTinh.SelectedIndex = -1;
            txtDiaChi.Text = "";
            txtSDT.Text = "";
            txtCCCD.Text = "";
            cbChucVu.SelectedIndex = -1;
            txtTenDangNhap.Text = "";
            txtMatKhau.Password = "";
            selectedNhanVien = null;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new CoffeeShopContext())
                {
                    var nv = new NhanVien
                    {
                        MaNhanVien = txtMaNV.Text.Trim(),
                        HoTen = txtHoTen.Text.Trim(),
                        GioiTinh = (cbGioiTinh.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        DiaChi = txtDiaChi.Text.Trim(),
                        SoDienThoai = txtSDT.Text.Trim(),
                        CCCD = txtCCCD.Text.Trim(),
                        ChucVu = (cbChucVu.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        MatKhau = txtMatKhau.Password.Trim()
                    };

                    db.NhanViens.Add(nv);
                    db.SaveChanges();
                    MessageBox.Show("Thêm nhân viên thành công!");
                    LoadNhanVien();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên:\n" + (ex.InnerException?.Message ?? ex.Message),
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (selectedNhanVien == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa.");
                return;
            }

            try
            {
                using (var db = new CoffeeShopContext())
                {
                    var nv = db.NhanViens.Find(selectedNhanVien.Id);
                    if (nv != null)
                    {
                        nv.MaNhanVien = txtMaNV.Text.Trim();
                        nv.HoTen = txtHoTen.Text.Trim();
                        nv.GioiTinh = (cbGioiTinh.SelectedItem as ComboBoxItem)?.Content.ToString();
                        nv.DiaChi = txtDiaChi.Text.Trim();
                        nv.SoDienThoai = txtSDT.Text.Trim();
                        nv.CCCD = txtCCCD.Text.Trim();
                        nv.ChucVu = (cbChucVu.SelectedItem as ComboBoxItem)?.Content.ToString();
                        nv.TenDangNhap = txtTenDangNhap.Text.Trim();
                        nv.MatKhau = txtMatKhau.Password.Trim();

                        db.SaveChanges();
                        MessageBox.Show("Cập nhật nhân viên thành công!");
                        LoadNhanVien();
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật nhân viên:\n" + (ex.InnerException?.Message ?? ex.Message),
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (selectedNhanVien == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận",
                                         MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new CoffeeShopContext())
                    {
                        var nv = db.NhanViens.Find(selectedNhanVien.Id);
                        if (nv != null)
                        {
                            db.NhanViens.Remove(nv);
                            db.SaveChanges();
                            MessageBox.Show("Xóa nhân viên thành công!");
                            LoadNhanVien();
                            ClearForm();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhân viên:\n" + (ex.InnerException?.Message ?? ex.Message),
                                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void dgNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNhanVien = dgNhanVien.SelectedItem as NhanVien;
            if (selectedNhanVien != null)
            {
                txtMaNV.Text = selectedNhanVien.MaNhanVien;
                txtHoTen.Text = selectedNhanVien.HoTen;
                cbGioiTinh.Text = selectedNhanVien.GioiTinh;
                txtDiaChi.Text = selectedNhanVien.DiaChi;
                txtSDT.Text = selectedNhanVien.SoDienThoai;
                txtCCCD.Text = selectedNhanVien.CCCD;
                cbChucVu.Text = selectedNhanVien.ChucVu;
                txtTenDangNhap.Text = selectedNhanVien.TenDangNhap;
                txtMatKhau.Password = selectedNhanVien.MatKhau;
            }
        }
    }
}
