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
    /// Interaction logic for PhieuXuatUI.xaml
    /// </summary>
    public partial class PhieuXuatUI : UserControl
    {
        public PhieuXuatUI()
        {
            InitializeComponent();
        }
        public void Reset()
        {
            CompanyTxb.Clear();
            ContentTxb.Clear();
            DiaChiTxb.Clear();
            NguoiGiaoTxb.Clear();
            NguoiNhanTxb.Clear();
            ThuKhoTxb.Clear();
            TongTxb.Text = "Tổng: ";
            GiamDocCbb.SelectedIndex = 0;
            InvoiceCbb.SelectedIndex = 0;
            KeToanTruongCbb.SelectedIndex = 0;
            NguoiLapCbb.SelectedIndex = 0;
        }
    }
}
