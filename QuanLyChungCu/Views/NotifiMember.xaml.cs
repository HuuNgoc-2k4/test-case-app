using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using QuanLyChungCu;

namespace QuanLyChungCu.Views
{
    public partial class NotifiMember : UserControl
    {
        public ObservableCollection<Notification> Notifications { get; set; }

        public NotifiMember()
        {
            InitializeComponent();

            // Load notifications from database
            Notifications = new ObservableCollection<Notification>(DatabaseHelper.GetNotifications());

            NotificationList.ItemsSource = Notifications;
        }

        public void AddNotification(string title, DateTime date, string content)
        {
            Notifications.Add(new Notification
            {
                Title = title,
                Date = date.ToString("yyyy-MM-dd"),
                Content = content
            });
        }
    }
}