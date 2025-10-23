using CoffeeShop.Data;
using CoffeeShop.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeShop.Views.Admin
{
    public partial class DanhMucWindow : Window
    {
        private readonly CoffeeShopContext _context;
        private DanhMuc _selectedDanhMuc;

        public DanhMucWindow()
        {
            InitializeComponent();

            try
            {
                _context = new CoffeeShopContext();
                LoadDanhMuc();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở danh mục:\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                // Tạm thời không đóng cửa sổ để dễ debug
                // this.Close();
            }
        }

        private void LoadDanhMuc()
        {
            try
            {
                var danhMucs = _context.DanhMucs
                    .OrderBy(dm => dm.MaDanhMuc)
                    .ToList()
                    .Select((dm, index) => new
                    {
                        STT = index + 1,
                        MaDanhMuc = dm.MaDanhMuc,
                        TenDanhMuc = dm.TenDanhMuc
                    })
                    .ToList();

                dgDanhMuc.ItemsSource = danhMucs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh mục:\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ten = txtTenDanhMuc.Text.Trim();

                if (string.IsNullOrEmpty(ten))
                {
                    MessageBox.Show("Vui lòng nhập tên danh mục!");
                    return;
                }

                if (_context.DanhMucs.Any(x => x.TenDanhMuc == ten))
                {
                    MessageBox.Show("Danh mục này đã tồn tại!");
                    return;
                }

                var newDanhMuc = new DanhMuc { TenDanhMuc = ten };
                _context.DanhMucs.Add(newDanhMuc);
                _context.SaveChanges();

                LoadDanhMuc();
                txtTenDanhMuc.Clear();
                MessageBox.Show("Đã thêm danh mục thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm danh mục:\n" + ex.Message);
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedDanhMuc == null)
                {
                    MessageBox.Show("Hãy chọn danh mục cần sửa!");
                    return;
                }

                string newName = txtTenDanhMuc.Text.Trim();
                if (string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show("Tên danh mục không được để trống!");
                    return;
                }

                var dm = _context.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == _selectedDanhMuc.MaDanhMuc);
                if (dm != null)
                {
                    dm.TenDanhMuc = newName;
                    _context.SaveChanges();
                    LoadDanhMuc();
                    txtTenDanhMuc.Clear();
                    MessageBox.Show("Đã cập nhật danh mục!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa danh mục:\n" + ex.Message);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedDanhMuc == null)
                {
                    MessageBox.Show("Hãy chọn danh mục cần xóa!");
                    return;
                }

                var dm = _context.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == _selectedDanhMuc.MaDanhMuc);
                if (dm != null)
                {
                    if (MessageBox.Show("Bạn có chắc muốn xóa danh mục này?", "Xác nhận",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        _context.DanhMucs.Remove(dm);
                        _context.SaveChanges();
                        LoadDanhMuc();
                        txtTenDanhMuc.Clear();
                        MessageBox.Show("Đã xóa danh mục thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa danh mục:\n" + ex.Message);
            }
        }

        private void dgDanhMuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = dgDanhMuc.SelectedItem;
                if (selectedItem != null)
                {
                    var maProp = selectedItem.GetType().GetProperty("MaDanhMuc");
                    var tenProp = selectedItem.GetType().GetProperty("TenDanhMuc");

                    if (maProp != null && tenProp != null)
                    {
                        int ma = (int)maProp.GetValue(selectedItem);
                        string ten = tenProp.GetValue(selectedItem)?.ToString();

                        _selectedDanhMuc = new DanhMuc
                        {
                            MaDanhMuc = ma,
                            TenDanhMuc = ten
                        };

                        txtTenDanhMuc.Text = ten;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn danh mục:\n" + ex.Message);
            }
        }

        private void dgDanhMuc_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            _context.Dispose();
            this.Close();
        }
    }
}
