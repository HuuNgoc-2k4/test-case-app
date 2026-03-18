using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChungCu.Views
{
    public partial class DataView : UserControl
    {
        public ObservableCollection<Owner> currentOwners { get; set; }
        public ObservableCollection<Owner> allOwners { get; set; }

        private int currentPage = 1;
        private int itemsPerPage = 9;
        public DataView()
        {
            InitializeComponent();
            DataContext = this;

            DatabaseHelper.CreateOwnersTableIfNotExists();
            DatabaseHelper.CreateResidentsTableIfNotExists();
            var listFromDb = DatabaseHelper.GetAllOwners();
            foreach (var owner in listFromDb)
            {
                Owner details = DatabaseHelper.GetOwnerDetails(owner.OwnerID);
                owner.Residents = details.Residents;
                owner.SoNguoiO = details.SoNguoiO;
            }

            allOwners = new ObservableCollection<Owner>(listFromDb);
            currentOwners = new ObservableCollection<Owner>();

            LoadData();
            LoadPage(currentPage);
        }
        #region Load dữ liệu
        private void LoadData()
        {
            var listFromDb = DatabaseHelper.GetAllOwners();
            for (int i = 0; i < listFromDb.Count; i++)
            {
                var details = DatabaseHelper.GetOwnerDetails(listFromDb[i].OwnerID);
                listFromDb[i].Residents = details.Residents;
                listFromDb[i].STT = i + 1;
                for (int j = 0; j < listFromDb[i].Residents.Count; j++)
                {
                    listFromDb[i].Residents[j].STT = j + 1;
                }
            }
            allOwners = new ObservableCollection<Owner>(listFromDb);
            LoadPage(currentPage);
        }



        private void LoadPage(int page)
        {
            currentOwners.Clear();

            int startIndex = (page - 1) * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, allOwners.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                currentOwners.Add(allOwners[i]);
            }
            MemberDataGrid.ItemsSource = currentOwners;
        }


        #endregion

        #region Xem Thêm Xóa Sửa chủ hộ
        //Thêm
        private void btnAddOwner_Click(object sender, RoutedEventArgs e)
        {
            AddOwner addOwnerWindow = new AddOwner();
            addOwnerWindow.ShowDialog();
            if (addOwnerWindow.isAdd)
            {
                // Chèn chủ hộ mới vào DB
                bool inserted = DatabaseHelper.InsertOwner(addOwnerWindow.NewOwner);
                if (inserted)
                {

                }
                else
                {
                    MessageBox.Show("Thêm chủ hộ thất bại!");
                }
                var fullList = DatabaseHelper.GetAllOwners();
                for (int i = 0; i < fullList.Count; i++)
                {
                    fullList[i].STT = i + 1;
                }
                foreach (var owner in fullList)
                {
                    Owner details = DatabaseHelper.GetOwnerDetails(owner.OwnerID);
                    owner.Residents = details.Residents;
                    owner.SoNguoiO = details.SoNguoiO;
                }
                allOwners = new ObservableCollection<Owner>(fullList);
                currentPage = 1;
                LoadPage(currentPage);
            }
        }

        //Sửa 
        private void EditOwner_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Owner selectedOwner)
            {
                // Lấy thông tin chi tiết của chủ hộ nếu cần (hoặc sử dụng selectedOwner)
                Owner ownerDetails = DatabaseHelper.GetOwnerDetails(selectedOwner.OwnerID);
                EditOwner editWindow = new EditOwner(ownerDetails);
                editWindow.ShowDialog();
                if (editWindow.IsUpdated)
                {
                    // Reload lại danh sách chủ hộ để cập nhật giao diện
                    LoadData();
                }
            }
        }

        //Xóa
        private void DeleteOwner_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Owner ownerToDelete)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa chủ hộ {ownerToDelete.Name} không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool deleted = DatabaseHelper.DeleteOwner(ownerToDelete.OwnerID);
                    if (deleted)
                    {
                        // Reload dữ liệu sau khi xóa
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!");
                    }
                }
            }
        }

        //Thông tin
        private void ShowOwnerDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Owner selectedOwner)
            {
                Owner details = DatabaseHelper.GetOwnerDetails(selectedOwner.OwnerID);
                HouseholdDetails detailsWindow = new HouseholdDetails(details);
                detailsWindow.ShowDialog();
            }
            LoadData();
        }
        #endregion

        #region Tìm kiếm
        // Tìm kiếm
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Nhập từ khóa...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }
        // Xử lý khi mất focus khỏi ô tìm kiếm
        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                txtSearch.Text = "Nhập từ khóa...";
                txtSearch.Foreground = Brushes.Gray;
            }
        }
        // Xử lý khi nhấn Tìm kiếm
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (keyword == "Nhập từ khóa...")
                keyword = "";

            var selectedItem = cmbSearchColumn.SelectedItem as ComboBoxItem;
            string searchBy = selectedItem?.Tag?.ToString() ?? "Name";

            var fullList = DatabaseHelper.GetAllOwners();
            List<Owner> filteredList;

            if (string.IsNullOrEmpty(keyword))
            {
                filteredList = fullList;
            }
            else
            {
                filteredList = fullList.Where(owner =>
                {
                    string valueToSearch = searchBy switch
                    {
                        "Name" => owner.Name ?? "",
                        "Phone" => owner.Phone ?? "",
                        "RoomNumber" => owner.RoomNumber.ToString(),
                        "QueQuan" => owner.QueQuan ?? "",
                        "BirthDate" => owner.BirthDate ?? "",
                        _ => ""
                    };
                    return valueToSearch.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                }).ToList();
            }

            // Cập nhật STT và thông tin cư dân
            for (int i = 0; i < filteredList.Count; i++)
            {
                Owner details = DatabaseHelper.GetOwnerDetails(filteredList[i].OwnerID);
                filteredList[i].Residents = details.Residents;
                filteredList[i].SoNguoiO = details.SoNguoiO;
                filteredList[i].STT = i + 1; // Cập nhật STT tại đây
            }

            allOwners = new ObservableCollection<Owner>(filteredList);
            currentPage = 1;
            LoadPage(currentPage);
        }
        // Xóa tìm kiếm
        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "Nhập từ khóa...";
            txtSearch.Foreground = Brushes.Gray;
            cmbSearchColumn.SelectedIndex = 0;

            var fullList = DatabaseHelper.GetAllOwners();

            // Cập nhật STT và thông tin cư dân
            for (int i = 0; i < fullList.Count; i++)
            {
                Owner details = DatabaseHelper.GetOwnerDetails(fullList[i].OwnerID);
                fullList[i].Residents = details.Residents;
                fullList[i].SoNguoiO = details.SoNguoiO;
                fullList[i].STT = i + 1; // Cập nhật STT tại đây
            }

            allOwners = new ObservableCollection<Owner>(fullList);
            currentPage = 1;
            LoadPage(currentPage);
        }
        #endregion

        #region Phân trang
        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            currentPage--;
            if (currentPage < 1) currentPage = 1;
            LoadPage(currentPage);
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (int.TryParse(btn.Content.ToString(), out int pageNumber))
                {
                    currentPage = pageNumber;
                    LoadPage(currentPage);
                }
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            int totalPages = (int)Math.Ceiling(allOwners.Count / (double)itemsPerPage);
            if (currentPage > totalPages) currentPage = totalPages;
            LoadPage(currentPage);
        }
    }
    #endregion
}
