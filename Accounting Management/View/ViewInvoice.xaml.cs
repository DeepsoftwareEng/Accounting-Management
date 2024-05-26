using Accounting_Management.Models;
using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ViewInvoice.xaml
    /// </summary>
    public partial class ViewInvoice : Window
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        public string MaHoaDon { get; }
        public ViewInvoice()
        {
            InitializeComponent();
            LoadData();
        }
        public ViewInvoice(string maHoaDon)
        {
            InitializeComponent();
            MaHoaDon = maHoaDon;
            LoadData();
        }
        private void LoadData()
        {
            var hoadon = dbcontext.Invoices.Where(c => c.MaHoaDon == MaHoaDon).FirstOrDefault();
            var customer = dbcontext.Customers.Where(c => c.MaKhachHang == hoadon.MaKhachHang).FirstOrDefault();
            TenKhachHangTxb.Text = customer.TenKhachHang;
            DiaChiTxb.Text = customer.DiaChiCuThe;
            var prod = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == MaHoaDon).ToList();
            List<dynamic> prodSource = new List<dynamic>();
            foreach(var i in prod)
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
            }
            ProdGrid.ItemsSource = prodSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".xlsx";
            saveDialog.Filter = "Excel Files|*xlsx;*xls;*xlsm";
            saveDialog.FileName = "Hóa đơn.xlsx";
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    var workbook = new XLWorkbook();
                    var sheet = workbook.Worksheets.Add("Hóa đơn");
                    int rowWrite = 1;
                    IXLCell cell;
                    cell = sheet.Cell(rowWrite, 1);
                    cell.Value = ProdGrid.Name;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 14;
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
                        for (int col = 0; col < ProdGrid.Columns.Count/2; col++)
                        {
                            TextBlock Value = ProdGrid.Columns[col].GetCellContent(item) as TextBlock;
                            cell = sheet.Cell(rowWrite, col + 1);
                            cell.Value = Value.Text;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                        rowWrite++;
                    }

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
