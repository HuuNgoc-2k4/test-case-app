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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChungCu.Views
{
    /// <summary>
    /// Interaction logic for dataCivili.xaml
    /// </summary>
    public partial class dataCivili : UserControl
    {

        private int currentPage = 1;
        private int itemsPerPage = 9;
        private Owner currentOwner;



        public dataCivili(Owner owner)
        {
            InitializeComponent();


            this.currentOwner = owner;
            this.Loaded += Civillian_Loaded;
            this.DataContext = currentOwner;

        }




        private void Civillian_Loaded(object sender, RoutedEventArgs e)
        {
            // Lấy danh sách cư dân theo OwnerID của chủ hộ đã đăng nhập
            var residents = DatabaseHelper.GetResidentsByOwnerID(currentOwner.OwnerID);

            // Nếu cần phân trang, lấy theo số bản ghi tương ứng:
            var pagedResidents = residents
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            // Gán dữ liệu vào DataGrid (giả sử bạn có DataGrid tên dataGridResidents)
            dataGridResidents.ItemsSource = pagedResidents;
        }


    }
}
