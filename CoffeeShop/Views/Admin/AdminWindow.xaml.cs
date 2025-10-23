using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CoffeeShop.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void BtnDanhMuc_Click(object sender, RoutedEventArgs e)
        {
            var win = new DanhMucWindow();
            win.ShowDialog();
        }
        private void BtnSanPham_Click(object sender, RoutedEventArgs e)
        {
            var window = new SanPhamWindow();
            window.ShowDialog();
        }
        private void BtnBan_Click(object sender, RoutedEventArgs e)
        {
            var frm = new FrmQuanLyBan();
            frm.ShowDialog(); // hoặc frm.Show() nếu bạn muốn không chặn cửa sổ chính
        }


    }
}
