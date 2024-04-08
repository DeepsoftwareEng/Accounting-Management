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

namespace Accounting_Management.View
{
    /// <summary>
    /// Interaction logic for NoteManage.xaml
    /// </summary>
    public partial class NoteManage : Page
    {
        public NoteManage()
        {
            InitializeComponent();
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            PhieuNhap.IsEnabled = true;
            PhieuNhap.Opacity = 1;
            PhieuNhap.Visibility = Visibility.Visible;
            PhieuNhap.Back.Click += Back_Click;
            PhieuXuat.Back.Click += Back_Click;
            PhieuNhap.ChangeToPhieuXuatBtn.Click += ChangeToPhieuXuatBtn_Click;
            PhieuXuat.ChangeToPhieuNhapBtn.Click += ChangeToPhieuNhapBtn_Click;
        }

        private void ChangeToPhieuNhapBtn_Click(object sender, RoutedEventArgs e)
        {
            PhieuXuat.IsEnabled = false;
            PhieuXuat.Opacity = 0;
            PhieuXuat.Visibility = Visibility.Hidden;
            PhieuNhap.IsEnabled = true;
            PhieuNhap.Opacity = 1;
            PhieuNhap.Visibility = Visibility.Visible;
        }

        private void ChangeToPhieuXuatBtn_Click(object sender, RoutedEventArgs e)
        {
            PhieuNhap.IsEnabled = false;
            PhieuNhap.Opacity = 0;
            PhieuNhap.Visibility = Visibility.Hidden;
            PhieuXuat.IsEnabled = true;
            PhieuXuat.Opacity = 1;
            PhieuXuat.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PhieuNhap.IsEnabled = false;
            PhieuNhap.Opacity = 0;
            PhieuNhap.Visibility = Visibility.Hidden;
            PhieuXuat.IsEnabled = false;
            PhieuXuat.Opacity = 0;
            PhieuXuat.Visibility = Visibility.Hidden;
        }
    }
}
