using QuanLyChungCu;
using System;
using System.Text.RegularExpressions; // Dùng để kiểm tra SDT bằng Regex
using System.Windows;
using System.Windows.Input;

namespace QuanLyChungCu
{
    public partial class Change : Window
    {
        //private Member member;
        public bool isChange;

        //public Change(Member selectedMember)
        //{
        //    InitializeComponent();
        //    member = selectedMember;

        //    txtName.Text = member.Name;
        //    txtSDT.Text = member.SDT;
        //    txtSoNguoiO.Text = member.SoNguoiO;
        //    txtViTri.Text = member.vitri;
        //}

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButunDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string soNguoiO = txtSoNguoiO.Text.Trim();
            string vitri = txtViTri.Text.Trim();

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(sdt) ||
                string.IsNullOrEmpty(soNguoiO) ||
                string.IsNullOrEmpty(vitri))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            if (!Regex.IsMatch(sdt, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại phải có 10 chữ số và bắt đầu bằng 0!");
                return;
            }

            if (!int.TryParse(soNguoiO, out int soNguoi) || soNguoi <= 0)
            {
                MessageBox.Show("Số người ở phải là số nguyên dương!");
                return;
            }

            //member.Name = name;
            //member.SDT = sdt;
            //member.SoNguoiO = soNguoiO;
            //member.vitri = vitri;

            this.isChange = true;
            this.Close();
        }
    }
}

