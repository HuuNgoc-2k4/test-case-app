using QuanLyChungCu.Views;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace QuanLyChungCu
{
    public partial class Civillian : Window
    {
        private int currentPage = 1;
        private int itemsPerPage = 9;
        private Owner currentOwner;

        public Civillian(Owner owner)
        {
            InitializeComponent();
            currentOwner = owner;

            Views.dataCivili data = new dataCivili(currentOwner);

            MainContent.Content = data;

            this.DataContext = currentOwner;

            DatabaseHelper.CreateReportsTableIfNotExists();
        }

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
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButunDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Views.dataCivili data = new dataCivili(currentOwner);

            MainContent.Content = data;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Views.NotifiMember notifi = new NotifiMember();

            MainContent.Content = notifi;
        }

        // Gửi khiếu nại
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Views.Khieu_Nai khieu_Nai = new Khieu_Nai(currentOwner);
            MainContent.Content = khieu_Nai;
        }
        // Xem tiền nhà
        private void TienNha_Click(object sender, RoutedEventArgs e)
        {
            Views.MoneyMemberView moneyMemberView = new MoneyMemberView(currentOwner);

            MainContent.Content = moneyMemberView;
        }
    }
}
