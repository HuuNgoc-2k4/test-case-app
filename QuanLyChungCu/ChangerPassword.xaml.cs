using System.Windows;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class ChangerPassword : Window
    {
        public ChangerPassword()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtCurrentPasswordVisible.Text = txtCurrentPassword.Password;
            txtNewPasswordVisible.Text = txtNewPassword.Password;
            txtCurrentPassword.Visibility = Visibility.Collapsed;
            txtCurrentPasswordVisible.Visibility = Visibility.Visible;
            txtNewPassword.Visibility = Visibility.Collapsed;
            txtNewPasswordVisible.Visibility = Visibility.Visible;
        }

        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtCurrentPassword.Password = txtCurrentPasswordVisible.Text;
            txtNewPassword.Password = txtNewPasswordVisible.Text;
            txtCurrentPassword.Visibility = Visibility.Visible;
            txtCurrentPasswordVisible.Visibility = Visibility.Collapsed;
            txtNewPassword.Visibility = Visibility.Visible;
            txtNewPasswordVisible.Visibility = Visibility.Collapsed;
        }


        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text.Trim();
            string currentPassword = txtCurrentPassword.Password.Trim();
            string newPassword = txtNewPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gọi hàm đổi mật khẩu từ DatabaseHelper
            bool changed = DatabaseHelper.ChangeUserPassword(username, currentPassword, newPassword);
            if (changed)
            {
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Login LoginWindow = new Login();
                LoginWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Đổi mật khẩu thất bại! Mật khẩu hiện tại không đúng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}
