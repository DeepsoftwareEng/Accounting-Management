using Accounting_Management.Components;
using Accounting_Management.Models;
using Accounting_Management.ViewModel;
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
        List<dynamic> choosenProd = new List<dynamic>();
        List<Prod> selectedProduct = new List<Prod>();
        double TongTien = 0;
        string MaKhachHang = "";
        bool isAdd = true;
        public InvoiceManage()
        {
            InitializeComponent();
            CurrentPageTxb.Text = CurrentPage.ToString();
            InvoiceGrid.AutoGenerateColumns = false;
            InvoiceGrid.ItemsSource = LoadData();
            Drawer.TongTienTxb.Text = "Tổng: 0";
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
            Blur.Visibility = Visibility.Visible;
            Blur.IsEnabled = true;
            Blur.Opacity = 0.4;
            isAdd = true;
            Drawer.Back.Click += Back_Click;
            Drawer.AddProductBtn.Click += AddProductBtn_Click;
            Drawer.DeleteProductBtn.Click += DeleteProductBtn_Click;
            Drawer.ResetBtn.Click += ResetBtn_Click;
            Drawer.CustomerCbb.SelectionChanged += ChooseCustomer;
            Drawer.SaveBtn.Click += SaveAddClick;
        }

        private async void SaveAddClick(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice();
            invoice.MaKhachHang = MaKhachHang;
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime end = start.AddDays(1);    
            int countSameDate = dbcontext.Invoices.Where(c => c.NgayLap >= start && c.NgayLap <= end).Count();
            invoice.MaHoaDon = "INV" + DateTime.Now.Year+ DateTime.Now.Month+ DateTime.Now.Day;
            if (countSameDate < 10)
            {
                invoice.MaHoaDon += "00" + countSameDate;
            }
            else if( countSameDate >= 10 && countSameDate < 100)
            {
                invoice.MaHoaDon += "0" + countSameDate;
            }
            else
            {
                invoice.MaHoaDon += + countSameDate;
            }
            invoice.NgayLap = DateTime.Now;
            invoice.ThanhTien = (float?)TongTien;
            invoice.NoiDung = Drawer.ContentTxb.Text;
            dbcontext.Invoices.Add(invoice);
            await dbcontext.SaveChangesAsync();
            foreach (var i in Drawer.ProductInvoiceGrid.Children)
            {
                dynamic prod = i;
                ProductInvoice temp = new ProductInvoice();
                temp.MaHangHoa = prod.MaHangHoaTxb.Text;
                temp.SoLuong = int.Parse(prod.SoLuongTxb.Text);
                temp.MaHoaDon = invoice.MaHoaDon;
                dbcontext.ProductInvoices.Add(temp);
            }
            await dbcontext.SaveChangesAsync();
            MessageBox.Show("Thành công!");
            Drawer.Reset();
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
            Blur.Visibility = Visibility.Hidden;
            Blur.IsEnabled = false;
            Blur.Opacity = 0;
            Drawer.ProductInvoiceGrid.Children.Clear();
            Drawer.TongTienTxb.Text = "Tổng: 0";
        }

        private void ChooseCustomer(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedCustomer = e.AddedItems[0];
            Drawer.TenKhachHangTxb.Text = selectedCustomer.TenKhachHang;
            MaKhachHang = selectedCustomer.MaKhachHang;
            Drawer.SDTTxb.Text = selectedCustomer.SoDienThoai;
            Drawer.CompanyTxb.Text = selectedCustomer.DonVi;
            Drawer.TaxCodeTxb.Text = selectedCustomer.MaSoThue;
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            Drawer.ProductInvoiceGrid.Children.Clear();
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if(choosenProd.Count() > 0)
            {
                foreach(var i in choosenProd)
                {
                    Drawer.ProductInvoiceGrid.Children.Remove(i);
                }
                choosenProd.Clear();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 sản phẩm cần xóa!");
            }
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            AddProduct add = new AddProduct(this.selectedProduct);
            add.Show();
            add.SaveBtn.Click += (sender, e) =>
            {
                List<string> prodSource = new();
                foreach (var i in Drawer.ProductInvoiceGrid.Children)
                {
                    dynamic tab = i;
                    prodSource.Add(tab.MaHangHoaTxb.Text.ToString());
                }
                List<string> newSource = add.selectedProducts.Select(c => c.MaHangHoa).ToList();
                List<string> keepProd = prodSource.Intersect(newSource).ToList();
                List<string> addProd = newSource.Except(prodSource).ToList();
                List<string> delProd = prodSource.Except(newSource).ToList();
                if (Drawer.ProductInvoiceGrid.Children.Count == 0)
                {
                    foreach (var i in add.selectedProducts)
                    {
                        var temp = CreateTab(i.MaHangHoa, i.TenHangHoa, i.DonGia.ToString());
                        Drawer.ProductInvoiceGrid.Children.Add(temp);
                    }
                }
                else
                {
                    foreach (var i in addProd)
                    {
                        var getProd = dbcontext.Products.Where(c => c.MaHangHoa == i).FirstOrDefault();
                        var temp = CreateTab(getProd.MaHangHoa, getProd.TenHangHoa, getProd.DonGia.ToString());
                        Drawer.ProductInvoiceGrid.Children.Add(temp);
                    }
                    foreach (var i in delProd)
                    {
                        foreach(var j in Drawer.ProductInvoiceGrid.Children)
                        {
                            dynamic tab = j;
                            if(tab.MaHangHoa.Text == i)
                            {
                                Drawer.ProductInvoiceGrid.Children.Remove(tab);
                                TongTien -= int.Parse(tab.SoLuongTxb.Text) * float.Parse(tab.DonGiaTxb.Text);
                                Drawer.TongTienTxb.Text = "Tổng: " + TongTien.ToString();
                            }
                        }
                    }
                }
                this.selectedProduct = add.selectedProducts;
                add.Close();
            };
        }
        private ProductTab CreateTab(string mahanghoa, string tenhanghoa, string dongia)
        {
            ProductTab temp = new ProductTab(mahanghoa, tenhanghoa, dongia);
            temp.Height = 70;
            temp.IncreaseBtn.Click += (sender, e) =>
            {
                int SoLuong = int.Parse(temp.SoLuongTxb.Text);
                double ThanhTien = double.Parse(temp.ThanhTienTxb.Text);
                SoLuong++;
                ThanhTien += double.Parse(temp.DonGiaTxb.Text);
                temp.SoLuongTxb.Text = SoLuong.ToString();
                temp.ThanhTienTxb.Text = ThanhTien.ToString();
                TongTien += double.Parse(temp.DonGiaTxb.Text);
                Drawer.TongTienTxb.Text = "Tổng: " + TongTien.ToString();
            };
            temp.DecreaseBtn.Click += (sender, e) =>
            {
                int SoLuong = int.Parse(temp.SoLuongTxb.Text);
                double ThanhTien = double.Parse(temp.ThanhTienTxb.Text);
                if(SoLuong <= 1)
                {
                    SoLuong = 0;
                    ThanhTien = 0;
                }
                else
                {
                    SoLuong--;
                    ThanhTien -= double.Parse(temp.DonGiaTxb.Text);
                }
                TongTien -= double.Parse(temp.DonGiaTxb.Text);
                Drawer.TongTienTxb.Text = "Tổng: " + TongTien.ToString();
                temp.SoLuongTxb.Text = SoLuong.ToString();
                temp.ThanhTienTxb.Text = ThanhTien.ToString();
            };
            temp.MouseLeftButtonDown += (sender, e) =>
            {
                if(temp.MaHangHoaTxb.FontWeight != FontWeights.Bold)
                {
                    temp.MaHangHoaTxb.FontWeight = FontWeights.Bold;
                    temp.TenHangHoaTxb.FontWeight = FontWeights.Bold;
                    temp.DonGiaTxb.FontWeight = FontWeights.Bold;
                    temp.SoLuongTxb.FontWeight = FontWeights.Bold;
                    temp.ThanhTienTxb.FontWeight = FontWeights.Bold;
                }
                else
                {
                    temp.MaHangHoaTxb.FontWeight = FontWeights.Regular;
                    temp.TenHangHoaTxb.FontWeight = FontWeights.Regular;
                    temp.DonGiaTxb.FontWeight = FontWeights.Regular;
                    temp.SoLuongTxb.FontWeight = FontWeights.Regular;
                    temp.ThanhTienTxb.FontWeight = FontWeights.Regular;
                }
                
                choosenProd.Add(temp);
            };
            return temp;
        }
        private ProductTab CreateTab(string mahanghoa, string tenhanghoa, string dongia, string soluong)
        {
            ProductTab temp = new ProductTab(mahanghoa, tenhanghoa, dongia, soluong);
            temp.Height = 70;
            temp.IncreaseBtn.Click += (sender, e) =>
            {
                int SoLuong = int.Parse(temp.SoLuongTxb.Text);
                double ThanhTien = double.Parse(temp.ThanhTienTxb.Text);
                SoLuong++;
                ThanhTien += double.Parse(temp.DonGiaTxb.Text);
                temp.SoLuongTxb.Text = SoLuong.ToString();
                temp.ThanhTienTxb.Text = ThanhTien.ToString();
                TongTien += double.Parse(temp.DonGiaTxb.Text);
                Drawer.TongTienTxb.Text = "Tổng: " + TongTien.ToString();
            };
            temp.DecreaseBtn.Click += (sender, e) =>
            {
                int SoLuong = int.Parse(temp.SoLuongTxb.Text);
                double ThanhTien = double.Parse(temp.ThanhTienTxb.Text);
                if (SoLuong <= 1)
                {
                    SoLuong = 0;
                    ThanhTien = 0;
                }
                else
                {
                    SoLuong--;
                    ThanhTien -= double.Parse(temp.DonGiaTxb.Text);
                }
                TongTien -= double.Parse(temp.DonGiaTxb.Text);
                Drawer.TongTienTxb.Text = "Tổng: " + TongTien.ToString();
                temp.SoLuongTxb.Text = SoLuong.ToString();
                temp.ThanhTienTxb.Text = ThanhTien.ToString();
            };
            temp.MouseLeftButtonDown += (sender, e) =>
            {
                if (temp.MaHangHoaTxb.FontWeight != FontWeights.Bold)
                {
                    temp.MaHangHoaTxb.FontWeight = FontWeights.Bold;
                    temp.TenHangHoaTxb.FontWeight = FontWeights.Bold;
                    temp.DonGiaTxb.FontWeight = FontWeights.Bold;
                    temp.SoLuongTxb.FontWeight = FontWeights.Bold;
                    temp.ThanhTienTxb.FontWeight = FontWeights.Bold;
                }
                else
                {
                    temp.MaHangHoaTxb.FontWeight = FontWeights.Regular;
                    temp.TenHangHoaTxb.FontWeight = FontWeights.Regular;
                    temp.DonGiaTxb.FontWeight = FontWeights.Regular;
                    temp.SoLuongTxb.FontWeight = FontWeights.Regular;
                    temp.ThanhTienTxb.FontWeight = FontWeights.Regular;
                }

                choosenProd.Add(temp);
            };
            return temp;
        }
        private void EditInvoice(object sender, RoutedEventArgs e)
        {
            var selected = InvoiceGrid.SelectedItem;
            dynamic choosenInvoice = selected;
            string Id = choosenInvoice.MaHoaDon;
            isAdd = false;
            var invoice = dbcontext.Invoices.Where(c => c.MaHoaDon == Id).AsNoTracking().FirstOrDefault();
            var cust = dbcontext.Customers.Where(c=> c.MaKhachHang == invoice.MaKhachHang).AsNoTracking().FirstOrDefault();
            InvoiceId = invoice.MaHoaDon;
            Drawer.CustomerCbb.SelectedValue = invoice.MaKhachHang;
            Drawer.TenKhachHangTxb.Text = cust.TenKhachHang;
            Drawer.CompanyTxb.Text = cust.DonVi;
            Drawer.SDTTxb.Text = cust.SoDienThoai;
            Drawer.ContentTxb.Text = invoice.NoiDung;
            Drawer.TaxCodeTxb.Text = cust.MaSoThue;
            Drawer.TongTienTxb.Text = "Tổng: "+invoice.ThanhTien.ToString();
            var listProd = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == invoice.MaHoaDon).AsNoTracking().ToList();
            foreach ( var i in listProd )
            {
                var product = dbcontext.Products.Where(c=> c.MaHangHoa == i.MaHangHoa).AsNoTracking().FirstOrDefault();
                var temp = CreateTab(i.MaHangHoa, product.TenHangHoa, product.DonGia.ToString(), i.SoLuong.ToString());
                Drawer.ProductInvoiceGrid.Children.Add(temp);
                Prod prod = new Prod("", false, product.MaHangHoa, product.TenHangHoa, product.DonViTinh, product.SoLuong.ToString(), product.DonGia.ToString()); 
                selectedProduct.Add(prod);
            }
            Id = null;
            Drawer.Back.Click += Back_Click;
            Drawer.AddProductBtn.Click += AddProductBtn_Click;
            Drawer.DeleteProductBtn.Click += DeleteProductBtn_Click;
            Drawer.ResetBtn.Click += ResetBtn_Click;
            Drawer.CustomerCbb.SelectionChanged += ChooseCustomer;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveEditClick; ;
        }

        private async void SaveEditClick(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new();
            invoice = dbcontext.Invoices.Where(c => c.MaHoaDon == InvoiceId).FirstOrDefault();
            invoice.ThanhTien = (float?)TongTien;
            invoice.NoiDung = Drawer.ContentTxb.Text;
            dbcontext.Invoices.Update(invoice);
            await dbcontext.SaveChangesAsync();
            var currentProd = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == InvoiceId).ToList();
            List<ProductInvoice> newProd = new();
            foreach (var i in Drawer.ProductInvoiceGrid.Children)
            {
                dynamic prod = i;
                ProductInvoice temp = new ProductInvoice();
                temp.MaHangHoa = prod.MaHangHoaTxb.Text;
                temp.SoLuong = int.Parse(prod.SoLuongTxb.Text);
                temp.MaHoaDon = invoice.MaHoaDon;
                newProd.Add(temp);
            }
            List<ProductInvoice> keepProd = currentProd.Union(newProd).ToList();
            List<ProductInvoice> addProd = newProd.Except(currentProd).ToList();
            List<ProductInvoice> delProd = currentProd.Except(newProd).ToList();
            if (addProd.Count > 0)
            {
                foreach(var i in addProd)
                {
                    dbcontext.ProductInvoices.Add(i);
                }
            }
            if (delProd.Count > 0)
            {
                foreach (var i in delProd)
                {
                    dbcontext.ProductInvoices.Remove(i);
                }
            }
            await dbcontext.SaveChangesAsync();
            MessageBox.Show("Thành công!");
            Drawer.Reset();
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
            Blur.Visibility = Visibility.Hidden;
            Blur.IsEnabled = false;
            Blur.Opacity = 0;
            Drawer.ProductInvoiceGrid.Children.Clear();
            Drawer.TongTienTxb.Text = "Tổng: 0";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
            Blur.Visibility = Visibility.Hidden;
            Blur.IsEnabled = false;
            Blur.Opacity = 0;
            Drawer.ProductInvoiceGrid.Children.Clear();
            this.selectedProduct.Clear();
            Drawer.TongTienTxb.Text = "Tổng: 0";
        }
    }
}
