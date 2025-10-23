using System.Windows;
using CoffeeShop.Views.Admin; // ✅ chỉ cần thế này, không có dấu { ở đây

namespace CoffeeShop.Views.Admin
{
    public partial class AdminLoginWindow : Window
    {
        public AdminLoginWindow()
        {
            InitializeComponent();

            // Ẩn hiện placeholder của Username (Admin)
            txtAdminUsername.TextChanged += (s, e) =>
            {
                adminUserPlaceholder.Visibility =
                    string.IsNullOrEmpty(txtAdminUsername.Text)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            };

            // Ẩn hiện placeholder của Password (Admin)
            txtAdminPassword.PasswordChanged += (s, e) =>
            {
                adminPassPlaceholder.Visibility =
                    string.IsNullOrEmpty(txtAdminPassword.Password)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            };
        }

        private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtAdminUsername.Text.Trim();
            string password = txtAdminPassword.Password;

            // Tài khoản mặc định (sau này bạn thay bằng DB)
            if (username == "admin" && password == "123")
            {
                MessageBox.Show("Đăng nhập admin thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // ✅ Mở cửa sổ chính của admin
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();

                this.Close(); // Đóng cửa sổ đăng nhập
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // đóng cửa sổ admin login, quay lại login thường
        }
    }
}
