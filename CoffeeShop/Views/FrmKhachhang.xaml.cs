using CoffeeShop.Data;
using CoffeeShop.Models; // Sử dụng thư mục Models của bạn
using System;
using System.Linq;
using System.Windows;

// Đảm bảo namespace này khớp với dự án của bạn
namespace CoffeeShop.Views
{
    public partial class FrmKhachhang : Window
    {
        // Khởi tạo đối tượng Context để làm việc với CSDL
        CoffeeShopContext db = new CoffeeShopContext();

        // Biến cờ để kiểm tra đang Thêm mới hay Sửa
        private bool isNew = false;

        public FrmKhachhang()
        {
            InitializeComponent();
        }

        // --- HÀM TRỢ GIÚP ---

        // Hàm tải dữ liệu từ CSDL lên DataGrid
        private void LoadData()
        {
            // Truy vấn lấy tất cả khách hàng, sắp xếp theo tên
            var query = from kh in db.KhachHangs
                        orderby kh.TenKhachHang
                        select kh;

            // Hiển thị lên DataGrid
            dgKhachHang.ItemsSource = query.ToList();
        }

        // Hàm điều khiển trạng thái các nút và ô nhập liệu
        private void SetTrangThai(bool isEditing)
        {
            // Khi isEditing = true (đang Lưu hoặc Hủy)
            // Ngược lại isEditing = false (đang Thêm hoặc Sửa)

            // Bật/tắt các ô nhập liệu
            gridForm.IsEnabled = isEditing;

            // Bật/tắt các nút
            btnLuu.IsEnabled = isEditing;
            btnHuy.IsEnabled = isEditing;

            btnThem.IsEnabled = !isEditing;
            btnSua.IsEnabled = !isEditing;
            btnXoa.IsEnabled = !isEditing;
            btnDong.IsEnabled = !isEditing;
        }

        // Hàm xóa rỗng các ô nhập liệu
        private void ClearFields()
        {
            txtMaKh.Text = "";
            txtTenKh.Text = "";
            txtSoDienThoai.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtDiemTichLuy.Text = "0";
        }

        // --- SỰ KIỆN CỦA CỬA SỔ ---

        // Khi cửa sổ được tải
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            SetTrangThai(false); // Trạng thái ban đầu là xem
        }

        // Khi chọn một dòng trên DataGrid
        private void dgKhachHang_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Kiểm tra xem có mục nào được chọn không
            if (dgKhachHang.SelectedItem != null)
            {
                try
                {
                    // Lấy KhachHang được chọn từ DataGrid
                    var kh = dgKhachHang.SelectedItem as KhachHang;

                    // Hiển thị thông tin lên các ô TextBox
                    txtMaKh.Text = kh.MaKhachHang;
                    txtTenKh.Text = kh.TenKhachHang;
                    txtSoDienThoai.Text = kh.SoDienThoai;
                    txtDiaChi.Text = kh.DiaChi;
                    txtEmail.Text = kh.Email;
                    txtDiemTichLuy.Text = kh.DiemTichLuy.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn khách hàng: " + ex.Message);
                }
            }
        }

        // --- SỰ KIỆN CÁC NÚT BẤM ---

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;       // Đặt cờ là Thêm mới
            SetTrangThai(true); // Bật form nhập liệu
            ClearFields();      // Xóa rỗng các ô
            txtMaKh.IsEnabled = true; // Cho phép nhập Mã KH
            txtMaKh.Focus();    // Di chuyển con trỏ vào ô Mã KH
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgKhachHang.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa.", "Thông báo");
                return;
            }

            isNew = false;      // Đặt cờ là Sửa
            SetTrangThai(true); // Bật form nhập liệu
            txtMaKh.IsEnabled = false; // Không cho phép sửa Mã KH (Khóa chính)
            txtTenKh.Focus();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgKhachHang.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo");
                return;
            }

            // Xác nhận xóa
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Lấy mã khách hàng từ ô TextBox
                    var maKh = txtMaKh.Text;

                    // Tìm khách hàng trong CSDL
                    var khToDelete = db.KhachHangs.Find(maKh);

                    if (khToDelete != null)
                    {
                        // Xóa khỏi CSDL
                        db.KhachHangs.Remove(khToDelete);
                        db.SaveChanges(); // Lưu thay đổi

                        // Tải lại dữ liệu
                        LoadData();
                        ClearFields();
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa khách hàng. Khách hàng này có thể đang được sử dụng trong Hóa đơn.\nChi tiết: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrWhiteSpace(txtMaKh.Text) || string.IsNullOrWhiteSpace(txtTenKh.Text))
                {
                    MessageBox.Show("Mã khách hàng và Tên khách hàng là bắt buộc.", "Lỗi");
                    return;
                }

                int diem = 0;
                if (!int.TryParse(txtDiemTichLuy.Text, out diem))
                {
                    MessageBox.Show("Điểm tích lũy phải là một con số.", "Lỗi");
                    return;
                }


                if (isNew) // Nếu là THÊM MỚI
                {
                    // Kiểm tra trùng mã KH (khóa chính)
                    var existing = db.KhachHangs.Find(txtMaKh.Text);
                    if (existing != null)
                    {
                        MessageBox.Show("Mã khách hàng này đã tồn tại!", "Lỗi");
                        return;
                    }

                    // Tạo đối tượng KhachHang mới
                    KhachHang kh = new KhachHang
                    {
                        MaKhachHang = txtMaKh.Text,
                        TenKhachHang = txtTenKh.Text,
                        SoDienThoai = txtSoDienThoai.Text,
                        DiaChi = txtDiaChi.Text,
                        Email = txtEmail.Text,
                        DiemTichLuy = diem
                    };

                    // Thêm vào CSDL
                    db.KhachHangs.Add(kh);
                }
                else // Nếu là SỬA
                {
                    // Tìm khách hàng cần sửa trong CSDL
                    var khToUpdate = db.KhachHangs.Find(txtMaKh.Text);
                    if (khToUpdate != null)
                    {
                        // Cập nhật thông tin
                        khToUpdate.TenKhachHang = txtTenKh.Text;
                        khToUpdate.SoDienThoai = txtSoDienThoai.Text;
                        khToUpdate.DiaChi = txtDiaChi.Text;
                        khToUpdate.Email = txtEmail.Text;
                        khToUpdate.DiemTichLuy = diem;
                    }
                }

                // Lưu tất cả thay đổi vào CSDL
                db.SaveChanges();

                // Cập nhật lại giao diện
                LoadData();
                SetTrangThai(false); // Trở về trạng thái xem
                ClearFields();
                MessageBox.Show("Lưu thông tin thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi");
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            SetTrangThai(false); // Tắt form, bật các nút Thêm/Sửa/Xóa
            ClearFields();       // Xóa rỗng các ô
        }

        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Đóng cửa sổ hiện tại
        }
    }
}