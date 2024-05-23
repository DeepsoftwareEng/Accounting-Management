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

namespace Accounting_Management.Components
{
    /// <summary>
    /// Interaction logic for ProductTab.xaml
    /// </summary>
    public partial class ProductTab : UserControl
    {
        int soLuong = 0;
        double thanhTien = 0;
        double donGia = 0;
        public ProductTab(string MaHangHoa, string TenHangHoa, string DonGia)
        {
            InitializeComponent();
            MaHangHoaTxb.Text = MaHangHoa;
            TenHangHoaTxb.Text = TenHangHoa;
            DonGiaTxb.Text = DonGia;
            this.donGia = double.Parse(DonGia);
            SoLuongTxb.Text = soLuong.ToString();
            ThanhTienTxb.Text = thanhTien.ToString();
        }
        public ProductTab(string MaHangHoa, string TenHangHoa, string DonGia, string SoLuong)
        {
            InitializeComponent();
            MaHangHoaTxb.Text = MaHangHoa;
            TenHangHoaTxb.Text = TenHangHoa;
            DonGiaTxb.Text = DonGia;
            this.donGia = double.Parse(DonGia);
            SoLuongTxb.Text = SoLuong;
            ThanhTienTxb.Text = (int.Parse(SoLuong) * this.donGia).ToString();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Tab.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A9A9A9"));
        }

        private void Tab_MouseLeave(object sender, MouseEventArgs e)
        {
            Tab.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9D9D9"));
        }
    }
}
