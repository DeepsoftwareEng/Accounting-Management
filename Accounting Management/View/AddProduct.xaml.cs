using Accounting_Management.Models;
using Accounting_Management.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
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
        public readonly ProductViewModel viewModel = new();
        public List<Prod> selectedProducts = new List<Prod>();
        public AddProduct()
        {
            InitializeComponent();
            ProductGrid.AutoGenerateColumns = false;
            this.viewModel.ProdSource = LoadData();
            this.DataContext = this.viewModel;
        }
        public AddProduct(List<Prod> selectedProduct)
        {
            InitializeComponent();
            this.selectedProducts = selectedProduct;
            ProductGrid.AutoGenerateColumns = false;
            this.viewModel.ProdSource = LoadData();
            this.DataContext = this.viewModel;
        }
        private List<Prod> LoadData()
        {
            var data = dbcontext.Products.Count();
            NumberOfPages = data / 20;
            if (data % 20 != 0)
                NumberOfPages++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = dbcontext.Products.AsNoTracking().ToList().Skip(20 * (CurrentPage - 1)).Take(20);
            List<Prod> response = new List<Prod>();
            int stt = (CurrentPage - 1) * 20 + 1;
            foreach (var item in rs)
            {
                
                if (selectedProducts.Where(c=> c.MaHangHoa == item.MaHangHoa).FirstOrDefault() != null)
                {
                    Prod product = new(stt.ToString(), true, item.MaHangHoa, item.TenHangHoa, item.DonViTinh, item.SoLuong.ToString(), item.DonGia.ToString());
                    stt++;
                    response.Add(product);
                }
                else
                {
                    Prod product = new(stt.ToString(), false, item.MaHangHoa, item.TenHangHoa, item.DonViTinh, item.SoLuong.ToString(), item.DonGia.ToString());
                    stt++;
                    response.Add(product);
                }
            }
            return response;
        }
        private List<Prod> FilterData(string filter)
        {
            var data = dbcontext.Products.Where(c => c.MaHangHoa.Contains(filter) || c.TenHangHoa.Contains(filter)).AsNoTracking().ToList();
            if (data.Count() == 0)
                return null;
            NumberOfPages = data.Count / 20;
            if (data.Count % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = data.Skip(20 * (CurrentPage - 1)).Take(20);
            List<Prod> response = new List<Prod>();
            int stt = (CurrentPage - 1) * 20 + 1;
            foreach (var item in rs)
            {
                if (selectedProducts.Where(c => c.MaHangHoa == item.MaHangHoa).FirstOrDefault() != null)
                {
                    Prod product = new(stt.ToString(), true, item.MaHangHoa, item.TenHangHoa, item.DonViTinh, item.SoLuong.ToString(), item.DonGia.ToString());
                    stt++;
                    response.Add(product);
                }
                else
                {
                    Prod product = new(stt.ToString(), false, item.MaHangHoa, item.TenHangHoa, item.DonViTinh, item.SoLuong.ToString(), item.DonGia.ToString());
                    stt++;
                    response.Add(product);
                }
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
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void OnChecked(object sender, RoutedEventArgs e)
        {
            var temp = ProductGrid.SelectedItem;
            dynamic selected = temp;
            if(selected != null)
            {
                Prod prod = new Prod("", true, selected.MaHangHoa, selected.TenHangHoa, selected.DonViTinh, selected.SoLuong, selected.DonGia);
                var checkExists = selectedProducts.Where(c => c.MaHangHoa == selected.MaHangHoa).FirstOrDefault();
                if(checkExists == null)
                    selectedProducts.Add(prod);
            }
        }
        void OnUnChecked(object sender, RoutedEventArgs e)
        {
            var temp = ProductGrid.SelectedItem;
            dynamic selected = temp;
            if(selected != null)
            {
                var removeItem = selectedProducts.Where(x => x.MaHangHoa == selected.MaHangHoa).FirstOrDefault();
                selectedProducts.Remove(removeItem);
            }
            
        }
    }
}
