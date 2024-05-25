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
    /// Interaction logic for PhieuNhapUI.xaml
    /// </summary>
    public partial class PhieuNhapUI : UserControl
    {
        public PhieuNhapUI()
        {
            InitializeComponent();
        }
        public void Reset()
        {
            CompanyTxb.Text = "";
            ContentTxb.Text = "";
            DiaChiTxb.Text = "";
            NguoiGiaoTxb.Text = "";
            NguoiNhanTxb.Text = "";
            NoiNhapTxb.Text = "";
            ThuKhoTxb.Text = "";
            TongTxb.Text = "Tổng: ";
            CreateByCbb.SelectedIndex = 0;
            GiamDocCbb.SelectedIndex = 0;
            KeToanTruongCbb.SelectedIndex = 0;
            InvoiceCbb.SelectedIndex = 0;
        }
    }
}
