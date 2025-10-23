using System.Windows;
using System.Windows.Controls;
using CoffeeShop.Views.Admin;

namespace CoffeeShop.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // Ẩn hiện placeholder của Username
            txtUsername.TextChanged += (s, e) =>
            {
                usernamePlaceholder.Visibility =
                    string.IsNullOrEmpty(txtUsername.Text)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            };

            // Ẩn hiện placeholder của Password
            txtPassword.PasswordChanged += (s, e) =>
            {
                txtPlaceholder.Visibility =
                    string.IsNullOrEmpty(txtPassword.Password)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            };
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            // Giả lập đăng nhập
            if (username == "phucvu" && password == "123")
            {
                MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                FrmPhucVu frm = new FrmPhucVu();
                frm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AdminLogin_Click(object sender, RoutedEventArgs e)
        {
            var adminWindow = new AdminLoginWindow();
            adminWindow.Owner = this; // Đè lên LoginWindow
            adminWindow.ShowDialog();
        }

    }
}
