using System.Windows;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class EditMoney : Window
    {
        public bool IsUpdated { get; private set; }
        private Money _money;

        public EditMoney(Money money)
        {
            InitializeComponent();
            _money = money;
            LoadCurrentData();
        }

        // Tải dữ liệu hiện tại
        private void LoadCurrentData()
        {
            txtPower.Text = _money.PowerReading.ToString();
            txtWater.Text = _money.WaterReading.ToString();
            txtRoomRent.Text = _money.RoomRent.ToString();
        }

        // Xử lý lưu
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            // Cập nhật giá trị
            _money.PowerReading = int.Parse(txtPower.Text);
            _money.WaterReading = int.Parse(txtWater.Text);
            _money.RoomRent = int.Parse(txtRoomRent.Text);

            // Cập nhật CSDL
            if (DatabaseHelper.UpdateMoney(_money))
            {
                IsUpdated = true;
                Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Validate đầu vào
        private bool ValidateInputs()
        {
            if (!int.TryParse(txtPower.Text, out int power) || power < 0)
            {
                MessageBox.Show("Số điện phải là số nguyên dương!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtWater.Text, out int water) || water < 0)
            {
                MessageBox.Show("Số nước phải là số nguyên dương!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtRoomRent.Text, out int rent) || rent < 0)
            {
                MessageBox.Show("Tiền phòng phải là số nguyên dương!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
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