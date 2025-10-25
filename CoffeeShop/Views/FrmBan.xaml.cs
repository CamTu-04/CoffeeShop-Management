using CoffeeShop.Data;
using CoffeeShop.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
                    // --- Tạo layout hiển thị bàn ---
                    var border = new Border
                    {
                        Width = 120,
                        Height = 120,
                        Margin = new Thickness(8),
                        CornerRadius = new CornerRadius(12),
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(1),
                        Background = Brushes.White,
                        Effect = new System.Windows.Media.Effects.DropShadowEffect
                        {
                            Color = Colors.Black,
                            Direction = 320,
                            ShadowDepth = 4,
                            Opacity = 0.3
                        }
                    };

                    var stack = new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    // Tên bàn
                    var tenBan = new TextBlock
                    {
                        Text = ban.TenBan.ToUpper(),
                        FontWeight = FontWeights.Bold,
                        FontSize = 16,
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(0, 8, 0, 4)
                    };

                    // Trạng thái bàn
                    var trangThaiText = new TextBlock
                    {
                        FontSize = 13,
                        TextAlignment = TextAlignment.Center,
                        FontWeight = FontWeights.SemiBold,
                        Margin = new Thickness(0, 4, 0, 0)
                    };

                    // --- Gán màu và trạng thái ---
                    string trangThai = ban.TrangThai?.Trim().ToLower();

                    if (trangThai == "trống")
                    {
                        border.Background = new SolidColorBrush(Color.FromRgb(204, 255, 204)); // xanh nhạt
                        trangThaiText.Text = "Trống";
                        trangThaiText.Foreground = Brushes.Green;
                    }
                    else if (trangThai == "đang phục vụ")
                    {
                        border.Background = new SolidColorBrush(Color.FromRgb(255, 249, 196)); // vàng nhạt
                        trangThaiText.Text = "Đang phục vụ";
                        trangThaiText.Foreground = Brushes.DarkGoldenrod;
                    }
                    else if (trangThai == "đã thanh toán")
                    {
                        border.Background = new SolidColorBrush(Color.FromRgb(255, 204, 204)); // đỏ nhạt
                        trangThaiText.Text = "Đã thanh toán";
                        trangThaiText.Foreground = Brushes.DarkRed;
                    }
                    else
                    {
                        border.Background = Brushes.LightGray;
                        trangThaiText.Text = "Không xác định";
                        trangThaiText.Foreground = Brushes.Gray;
                    }

                    // Nút bấm bao quanh bàn
                    var btn = new Button
                    {
                        Content = stack,
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent,
                        Cursor = System.Windows.Input.Cursors.Hand,
                        Tag = ban.Id
                    };

                    // Thêm text vào StackPanel
                    stack.Children.Add(tenBan);
                    stack.Children.Add(trangThaiText);

                    // Gán sự kiện click
                    btn.Click += Btn_Click;

                    // Thêm vào Border
                    border.Child = btn;
                    wrapPanelBan.Children.Add(border);
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int idBan = (int)btn.Tag;

            FrmHoaDon frm = new FrmHoaDon(idBan);
            frm.Closed += (s, args) =>
            {
                // Khi đóng (tạm ẩn hoặc thanh toán), load lại trạng thái bàn
                LoadBan();
            };
            frm.Show();
        }
    }
}
