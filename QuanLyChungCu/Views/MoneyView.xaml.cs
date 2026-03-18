using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyChungCu.Views
{
    public partial class MoneyView : UserControl
    {
        public ObservableCollection<Money> currentOwners { get; set; }
        public ObservableCollection<Money> allOwners { get; set; }

        private int currentPage = 1;
        private int itemsPerPage = 9;

        public MoneyView()
        {
            InitializeComponent();
            DataContext = this;

            DatabaseHelper.CreateTIENTableIfNotExists();
            var listMoney = DatabaseHelper.GetMoneyData();

            allOwners = new ObservableCollection<Money>(listMoney);
            currentOwners = new ObservableCollection<Money>();

            LoadData();
            LoadPage(currentPage);
        }

        private void LoadData()
        {
            var listFromDb = DatabaseHelper.GetMoneyData();
            for (int i = 0; i < listFromDb.Count; i++)
            {
                listFromDb[i].STT = i + 1;
            }
            allOwners = new ObservableCollection<Money>(listFromDb);
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

        private void EditMoney_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int moneyId)
            {
                var selectedMoney = allOwners.FirstOrDefault(m => m.ID == moneyId);
                if (selectedMoney != null)
                {
                    EditMoney editWindow = new EditMoney(selectedMoney);
                    editWindow.ShowDialog();

                    if (editWindow.IsUpdated)
                    {
                        LoadData();
                        LoadPage(currentPage);
                    }
                }
            }
        }
    }
}