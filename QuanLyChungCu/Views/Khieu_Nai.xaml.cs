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
    public partial class Khieu_Nai : UserControl
    {
        private Owner _currentOwner;

        public Khieu_Nai(Owner owner)
        {
            InitializeComponent();
            _currentOwner = owner;
            LoadReports();
        }

        private void LoadReports()
        {
            var reports = DatabaseHelper.GetAllReports().Where(r => r.OwnerID == _currentOwner.OwnerID).ToList();
            NotificationListss.ItemsSource = reports;
        }
        // Xử lý khi TextBox tiêu đề nhận focus
        private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TitleTextBox.Text == "Nhập vấn đề...")
            {
                TitleTextBox.Text = "";
                TitleTextBox.Foreground = Brushes.Black;
            }
        }

        // Xử lý khi TextBox tiêu đề mất focus
        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                TitleTextBox.Text = "Nhập vấn đề...";
                TitleTextBox.Foreground = Brushes.Gray;
            }
        }

        // Xử lý khi TextBox nội dung nhận focus
        private void ContentTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ContentTextBox.Text == "Nhập thông tin chi tiết...")
            {
                ContentTextBox.Text = "";
                ContentTextBox.Foreground = Brushes.Black;
            }
        }

        // Xử lý khi TextBox nội dung mất focus
        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ContentTextBox.Text))
            {
                ContentTextBox.Text = "Nhập thông tin chi tiết...";
                ContentTextBox.Foreground = Brushes.Gray;
            }
        }

        private void SendNotification_Clicks(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string content = ContentTextBox.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(title) || title == "Nhập vấn đề...")
            {
                MessageBox.Show("Vui lòng nhập tiêu đề!");
                return;
            }

            if (string.IsNullOrWhiteSpace(content) || content == "Nhập thông tin chi tiết...")
            {
                MessageBox.Show("Vui lòng nhập nội dung!");
                return;
            }

            // Insert to database
            bool success = DatabaseHelper.InsertReport(
                title,
                content,
                DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                _currentOwner.OwnerID
            );

            if (success)
            {
                LoadReports();
                TitleTextBox.Text = "Nhập vấn đề...";
                ContentTextBox.Text = "Nhập thông tin chi tiết...";
            }
            else
            {
                MessageBox.Show("Có lỗi khi gửi khiếu nại!");
            }
        }
    }
}
