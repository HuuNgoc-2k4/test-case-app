using QuanLyChungCu.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace QuanLyChungCu.Views
{
    public partial class MoneyMemberView : UserControl
    {
        public Money MoneyInfo { get; set; }

        public MoneyMemberView(Owner owner)
        {
            InitializeComponent();

            MoneyInfo = DatabaseHelper.GetMoneyByOwnerId(owner.OwnerID);

            this.DataContext = MoneyInfo;
        }
    }
}