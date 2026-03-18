using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChungCu;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();

        DatabaseHelper.EnsureCoreTablesCreated();
    }
    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    private void btnChangerPassword_Click(object sender, RoutedEventArgs e)
    {
        ChangerPassword changerPasswordWindow = new ChangerPassword();
        changerPasswordWindow.Show();
        this.Close();
    }
    private void ShowPassword_Checked(object sender, RoutedEventArgs e)

    {
        txtPasswordVisible.Text = txtPassword.Password;
        txtPasswordVisible.Visibility = Visibility.Visible;
        txtPassword.Visibility = Visibility.Collapsed;
    }

    private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
    {
        txtPassword.Password = txtPasswordVisible.Text;
        txtPassword.Visibility = Visibility.Visible;
        txtPasswordVisible.Visibility = Visibility.Collapsed;
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        string username = txtUser.Text;
        string password = txtPassword.Password;
        if (!DatabaseHelper.ValidateUser(username, password))
        {
            MessageBox.Show("SDT không đúng hoặc người dùng không tồn tại!");
            return;
        }

        // Lấy role của người dùng từ database
        int? role = DatabaseHelper.GetUserRole(username, password);

        if (role.HasValue)
        {
            // Nếu role là 0 -> mở cửa sổ Civillian (user)
            if (role.Value == 0)
            {
                // Lấy thông tin chủ hộ dựa trên số điện thoại đăng nhập
                Owner owner = DatabaseHelper.GetOwnerByPhone(username);
                if (owner != null)
                {
                    Civillian civillianWindow = new Civillian(owner);
                    civillianWindow.Show();
                }
            }
            // Nếu role là 1 -> mở cửa sổ Programs (admin)
            else if (role.Value == 1)
            {
                Programs programWindow = new Programs();
                programWindow.Show();
            }
            this.Close();
        }
        else
        {
            MessageBox.Show("tài khoản không đúng hoặc người dùng không tồn tại!");
        }
    }
}
