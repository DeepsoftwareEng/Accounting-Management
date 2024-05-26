using Accounting_Management.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Accounting_Management.View
{
    /// <summary>
    /// Interaction logic for ViewOutNote.xaml
    /// </summary>
    public partial class ViewOutNote : Window
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        string TongTien = "";
        public string MaPhieu { get; set; }
        public ViewOutNote()
        {
            InitializeComponent();
        }
        public ViewOutNote(string maPhieu)
        {
            InitializeComponent();
            MaPhieu = maPhieu;
            LoadData();
        }
        private void LoadData()
        {
            var phieuXuat = dbcontext.PhieuXuats.Where(c => c.MaPhieu == MaPhieu).FirstOrDefault();
            var shop = dbcontext.Shops.FirstOrDefault();
            var prod = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == phieuXuat.MaHoaDon).ToList();
            var ketoantruong = dbcontext.Employees.Where(c => c.MaNhanVien == phieuXuat.KeToanTruong).FirstOrDefault();
            var giamdoc = dbcontext.Employees.Where(c => c.MaNhanVien == phieuXuat.GiamDoc).FirstOrDefault();
            var nguoiLap = dbcontext.Employees.Where(c => c.MaNhanVien == phieuXuat.NguoiLap).FirstOrDefault();
            List<dynamic> prodSource = new List<dynamic>();
            float tongtien = 0;
            foreach (var i in prod)
            {
                var temp = dbcontext.Products.Where(c => c.MaHangHoa == i.MaHangHoa).FirstOrDefault();
                var item = new
                {
                    MaHangHoa = temp.MaHangHoa,
                    TenHangHoa = temp.TenHangHoa,
                    SoLuong = i.SoLuong.ToString(),
                    DonGia = temp.DonGia,
                    ThanhTien = i.SoLuong * temp.DonGia
                };
                prodSource.Add(item);
                float thanhtien = (float)(i.SoLuong * temp.DonGia);
                tongtien += thanhtien;
            }
            TongTien = tongtien.ToString();
            ProdGrid.ItemsSource = prodSource;
            ShopAddressTxb.Text = shop.MaSoThue;
            ShopNameTxb.Text = shop.TenCongTy;
            DateTime t = (DateTime)phieuXuat.NgayLap;
            DateTxb.Text = "Ngày "+ t.Day + " tháng "+t.Month + " năm "+ t.Year;
            MaPhieuTxb.Text = phieuXuat.MaPhieu;
            NguoiNhanTxb.Text = phieuXuat.NguoiNhan;
            ContentTxb.Text = phieuXuat.LiDo;
            NguoiLapKyTxb.Text = nguoiLap.TenNhanVien;
            NguoiNhanKyTxb.Text = phieuXuat.NguoiNhan;
            ThuKhoKyTxb.Text = phieuXuat.ThuKho;
            KeToantruongKyTxb.Text = ketoantruong.TenNhanVien;
            GiamDocKyTxb.Text = giamdoc.TenNhanVien;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".xlsx";
            saveDialog.Filter = "Excel Files|*xlsx;*xls;*xlsm";
            saveDialog.FileName = "Phiếu xuất.xlsx";
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    var workbook = new XLWorkbook();
                    var sheet = workbook.Worksheets.Add("Phiếu xuất");
                    int rowWrite = 6;
                    IXLCell cell;
                    #region Header
                    cell = sheet.Cell(2, ProdGrid.Columns.Count / 4);
                    cell.Value = "Phiếu xuất";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(2, 1);
                    cell.Value = "Mã phiếu";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(3, 1);
                    cell.Value = "Tên người nhận";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(4, 1);
                    cell.Value = "Lý do";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(5, 1);
                    cell.Value = "Nhập tại";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(2, 2);
                    cell.Value = MaPhieuTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(3, 2);
                    cell.Value = NguoiNhanTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(4, 2);
                    cell.Value = ContentTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(5, 2);
                    cell.Value = "";
                    cell.Style.Font.FontSize = 14;
                    #endregion
                    rowWrite++;
                    for (int col = 0; col < ProdGrid.Columns.Count / 2; col++)
                    {
                        cell = sheet.Cell(rowWrite, col + 1);
                        cell.Value = ProdGrid.Columns[col].Header.ToString();
                        cell.Style.Fill.BackgroundColor = XLColor.Aqua;
                        cell.Style.Font.Bold = true;
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                    rowWrite++;

                    foreach (var item in ProdGrid.Items)
                    {
                        for (int col = 0; col < ProdGrid.Columns.Count / 2; col++)
                        {
                            TextBlock Value = ProdGrid.Columns[col].GetCellContent(item) as TextBlock;
                            cell = sheet.Cell(rowWrite, col + 1);
                            cell.Value = Value.Text;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                        rowWrite++;
                    }
                    #region Footer
                    cell = sheet.Cell(rowWrite, ProdGrid.Columns.Count / 4);
                    cell.Value = "Tổng tiền";
                    cell.Style.Font.FontSize = 14;
                    cell.Style.Font.FontColor = XLColor.Red;
                    cell.Style.Font.Bold = true;
                    cell = sheet.Cell(rowWrite, ProdGrid.Columns.Count / 4 + 1);
                    cell.Value = TongTien; 
                    cell.Style.Font.FontSize = 14;
                    cell.Style.Font.FontColor = XLColor.Red;
                    cell.Style.Font.Bold = true;
                    rowWrite++;
                    cell = sheet.Cell(rowWrite, 1);
                    cell.Value = "Người lập";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 2);
                    cell.Value = "Người nhận";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 3);
                    cell.Value = "Thủ kho";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 4);
                    cell.Value = "Kế toán trưởng";
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 5);
                    cell.Value = "Giám đốc";
                    cell.Style.Font.FontSize = 14;

                    rowWrite++;

                    cell = sheet.Cell(rowWrite, 1);
                    cell.Value = NguoiLapKyTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 2);
                    cell.Value = NguoiNhanKyTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 3);
                    cell.Value = ThuKhoKyTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 4);
                    cell.Value = KeToantruongKyTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    cell = sheet.Cell(rowWrite, 5);
                    cell.Value = GiamDocKyTxb.Text;
                    cell.Style.Font.FontSize = 14;
                    #endregion
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export to excel successfull", "Message");
                    workbook.Dispose();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
