using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class EditResident : Window
    {
        public bool IsUpdated { get; set; } = false;
        private Resident _resident;
        public int OwnerID { get; private set; }

        public EditResident(int ownerId)
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
        public EditResident(Resident resident)
        {
            InitializeComponent();
            _resident = resident;
            txtName.Text = _resident.Name;
            txtPhone.Text = _resident.Phone;
            dpBirthDate.Text = _resident.BirthDate;
            txtQueQuan.Text = _resident.QueQuan;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _resident.Name = txtName.Text;
            _resident.Phone = txtPhone.Text;
            _resident.BirthDate = dpBirthDate.Text;
            _resident.QueQuan = txtQueQuan.Text;

            bool isUpdated = DatabaseHelper.UpdateResident(_resident);
            if (isUpdated)
            {
                IsUpdated = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!");
            }
        }
    }
}
