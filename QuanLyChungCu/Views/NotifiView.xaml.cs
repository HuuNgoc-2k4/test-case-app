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
    public partial class NotifiView : UserControl
    {
        public ObservableCollection<Notification> Notifications { get; set; }

        public NotifiView()
        {
            InitializeComponent();
            // Lấy dữ liệu từ database
            var notificationsFromDb = DatabaseHelper.GetNotifications();
            Notifications = new ObservableCollection<Notification>(notificationsFromDb);
            NotificationLists.ItemsSource = Notifications;
        }
        // Sự kiện khi TextBox nhận focus
        private void ContentTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text == "Nhập nội dung thông báo...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        // Sự kiện khi TextBox mất focus
        private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text == "Nhập tiêu đề...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }
        // Sự kiện khi TextBox mất focus
        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "Nhập tiêu đề...";
                tb.Foreground = Brushes.Gray;
            }
        }
        // Sự kiện khi TextBox mất focus
        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "Nhập nội dung thông báo...";
                tb.Foreground = Brushes.Gray;
            }
        }

        // Sự kiện khi nhấn nút gửi thông báo
        private void SendNotification_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string content = ContentTextBox.Text;

            if (string.IsNullOrWhiteSpace(title) || title == "Nhập tiêu đề...")
            {
                MessageBox.Show("Vui lòng nhập tiêu đề!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(content) || content == "Nhập nội dung thông báo...")
            {
                MessageBox.Show("Vui lòng nhập nội dung thông báo!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            // Chèn thông báo vào database
            bool inserted = DatabaseHelper.InsertNotification(title, content, date);
            if (inserted)
            {
                // Cập nhật lại ObservableCollection (có thể thêm vào đầu danh sách)
                Notifications.Insert(0, new Notification { Title = title, Content = content, Date = date });
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thông báo!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Reset lại TextBox
            TitleTextBox.Text = "Nhập tiêu đề...";
            TitleTextBox.Foreground = Brushes.Gray;
            ContentTextBox.Text = "Nhập nội dung thông báo...";
            ContentTextBox.Foreground = Brushes.Gray;
        }
    }
}
