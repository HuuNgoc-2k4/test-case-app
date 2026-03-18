using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class AddResident : Window
    {
        public bool IsAdded { get; private set; } = false;
        public int OwnerID { get; private set; }

        public AddResident(int ownerId)
        {
            InitializeComponent();
            OwnerID = ownerId;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtBirthDate.Text) ||
                string.IsNullOrWhiteSpace(txtQueQuan.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string phonePattern = @"^0\d{9}$";
            if (!Regex.IsMatch(txtPhone.Text.Trim(), phonePattern))
            {
                MessageBox.Show("Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Resident newResident = new Resident
            {
                Name = txtName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                BirthDate = txtBirthDate.Text.Trim(),
                QueQuan = txtQueQuan.Text.Trim(),
                OwnerID = OwnerID
            };

            bool inserted = DatabaseHelper.InsertResident(newResident);
            if (inserted)
            {
                IsAdded = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm cư dân thất bại!");
            }
        }
    }
}
