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
    /// Interaction logic for ProductManage.xaml
    /// </summary>
    public partial class ProductManage : Page
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        int NumberOfPages;
        int CurrentPage = 1;
        string ProductId;
        string globalFilter="";
        public ProductManage()
        {
            InitializeComponent();
            CurrentPageTxb.Text = CurrentPage.ToString();
            ProductGrid.AutoGenerateColumns = false;
            ProductGrid.ItemsSource = LoadData();
        }
        private dynamic LoadData()
        {
            var data = dbcontext.Products.Count();
            NumberOfPages = data / 20;
            if (data % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = dbcontext.Products.AsNoTracking().ToList().Skip(20 * (CurrentPage - 1)).Take(20);
            List<dynamic> response = new List<dynamic>();
            int stt = (CurrentPage - 1) * 20 + 1;
            foreach (var item in rs)
            {
                var product = new
                {
                    STT = stt,
                    MaHangHoa = item.MaHangHoa,
                    TenHangHoa = item.TenHangHoa,
                    DonViTinh = item.DonViTinh,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                };
                stt++;
                response.Add(product);
            }
            return response;
        }
        private dynamic FilterData(string filter)
        {
            var data = dbcontext.Products.Where(c => c.MaHangHoa.Contains(filter) || c.TenHangHoa.Contains(filter)).AsNoTracking().ToList();
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
                var product = new
                {
                    STT = stt,
                    MaHangHoa = item.MaHangHoa,
                    TenHangHoa = item.TenHangHoa,
                    DonViTinh = item.DonViTinh,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                };
                stt++;
                response.Add(product);
            }
            return response;
        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Drawer.ProductNameTxb.Text = string.Empty;
            Drawer.DonViTinhTxb.Text = string.Empty;
            Drawer.DonGiaTxb.Text = string.Empty;
            Drawer.SoLuongTxb.Text = string.Empty;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveNewProduct;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Drawer.Reset();
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                ProductGrid.ItemsSource = LoadData();
            else
            {
                ProductGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != 1)
                CurrentPage--;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                ProductGrid.ItemsSource = LoadData();
            else
            {
                ProductGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = NumberOfPages;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                ProductGrid.ItemsSource = LoadData();
            else
            {
                ProductGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        private void EditProduct(object sender, RoutedEventArgs e)
        {
            var selected = ProductGrid.SelectedItem;
            dynamic choosenCustomer = selected;
            string Id = choosenCustomer.MaHangHoa;
            var product = dbcontext.Products.Where(c => c.MaHangHoa == Id).AsNoTracking().FirstOrDefault();
            ProductId = product.MaHangHoa;
            Drawer.ProductNameTxb.Text = product.TenHangHoa;
            Drawer.DonViTinhTxb.Text = product.DonViTinh;
            Drawer.DonGiaTxb.Text = product.DonGia.ToString();
            Drawer.SoLuongTxb.Text = product.SoLuong.ToString();
            product = null;
            choosenCustomer = null;
            Id = null;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveEditProduct;
        }
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < NumberOfPages)
                CurrentPage++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                ProductGrid.ItemsSource = LoadData();
            else
            {
                ProductGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        private void SaveEditProduct(object sender, RoutedEventArgs e)
        {
            Product prod = new();
            prod.TenHangHoa = Drawer.ProductNameTxb.Text;
            prod.DonViTinh = Drawer.DonViTinhTxb.Text;
            prod.DonGia = float.Parse(Drawer.DonGiaTxb.Text);
            prod.SoLuong = int.Parse(Drawer.SoLuongTxb.Text);
            prod.MaHangHoa = ProductId;
            ProductId = "";
            dbcontext.Entry(prod).State = EntityState.Detached;
            try
            {
                dbcontext.Set<Product>().Update(prod);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ProductGrid.ItemsSource = LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.Reset();
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }

        private void SaveNewProduct(object sender, RoutedEventArgs e)
        {
            Product prod = new();
            prod.TenHangHoa = Drawer.ProductNameTxb.Text;
            prod.DonViTinh = Drawer.DonViTinhTxb.Text;
            prod.DonGia = float.Parse(Drawer.DonGiaTxb.Text);
            prod.SoLuong = int.Parse(Drawer.SoLuongTxb.Text);
            prod.CreateDate = DateTime.Now;
            var today = DateOnly.FromDateTime((DateTime)prod.CreateDate);
            DateTime start = new DateTime(today.Year, today.Month, today.Day);
            var finish = start.AddDays(1);
            int count = dbcontext.Customers.Where(c => c.CreateDate >= start && c.CreateDate <= finish).ToList().Count;
            prod.MaHangHoa = "CUST" + today.Year.ToString() + today.Month.ToString() + today.Day.ToString();
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
                prod.MaHangHoa += countString;
            }
            dbcontext.Entry(prod).State = EntityState.Detached;
            try
            {
                dbcontext.Products.Add(prod);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ProductGrid.ItemsSource = LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.Reset();
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            if (!string.IsNullOrEmpty(FilterTxt.Text.Trim()))
            {
                globalFilter = FilterTxt.Text;
                ProductGrid.ItemsSource = FilterData(globalFilter);
            }
            else
            {
                globalFilter = "";
                ProductGrid.ItemsSource = LoadData();
            }

        }
    }
}
