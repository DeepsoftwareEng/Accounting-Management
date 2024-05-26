using Accounting_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Interaction logic for ThanhToanUI.xaml
    /// </summary>
    public partial class ThanhToanUI : Window
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        public string MaPhieu { get; }
        public string Type { get; }

        public ThanhToanUI()
        {
            InitializeComponent();
        }
        public ThanhToanUI(string MaPhieu, string type)
        {
            InitializeComponent();
            this.MaPhieu = MaPhieu;
            Type = type;
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            dynamic taikhoans;
            if (Type == "IN")
            {
               taikhoans = dbcontext.BankAccounts.Where(c => c.LoaiTaiKhoan == "IN").AsNoTracking().ToList();
            }
            else
            {
                taikhoans = dbcontext.BankAccounts.Where(c => c.LoaiTaiKhoan == "OUT").AsNoTracking().ToList();
            }
            BankAccountCbb.SelectedValuePath = "MaTaiKhoan";
            BankAccountCbb.DisplayMemberPath = "TenTaiKhoan";
            BankAccountCbb.ItemsSource = taikhoans;
        }
        private void Quit(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void PayBtn_Click(object sender, RoutedEventArgs e)
        {
            float TongTien = 0;
            string selectedAccount = BankAccountCbb.SelectedValue.ToString();
            if(Type == "IN")
            {
                var note = dbcontext.PhieuNhaps.Where(c => c.MaPhieu ==  MaPhieu).AsNoTracking().FirstOrDefault();
                note.IsThanhToan = 1;
                dbcontext.PhieuNhaps.Update(note);
                var listProd = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == note.MaHoaDon).AsNoTracking().ToList();
                foreach(var i in listProd)
                {
                    Product prod = dbcontext.Products.Where(c => c.MaHangHoa == i.MaHangHoa).AsNoTracking().FirstOrDefault();
                    prod.SoLuong += i.SoLuong;
                    TongTien -= (float)(i.SoLuong * prod.DonGia);
                    dbcontext.Update(prod);
                }
                var bankAccount = dbcontext.BankAccounts.Where(c => c.MaTaiKhoan == selectedAccount).AsNoTracking().FirstOrDefault();
                bankAccount.TienNo -= TongTien;
                dbcontext.BankAccounts.Update(bankAccount);
                BankLog bankLog = new BankLog();
                bankLog.NoiDung = "Thanh toán phiếu nhập: " + note.MaPhieu;
                bankLog.SoTien = TongTien;
                bankLog.MaTaiKhoan = bankAccount.MaTaiKhoan;
                bankLog.CreateDate = DateTime.Now;
                dbcontext.BankLogs.Add(bankLog);
                var invoice = dbcontext.Invoices.Where(c => c.MaHoaDon == note.MaHoaDon).AsNoTracking().FirstOrDefault();
                invoice.IsPayed = 1;
                dbcontext.Invoices.Update(invoice); 
                dbcontext.SaveChanges();
            }
            else
            {
                var note = dbcontext.PhieuXuats.Where(c => c.MaPhieu == MaPhieu).AsNoTracking().FirstOrDefault();
                note.IsThanhToan = 1;
                dbcontext.PhieuXuats.Update(note);
                var listProd = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == note.MaHoaDon).AsNoTracking().ToList();
                foreach (var i in listProd)
                {
                    Product prod = dbcontext.Products.Where(c => c.MaHangHoa == i.MaHangHoa).AsNoTracking().FirstOrDefault();
                    prod.SoLuong -= i.SoLuong;
                    TongTien += (float)(i.SoLuong * prod.DonGia);
                    dbcontext.Update(prod);
                }
                var bankAccount = dbcontext.BankAccounts.Where(c => c.MaTaiKhoan == selectedAccount).AsNoTracking().FirstOrDefault();
                bankAccount.HienCo += TongTien;
                dbcontext.BankAccounts.Update(bankAccount);
                BankLog bankLog = new BankLog();
                bankLog.NoiDung = "Thanh toán phiếu xuất: " + note.MaPhieu;
                bankLog.SoTien = TongTien;
                bankLog.MaTaiKhoan = bankAccount.MaTaiKhoan;
                bankLog.CreateDate = DateTime.Now;
                dbcontext.BankLogs.Add(bankLog);
                var invoice = dbcontext.Invoices.Where(c => c.MaHoaDon == note.MaHoaDon).AsNoTracking().FirstOrDefault();
                invoice.IsPayed = 1;
                dbcontext.Invoices.Update(invoice);
                dbcontext.SaveChanges();
            }
        }

    }
}
