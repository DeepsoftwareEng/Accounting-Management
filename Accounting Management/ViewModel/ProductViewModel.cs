using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Accounting_Management.ViewModel
{
    public class ProductViewModel : DependencyObject
    {
        public List<Prod> ProdSource {  get; set; }
    }
    public class Prod : DependencyObject
    {
        public Prod(string STT, bool Ischecked, string MaHangHoa,string TenHangHoa, string DonViTinh, string SoLuong, string DonGia)
        {
            this.STT = STT;
            this.IsChecked = Ischecked;
            this.MaHangHoa = MaHangHoa;
            this.DonViTinh = DonViTinh;
            this.SoLuong = SoLuong;
            this.DonGia = DonGia;
            this.TenHangHoa = TenHangHoa;
        }
        public string? STT {  get; set; }
        public bool? IsChecked {  get; set; }
        public string? MaHangHoa {  get; set; }
        public string? TenHangHoa { get; set; }
        public string? DonViTinh { get; set; }
        public string? SoLuong {  get; set; }
        public string? DonGia {  get; set; }
    }
}
