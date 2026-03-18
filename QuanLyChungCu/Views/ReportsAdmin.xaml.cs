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
    public partial class ReportsAdmin : UserControl
    {
        public ObservableCollection<ReportWithOwnerInfo> Reports { get; set; }

        public ReportsAdmin()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            var reportsFromDb = DatabaseHelper.GetAllReportsWithOwnerInfo();
            Reports = new ObservableCollection<ReportWithOwnerInfo>(reportsFromDb);
            ReportList.ItemsSource = Reports;
        }
    }
}
