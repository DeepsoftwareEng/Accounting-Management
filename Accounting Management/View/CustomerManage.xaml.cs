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
    /// Interaction logic for CustomerManage.xaml
    /// </summary>
    
    public partial class CustomerManage : Page
    {
        private class DataGridRow
        {

        }
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        int NumberOfPages;
        int CurrentPage = 1;
        string globalFilter = "";
        string CustomerId;

        public CustomerManage()
        {
            InitializeComponent();
            CurrentPageTxb.Text = CurrentPage.ToString();
            CustomerGrid.AutoGenerateColumns = false;
            CustomerGrid.ItemsSource = LoadData();
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            var citys = dbcontext.Cities.ToList();
            var states = dbcontext.States.ToList();
            var communes = dbcontext.Communes.ToList();
            Drawer.CityCbb.SelectedValuePath = "Id";
            Drawer.CityCbb.DisplayMemberPath = "TenThanhPho";
            Drawer.StateCbb.SelectedValuePath = "Id";
            Drawer.StateCbb.DisplayMemberPath = "TenHuyen";
            Drawer.CommuneCbb.SelectedValuePath = "Id";
            Drawer.CommuneCbb.DisplayMemberPath = "TenXa";
            Drawer.CityCbb.ItemsSource = citys;
            Drawer.StateCbb.ItemsSource= states;
            Drawer.CommuneCbb.ItemsSource = communes;
        }
        private dynamic LoadData()
        {
            var data = dbcontext.Customers.AsNoTracking().ToList();
            NumberOfPages = data.Count / 20;
            if (data.Count % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = dbcontext.Customers.AsNoTracking().ToList().Skip(20 * (CurrentPage - 1)).Take(20);
            List<dynamic> response = new List<dynamic>();
            int stt = (CurrentPage - 1) * 20 +1;
            foreach (var item in rs)
            {
                var customer = new 
                {
                    STT = stt,
                    MaKhachHang = item.MaKhachHang,
                    TenKhachHang = item.TenKhachHang,
                    DonVi = item.DonVi,
                    DiaChiCuthe = item.DiaChiCuThe,
                    SoDienThoai = item.SoDienThoai,
                    MaSoThue = item.MaSoThue,
                };
                stt++;
                response.Add(customer); 
            }
            return response;
        }
        private dynamic FilterData(string filter)
        {
            var data = dbcontext.Customers.Where(c => c.MaKhachHang.Contains(filter) || c.TenKhachHang.Contains(filter) || c.MaSoThue.Contains(filter) || c.SoDienThoai.Contains(filter)).AsNoTracking().ToList();
            if(data.Count() == 0)
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
                var customer = new
                {
                    STT = stt,
                    MaKhachHang = item.MaKhachHang,
                    TenKhachHang = item.TenKhachHang,
                    DonVi = item.DonVi,
                    DiaChiCuthe = item.DiaChiCuThe,
                    SoDienThoai = item.SoDienThoai,
                    MaSoThue = item.MaSoThue
                };
                stt++;
                response.Add(customer);
            }
            return response;
        }
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Drawer.CustomerNameTxb.Text = string.Empty;
            Drawer.CompanyTxb.Text = string.Empty;
            Drawer.TaxCodeTxb.Text = string.Empty;
            Drawer.AddressTxb.Text = string.Empty;
            Drawer.PhoneNumberTxb.Text = string.Empty;
            Drawer.CityCbb.SelectedValue = 1;
            Drawer.StateCbb.SelectedValue = 1;
            Drawer.CommuneCbb.SelectedValue = 1;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveNewCustomer;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }

        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            if(CurrentPage != 1)
                CurrentPage--;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                CustomerGrid.ItemsSource = LoadData();
            else
            {
                CustomerGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = NumberOfPages;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if(string.IsNullOrEmpty(globalFilter))
                CustomerGrid.ItemsSource = LoadData();
            else
            {
                CustomerGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentPage < NumberOfPages)
                CurrentPage++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                CustomerGrid.ItemsSource = LoadData();
            else
            {
                CustomerGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            if (!string.IsNullOrEmpty(FilterTxt.Text.Trim()))
            {
                globalFilter = FilterTxt.Text;
                CustomerGrid.ItemsSource = FilterData(globalFilter);
            }
            else
            {
                globalFilter = "";
                CustomerGrid.ItemsSource = LoadData();
            }
                
        }
        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                CustomerGrid.ItemsSource = LoadData();
            else
            {
                CustomerGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void EditCustomer(object sender, RoutedEventArgs e)
        {
            var selected = CustomerGrid.SelectedItem;
            dynamic choosenCustomer = selected;
            string Id = choosenCustomer.MaKhachHang;
            var customer = dbcontext.Customers.Where(c => c.MaKhachHang == Id).AsNoTracking().FirstOrDefault();
            CustomerId = customer.MaKhachHang;
            Drawer.CustomerNameTxb.Text = customer.TenKhachHang;
            Drawer.CompanyTxb.Text = customer.DonVi;
            Drawer.TaxCodeTxb.Text = customer.MaSoThue;
            Drawer.AddressTxb.Text = customer.DiaChiCuThe;
            Drawer.PhoneNumberTxb.Text = customer.SoDienThoai;
            Drawer.CityCbb.SelectedValue = (int)customer.IdThanhPho;
            Drawer.StateCbb.SelectedValue = (int)customer.IdHuyen;
            Drawer.CommuneCbb.SelectedValue = (int)customer.IdXa;
            customer = null;
            choosenCustomer = null;
            Id = null;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveEditCustomer;
        }

        private void SaveEditCustomer(object sender, RoutedEventArgs e)
        {
            Customer cus = new();
            cus.TenKhachHang = Drawer.CustomerNameTxb.Text;
            cus.DonVi = Drawer.CompanyTxb.Text;
            cus.MaSoThue = Drawer.TaxCodeTxb.Text;
            cus.DiaChiCuThe = Drawer.AddressTxb.Text;
            cus.SoDienThoai = Drawer.PhoneNumberTxb.Text;
            cus.IdThanhPho = (int?)Drawer.CityCbb.SelectedValue;
            cus.IdXa = (int?)Drawer.CommuneCbb.SelectedValue;
            cus.IdHuyen = (int?)Drawer.StateCbb.SelectedValue;
            cus.MaKhachHang = CustomerId;
            CustomerId = "";
            dbcontext.Entry(cus).State = EntityState.Detached;
            try
            {
                dbcontext.Set<Customer>().Update(cus);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                CustomerGrid.ItemsSource = LoadData();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }

        private void SaveNewCustomer(object sender, RoutedEventArgs e)
        {
            Customer cus = new();
            cus.CreateDate = DateTime.Now;
            var today = DateOnly.FromDateTime((DateTime)cus.CreateDate);
            DateTime start = new DateTime(today.Year, today.Month, today.Day);
            var finish = start.AddDays(1);
            int count = dbcontext.Customers.Where(c => c.CreateDate >= start && c.CreateDate <= finish).ToList().Count;
            cus.MaKhachHang = "CUST" + today.Year.ToString() + today.Month.ToString() + today.Day.ToString();
            if (count > 0)
            {
                string countString = "";
                count++;
                if (count < 10)
                    countString += "00" + count;
                else if (count >= 10 && count < 100)
                    countString += "0" + count;
                else
                    countString += count;
                cus.MaKhachHang += countString;
            }
            cus.TenKhachHang = Drawer.CustomerNameTxb.Text;
            cus.DonVi = Drawer.CompanyTxb.Text;
            cus.MaSoThue = Drawer.TaxCodeTxb.Text;
            cus.DiaChiCuThe = Drawer.AddressTxb.Text;
            cus.SoDienThoai = Drawer.PhoneNumberTxb.Text;
            cus.IdThanhPho = (int?)Drawer.CityCbb.SelectedValue;
            cus.IdXa = (int?)Drawer.CommuneCbb.SelectedValue;
            cus.IdHuyen = (int?)Drawer.StateCbb.SelectedValue;
            dbcontext.Entry(cus).State = EntityState.Detached;
            try
            {
                dbcontext.Customers.Add(cus);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                CustomerGrid.ItemsSource = LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
    }
}
