using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoffeeShop.Views
{
    public partial class FrmBan : UserControl
    {
        public class ChiTietHoaDon
        {
            public string TenMon { get; set; }
            public int SoLuong { get; set; }
            public double Gia { get; set; }
            public double ThanhTien => SoLuong * Gia;
        }

        ObservableCollection<ChiTietHoaDon> dsHoaDon = new();

        public FrmBan()
        {
            InitializeComponent();
            dgHoaDon.ItemsSource = dsHoaDon;
            TaoDanhSachBan();
            TaoDanhSachMon();
        }

        void TaoDanhSachBan()
        {
            for (int i = 1; i <= 8; i++)
            {
                Button btn = new Button()
                {
                    Content = $"Bàn {i}",
                    Width = 90,
                    Height = 70,
                    Margin = new Thickness(5),
                    Background = Brushes.MediumSeaGreen,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                btn.Click += BtnBan_Click;
                wrapPanelBan.Children.Add(btn);
            }
        }

        void BtnBan_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            txtBanHienTai.Text = $"Bàn: {btn.Content}";
            dsHoaDon.Clear(); // reset bill khi chọn bàn mới
            CapNhatTongTien();
        }

        void TaoDanhSachMon()
        {
            ThemMon(wrapMonCafe, "Cà phê sữa", "Images/caphesua.png", 25000);
            ThemMon(wrapMonCafe, "Cà phê đen", "Images/capheden.png", 20000);
            ThemMon(wrapMonCafe, "Bạc xỉu", "Images/bacxiu.png", 30000);
            ThemMon(wrapMonSinhTo, "Sinh tố bơ", "Images/sinhtobo.png", 35000);
            ThemMon(wrapMonBanh, "Bánh flan", "Images/banhflan.png", 25000);
        }

        void ThemMon(WrapPanel panel, string tenMon, string duongDanAnh, double gia)
        {
            Border card = new Border()
            {
                Width = 150,
                Height = 160,
                Background = Brushes.White,
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            StackPanel sp = new StackPanel();
            Image img = new Image()
            {
                Source = new BitmapImage(new System.Uri(duongDanAnh, System.UriKind.Relative)),
                Height = 90,
                Margin = new Thickness(5)
            };
            TextBlock name = new TextBlock()
            {
                Text = tenMon,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            };
            TextBlock price = new TextBlock()
            {
                Text = gia.ToString("N0") + " đ",
                Foreground = Brushes.DarkSlateGray,
                TextAlignment = TextAlignment.Center
            };

            sp.Children.Add(img);
            sp.Children.Add(name);
            sp.Children.Add(price);
            card.Child = sp;

            card.MouseLeftButtonUp += (s, e) =>
            {
                ThemVaoHoaDon(tenMon, gia);
            };

            panel.Children.Add(card);
        }

        void ThemVaoHoaDon(string tenMon, double gia)
        {
            var mon = dsHoaDon.FirstOrDefault(m => m.TenMon == tenMon);
            if (mon != null)
            {
                mon.SoLuong++;
            }
            else
            {
                dsHoaDon.Add(new ChiTietHoaDon { TenMon = tenMon, SoLuong = 1, Gia = gia });
            }
            dgHoaDon.Items.Refresh();
            CapNhatTongTien();
        }

        void CapNhatTongTien()
        {
            double tong = dsHoaDon.Sum(m => m.ThanhTien);
            txtTongTien.Text = tong.ToString("N0") + " đ";
        }
    }
}
