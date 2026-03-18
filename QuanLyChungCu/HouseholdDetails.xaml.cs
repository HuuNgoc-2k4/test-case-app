using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class HouseholdDetails : Window
    {
        public Owner CurrentOwner { get; set; }
        public HouseholdDetails(Owner ownerDetails)
        {
            InitializeComponent();
            CurrentOwner = ownerDetails;
            DataContext = ownerDetails;
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
        // Sự kiện thêm cư dân
        private void btnAddResident_Click(object sender, RoutedEventArgs e)
        {
            AddResident addResidentWindow = new AddResident(CurrentOwner.OwnerID);
            addResidentWindow.ShowDialog();
            if (addResidentWindow.IsAdded)
            {
                Owner updatedOwner = DatabaseHelper.GetOwnerDetails(CurrentOwner.OwnerID);
                CurrentOwner.Residents = updatedOwner.Residents;
                MemberDataGrid.ItemsSource = CurrentOwner.Residents;
            }
        }
        // Sửa cư dân
        private void EditResident_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Resident selectedResident)
            {
                EditResident editWindow = new EditResident(selectedResident);
                editWindow.ShowDialog();
                if (editWindow.IsUpdated)
                {
                    Owner updatedOwner = DatabaseHelper.GetOwnerDetails(CurrentOwner.OwnerID);
                    CurrentOwner.Residents = updatedOwner.Residents;
                    MemberDataGrid.ItemsSource = CurrentOwner.Residents;
                    MemberDataGrid.Items.Refresh();
                }
            }
        }

        // Xóa cư dân
        private void DeleteResident_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Resident residentToDelete)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa {residentToDelete.Name}?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    bool isDeleted = DatabaseHelper.DeleteResident(residentToDelete.ResidentID);
                    if (isDeleted)
                    {
                        CurrentOwner.Residents.Remove(residentToDelete);
                        DatabaseHelper.UpdateOwnerResidentCount(CurrentOwner.OwnerID, CurrentOwner.Residents.Count);
                        MemberDataGrid.Items.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!");
                    }
                }
            }
        }
    }
}
