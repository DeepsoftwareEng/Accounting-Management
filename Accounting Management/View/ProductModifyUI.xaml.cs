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
    /// Interaction logic for ProductModifyUI.xaml
    /// </summary>
    public partial class ProductModifyUI : UserControl
    {
        public ProductModifyUI()
        {
            InitializeComponent();
        }
        public void Reset()
        {
            DonGiaTxb.Clear();
            DonViTinhTxb.Clear();
            ProductNameTxb.Clear();
            SoLuongTxb.Clear();
        }
    }
}
