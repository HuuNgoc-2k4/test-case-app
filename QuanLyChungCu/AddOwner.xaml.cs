using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class AddOwner : Window
    {
        public bool isAdd { get; private set; }
        public Owner NewOwner { get; private set; }

        public AddOwner()
        {
            InitializeComponent();
            isAdd = false;
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
                string.IsNullOrWhiteSpace(txtRoomNumber.Text) ||
                string.IsNullOrWhiteSpace(txtQueQuan.Text) ||
                string.IsNullOrWhiteSpace(txtBirthDate.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtRoomNumber.Text, out int roomNumber))
            {
                MessageBox.Show("Số phòng phải là số nguyên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string phonePattern = @"^0\d{9}$";
            string phone = txtPhone.Text.Trim();
            if (!Regex.IsMatch(phone, phonePattern))
            {
                MessageBox.Show("Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DatabaseHelper.IsOwnerPhoneExists(phone))
            {
                MessageBox.Show("Số điện thoại đã tồn tại, vui lòng nhập số khác!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DatabaseHelper.IsRoomNumberExists(roomNumber))
            {
                MessageBox.Show("Số phòng đã tồn tại, vui lòng nhập số phòng khác!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewOwner = new Owner
            {
                Name = txtName.Text.Trim(),
                Phone = phone,
                RoomNumber = roomNumber,
                QueQuan = txtQueQuan.Text.Trim(),
                BirthDate = txtBirthDate.Text.Trim()
            };

            isAdd = true;
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
