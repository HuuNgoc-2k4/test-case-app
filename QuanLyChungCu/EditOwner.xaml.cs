using System;
using System.Windows;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class EditOwner : Window
    {
        public Owner CurrentOwner { get; set; }
        public bool IsUpdated { get; private set; } = false;

        public EditOwner(Owner owner)
        {
            InitializeComponent();
            CurrentOwner = owner;
            LoadOwnerData();
        }

        private void LoadOwnerData()
        {
            if (CurrentOwner != null)
            {
                txtName.Text = CurrentOwner.Name;
                txtPhone.Text = CurrentOwner.Phone;
                txtRoomNumber.Text = CurrentOwner.RoomNumber.ToString();
                txtQueQuan.Text = CurrentOwner.QueQuan;
                txtBirthDate.Text = CurrentOwner.BirthDate;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            // Cập nhật dữ liệu vào CurrentOwner
            CurrentOwner.Name = txtName.Text.Trim();
            CurrentOwner.Phone = txtPhone.Text.Trim();
            CurrentOwner.RoomNumber = int.Parse(txtRoomNumber.Text);
            CurrentOwner.QueQuan = txtQueQuan.Text.Trim();
            CurrentOwner.BirthDate = txtBirthDate.Text.Trim();

            // Cập nhật database
            bool updated = DatabaseHelper.UpdateOwner(CurrentOwner);
            if (updated)
            {
                MessageBox.Show("Cập nhật chủ hộ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                IsUpdated = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên chủ hộ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtRoomNumber.Text, out _))
            {
                MessageBox.Show("Số phòng phải là số nguyên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidPhone(string phone)
        {
            return phone.Length >= 9 && phone.Length <= 12 && long.TryParse(phone, out _);
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
    }
}
