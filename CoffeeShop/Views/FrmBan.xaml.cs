using CoffeeShop.Data;
using CoffeeShop.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CoffeeShop.Views
{
    public partial class FrmBan : UserControl
    {
        public FrmBan()
        {
            InitializeComponent();
            LoadBan();
        }

        private void LoadBan()
        {
            wrapPanelBan.Children.Clear();

            using (var db = new CoffeeShopContext())
            {
                var danhSachBan = db.Bans.ToList();
                foreach (var ban in danhSachBan)
                {
                    Button btn = new Button();
                    btn.Content = ban.TenBan;
                    btn.Tag = ban.Id;
                    btn.Width = 100;
                    btn.Height = 100;
                    btn.Margin = new Thickness(5);
                    btn.FontWeight = FontWeights.Bold;

                    // ✅ Chuẩn hóa chuỗi trạng thái để so sánh chính xác
                    string trangThai = ban.TrangThai?.Trim().ToLower();

                    if (trangThai == "trống")
                    {
                        btn.Background = Brushes.LightGreen;
                    }
                    else if (trangThai == "đang phục vụ")
                    {
                        btn.Background = Brushes.Gold;
                    }
                    else if (trangThai == "đã thanh toán")
                    {
                        btn.Background = Brushes.IndianRed;
                    }
                    else
                    {
                        btn.Background = Brushes.LightGray; // ✅ trạng thái không xác định
                    }

                    btn.Click += Btn_Click;
                    wrapPanelBan.Children.Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int idBan = (int)btn.Tag;

            FrmHoaDon frm = new FrmHoaDon(idBan);
            bool? result = frm.ShowDialog();

            if (result == true)
            {
                LoadBan(); // ✅ cập nhật lại trạng thái bàn sau khi hóa đơn đóng
            }
        }
    }
}
