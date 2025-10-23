using CoffeeShop.Data;
using CoffeeShop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeShop.Views.Admin
{
    public partial class SanPhamWindow : Window
    {
        private readonly CoffeeShopContext _context;
        private SanPham _selectedSanPham;

        public SanPhamWindow()
        {
            InitializeComponent();

            try
            {
                _context = new CoffeeShopContext();
                LoadDanhMuc();
                LoadSanPham();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở cửa sổ sản phẩm:\n" + ex.ToString(),
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDanhMuc()
        {
            cbDanhMuc.ItemsSource = _context.DanhMucs.OrderBy(d => d.TenDanhMuc).ToList();
        }

        private void LoadSanPham()
        {
            var sanPhams = _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .OrderBy(sp => sp.MaSanPham)
                .ToList()
                .Select((sp, index) => new
                {
                    STT = index + 1,
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    Gia = sp.Gia,
                    TenDanhMuc = sp.DanhMuc?.TenDanhMuc,
                    MoTa = sp.MoTa
                })
                .ToList();

            dgSanPham.ItemsSource = sanPhams;
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ten = txtTenSanPham.Text.Trim();
                string moTa = txtMoTa.Text.Trim();
                decimal gia;

                if (string.IsNullOrEmpty(ten) || !decimal.TryParse(txtGia.Text.Trim(), out gia) || cbDanhMuc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ!");
                    return;
                }

                var danhMuc = cbDanhMuc.SelectedItem as DanhMuc;

                var sp = new SanPham
                {
                    TenSanPham = ten,
                    Gia = gia,
                    MoTa = moTa,
                    MaDanhMuc = danhMuc.MaDanhMuc
                };

                _context.SanPhams.Add(sp);
                _context.SaveChanges();

                LoadSanPham();
                ClearForm();
                MessageBox.Show("Đã thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sản phẩm:\n" + ex.Message);
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSanPham == null)
                {
                    MessageBox.Show("Hãy chọn sản phẩm cần sửa!");
                    return;
                }

                string ten = txtTenSanPham.Text.Trim();
                string moTa = txtMoTa.Text.Trim();
                decimal gia;

                if (string.IsNullOrEmpty(ten) || !decimal.TryParse(txtGia.Text.Trim(), out gia) || cbDanhMuc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ!");
                    return;
                }

                var danhMuc = cbDanhMuc.SelectedItem as DanhMuc;
                var sp = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == _selectedSanPham.MaSanPham);

                if (sp != null)
                {
                    sp.TenSanPham = ten;
                    sp.Gia = gia;
                    sp.MoTa = moTa;
                    sp.MaDanhMuc = danhMuc.MaDanhMuc;

                    _context.SaveChanges();
                    LoadSanPham();
                    ClearForm();
                    MessageBox.Show("Đã cập nhật sản phẩm!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa sản phẩm:\n" + ex.Message);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSanPham == null)
                {
                    MessageBox.Show("Hãy chọn sản phẩm cần xóa!");
                    return;
                }

                var sp = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == _selectedSanPham.MaSanPham);
                if (sp != null)
                {
                    if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?", "Xác nhận",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        _context.SanPhams.Remove(sp);
                        _context.SaveChanges();
                        LoadSanPham();
                        ClearForm();
                        MessageBox.Show("Đã xóa sản phẩm thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sản phẩm:\n" + ex.Message);
            }
        }

        private void dgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = dgSanPham.SelectedItem;
            if (selectedItem != null)
            {
                var maProp = selectedItem.GetType().GetProperty("MaSanPham");
                if (maProp != null)
                {
                    int ma = (int)maProp.GetValue(selectedItem);
                    var sp = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == ma);
                    if (sp != null)
                    {
                        _selectedSanPham = sp;
                        txtTenSanPham.Text = sp.TenSanPham;
                        txtGia.Text = sp.Gia.ToString();
                        txtMoTa.Text = sp.MoTa;
                        cbDanhMuc.SelectedValue = sp.MaDanhMuc;
                    }
                }
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            _context.Dispose();
            this.Close();
        }

        private void ClearForm()
        {
            txtTenSanPham.Clear();
            txtGia.Clear();
            txtMoTa.Clear();
            cbDanhMuc.SelectedIndex = -1;
            _selectedSanPham = null;
        }
    }
}
