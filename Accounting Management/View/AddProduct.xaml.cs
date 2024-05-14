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
using System.Windows.Shapes;

namespace Accounting_Management.View
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        int NumberOfPages;
        int CurrentPage = 1;
        string ProductId;
        string globalFilter = "";
        public AddProduct()
        {
            InitializeComponent();
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
        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            CurrentPageTxb.Text = CurrentPage.ToString();
            ProductGrid.ItemsSource = LoadData();
        }
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != 1)
                CurrentPage--;
            CurrentPageTxb.Text = CurrentPage.ToString();
            ProductGrid.ItemsSource = LoadData();
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
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < NumberOfPages)
                CurrentPage++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            ProductGrid.ItemsSource = LoadData();
        }
    }
}
