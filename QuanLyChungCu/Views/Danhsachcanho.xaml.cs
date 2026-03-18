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
    
    public partial class Danhsachcanho : UserControl
    {
        public ObservableCollection<CanHo> currentOwners { get; set; }
        public ObservableCollection<CanHo> allOwners { get; set; }

        private int currentPage = 1;
        private int itemsPerPage = 9;

        public Danhsachcanho()
        {
            InitializeComponent();
            DataContext = this;

            DatabaseHelper.CreateCANHOTableIfNotExists();
            var listCanHo = DatabaseHelper.GetAllCanHo();

            allOwners = new ObservableCollection<CanHo>(listCanHo);
            currentOwners = new ObservableCollection<CanHo>();

            LoadData();
            LoadPage(currentPage);

        }
        private void LoadData()
        {
            var listFromDb = DatabaseHelper.GetAllCanHo();
            allOwners = new ObservableCollection<CanHo>(listFromDb);
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

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            currentPage--;
            if (currentPage < 1) currentPage = 1;
            LoadPage(currentPage);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            int totalPages = (int)Math.Ceiling(allOwners.Count / (double)itemsPerPage);
            if (currentPage > totalPages) currentPage = totalPages;
            LoadPage(currentPage);
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "Nhập từ khóa...";
            txtSearch.Foreground = Brushes.Gray;

            var fullList = DatabaseHelper.GetAllCanHo();
            allOwners = new ObservableCollection<CanHo>(fullList);

            currentPage = 1;
            LoadPage(currentPage);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (keyword == "Nhập từ khóa...")
                keyword = "";

            if (string.IsNullOrEmpty(keyword))
            {
                var fullList = DatabaseHelper.GetAllCanHo();
                
                allOwners = new ObservableCollection<CanHo>(fullList);
            }
            else
            {
                var filtered = new ObservableCollection<CanHo>();
                foreach (var owner in allOwners)
                {
                    if (owner.LoaiPhong.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        filtered.Add(owner);
                    }
                }
                allOwners = filtered;
            }

            currentPage = 1;
            LoadPage(currentPage);
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Nhập từ khóa...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                txtSearch.Text = "Nhập từ khóa...";
                txtSearch.Foreground = Brushes.Gray;
            }
        }
    }
}
