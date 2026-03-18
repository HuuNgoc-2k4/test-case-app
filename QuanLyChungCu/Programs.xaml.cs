using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using QuanLyChungCu.Views;


namespace QuanLyChungCu
{
    public partial class Programs : Window
    {
        public ObservableCollection<Owner> currentOwners { get; set; }
        public ObservableCollection<Owner> allOwners { get; set; }

        private int currentPage = 1;
        private int itemsPerPage = 9;

        public Programs()
        {
            InitializeComponent();
            DataContext = this;

            DatabaseHelper.CreateOwnersTableIfNotExists();
            DatabaseHelper.CreateResidentsTableIfNotExists();
            DatabaseHelper.CreateNotificationsTableIfNotExists();

            var listFromDb = DatabaseHelper.GetAllOwners();
            foreach (var owner in listFromDb)
            {
                Owner details = DatabaseHelper.GetOwnerDetails(owner.OwnerID);
                owner.Residents = details.Residents;
                owner.SoNguoiO = details.SoNguoiO;
            }

            allOwners = new ObservableCollection<Owner>(listFromDb);
            currentOwners = new ObservableCollection<Owner>();


            Views.DataView view1 = new DataView();

            MainContent.Content = view1;
        }

        #region Sự kiện điều khiển cửa sổ

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #endregion

        #region Các trang
        //Gửi Thông báo
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Views.NotifiView notifiView = new Views.NotifiView();
            MainContent.Content = notifiView;
        }

        //Danh sách cư dân
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Views.DataView view = new Views.DataView();

            MainContent.Content = view;
        }


        // Tiền Dịch vụ
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Views.MoneyView view = new Views.MoneyView();

            MainContent.Content = view;
        }


        // Danh Sách căn hộ
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Views.Danhsachcanho view = new Views.Danhsachcanho();

            MainContent.Content = view;
        }
        #endregion

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Views.ReportsAdmin view = new Views.ReportsAdmin();

            MainContent.Content = view;
        }
    }
}
