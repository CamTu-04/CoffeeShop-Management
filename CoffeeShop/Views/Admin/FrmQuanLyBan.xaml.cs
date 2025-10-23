using CoffeeShop.Data;
using CoffeeShop.Models;
using System.Linq;
using System.Windows;

namespace CoffeeShop.Views.Admin
{
    public partial class FrmQuanLyBan : Window
    {
        public FrmQuanLyBan()
        {
            InitializeComponent();
            LoadBan();
        }

        private void LoadBan()
        {
            using (var db = new CoffeeShopContext())
            {
                dgBan.ItemsSource = db.Bans.ToList();
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenBan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bàn.");
                return;
            }

            using (var db = new CoffeeShopContext())
            {
                var ban = new Ban
                {
                    TenBan = txtTenBan.Text.Trim(),
                    TrangThai = "Trống"
                };
                db.Bans.Add(ban);
                db.SaveChanges();
            }
            txtTenBan.Clear();
            LoadBan();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            var selectedBan = dgBan.SelectedItem as Ban;
            if (selectedBan == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần sửa.");
                return;
            }

            using (var db = new CoffeeShopContext())
            {
                var ban = db.Bans.Find(selectedBan.Id);
                if (ban != null)
                {
                    ban.TenBan = txtTenBan.Text.Trim();
                    db.SaveChanges();
                }
            }
            txtTenBan.Clear();
            LoadBan();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var selectedBan = dgBan.SelectedItem as Ban;
            if (selectedBan == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa.");
                return;
            }

            using (var db = new CoffeeShopContext())
            {
                var ban = db.Bans.Find(selectedBan.Id);
                if (ban != null)
                {
                    if (ban.TrangThai != "Trống")
                    {
                        MessageBox.Show("Không thể xóa bàn đang phục vụ hoặc đã thanh toán.");
                        return;
                    }

                    db.Bans.Remove(ban);
                    db.SaveChanges();
                }
            }
            txtTenBan.Clear();
            LoadBan();
        }
    }
}
