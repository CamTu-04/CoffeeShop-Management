using CoffeeShop.Views;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeShop.Views
{
    public partial class FrmPhucVu : Window
    {
        public FrmPhucVu()
        {
            InitializeComponent();
        }

        private void btnBan_Click(object sender, RoutedEventArgs e)
        {
            mainContent.Children.Clear();
            var frmBan = new FrmBan(); // Đây là UserControl, không phải Window
            mainContent.Children.Add(frmBan );
        }
    }
}
