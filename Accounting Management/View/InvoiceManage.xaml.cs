using Accounting_Management.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for InvoiceManage.xaml
    /// </summary>
    public partial class InvoiceManage : Page
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        int NumberOfPages;
        int CurrentPage = 1;
        string globalFilter = "";
        string InvoiceId;
        public InvoiceManage()
        {
            InitializeComponent();
            CurrentPageTxb.Text = CurrentPage.ToString();
            InvoiceGrid.AutoGenerateColumns = false;
            InvoiceGrid.ItemsSource = LoadData();
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            var customers = dbcontext.Customers.ToList();
            Drawer.CustomerCbb.DisplayMemberPath = "TenKhachHang";
            Drawer.CustomerCbb.SelectedValuePath = "MaKhachHang";
            Drawer.CustomerCbb.ItemsSource = customers;
        }
        private dynamic LoadData()
        {
            var data = dbcontext.Invoices.AsNoTracking().ToList();
            NumberOfPages = data.Count / 20;
            if (data.Count % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = dbcontext.Invoices.AsNoTracking().ToList().Skip(20 * (CurrentPage - 1)).Take(20);
            List<dynamic> response = new List<dynamic>();
            int stt = (CurrentPage - 1) * 20 + 1;
            foreach (var item in rs)
            {
                var buyer = dbcontext.Customers.Where(c => c.MaKhachHang == item.MaKhachHang).FirstOrDefault();
                var shop = dbcontext.Shops.FirstOrDefault();
                var customer = new
                {
                    STT = stt,
                    MaHoaDon = item.MaHoaDon,
                    DonViBan = shop.TenCongTy,
                    MaSoThueBan = shop.MaSoThue,
                    NguoiMua = buyer.TenKhachHang,
                    DonViMua = buyer.DonVi,
                    MaSoThueMua = buyer.MaSoThue,
                };
                stt++;
                response.Add(customer);
            }
            return response;
        }
        private dynamic FilterData(string filter)
        {
            var data = dbcontext.Invoices.Where(c => c.MaHoaDon.Contains(filter)).AsNoTracking().ToList();
            if (data.Count() == 0)
                return data;
            NumberOfPages = data.Count / 20;
            if (data.Count % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = data.Skip(20 * (CurrentPage - 1)).Take(20);
            List<dynamic> response = new List<dynamic>();
            int stt = (CurrentPage - 1) * 20 + 1;
            foreach (var item in rs)
            {
                var buyer = dbcontext.Customers.Where(c => c.MaKhachHang == item.MaKhachHang).FirstOrDefault();
                var shop = dbcontext.Shops.FirstOrDefault();
                var customer = new
                {
                    STT = stt,
                    MaHoaDon = item.MaHoaDon,
                    DonViBan = shop.TenCongTy,
                    MaSoThueBan = shop.MaSoThue,
                    NguoiMua = buyer.TenKhachHang,
                    DonViMua = buyer.DonVi,
                    MaSoThueMua = buyer.MaSoThue,
                };
                stt++;
                response.Add(customer);
            }
            return response;
        }

        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != 1)
                CurrentPage--;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                InvoiceGrid.ItemsSource = LoadData();
            else
            {
                InvoiceGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < NumberOfPages)
                CurrentPage++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                InvoiceGrid.ItemsSource = LoadData();
            else
            {
                InvoiceGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = NumberOfPages;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                InvoiceGrid.ItemsSource = LoadData();
            else
            {
                InvoiceGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                InvoiceGrid.ItemsSource = LoadData();
            else
            {
                InvoiceGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        private void AddInvoice_Click(object sender, RoutedEventArgs e)
        {
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.AddProductBtn.Click += AddProductBtn_Click;
            Drawer.DeleteProductBtn.Click += DeleteProductBtn_Click;
            Drawer.ResetBtn.Click += ResetBtn_Click;
            Drawer.CustomerCbb.SelectionChanged += ChooseCustomer;
        }

        private void ChooseCustomer(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedCustomer = e.AddedItems[0];
            Drawer.TenKhachHangTxb.Text = selectedCustomer.TenKhachHang;
            Drawer.SDTTxb.Text = selectedCustomer.SoDienThoai;
            Drawer.CompanyTxb.Text = selectedCustomer.DonVi;
            Drawer.TaxCodeTxb.Text = selectedCustomer.MaSoThue;
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            AddProduct add = new AddProduct();
            add.Show();
            List<dynamic> selectedProd = new();
            add.SaveBtn.Click += (sender , e) =>
            {
                var temp = add.ProductGrid.ItemsSource;
                dynamic selectedItem = temp;

            };
        }

        private void EditInvoice(object sender, RoutedEventArgs e)
        {
            var selected = InvoiceGrid.SelectedItem;
            dynamic choosenInvoice = selected;
            string Id = choosenInvoice.MaHoaDon;
            var invoice = dbcontext.Invoices.Where(c => c.MaHoaDon == Id).AsNoTracking().FirstOrDefault();
            InvoiceId = invoice.MaHoaDon;
            //Drawer.CustomerNameTxb.Text = customer.TenKhachHang;
            //Drawer.CompanyTxb.Text = customer.DonVi;
            //Drawer.TaxCodeTxb.Text = customer.MaSoThue;
            //Drawer.AddressTxb.Text = customer.DiaChiCuThe;
            //Drawer.PhoneNumberTxb.Text = customer.SoDienThoai;
            //Drawer.CityCbb.SelectedValue = (int)customer.IdThanhPho;
            //Drawer.StateCbb.SelectedValue = (int)customer.IdHuyen;
            //Drawer.CommuneCbb.SelectedValue = (int)customer.IdXa;
            //customer = null;
            //choosenCustomer = null;
            //Id = null;
            //Drawer.IsEnabled = true;
            //Drawer.Opacity = 1;
            //Drawer.Visibility = Visibility.Visible;
            //Drawer.Back.Click += Back_Click;
            //Drawer.SaveBtn.Click += SaveEditCustomer;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
    }
}
