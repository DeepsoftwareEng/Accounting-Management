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
    /// Interaction logic for BankAccoutManage.xaml
    /// </summary>
    public partial class BankAccoutManage : Page
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        int NumberOfPages;
        int CurrentPage = 1;
        string globalFilter = "";
        string bankAccountId;
        public BankAccoutManage()
        {
            InitializeComponent();
            CurrentPageTxb.Text = CurrentPage.ToString();
            BankAccountGrid.AutoGenerateColumns = false;
            BankAccountGrid.ItemsSource = LoadData();
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            var banks = dbcontext.Banks.ToList();
            List<dynamic> accountType = new List<dynamic>
            {
                new {
                    Id = "IN",
                    LoaiTaiKhoan = "Tài khoản thu"
                },
                new {
                    Id = "OUT",
                    LoaiTaiKhoan = "Tài khoản chi"
                },
            };
            Drawer.AccountTypeCbb.SelectedValuePath = "Id";
            Drawer.AccountTypeCbb.DisplayMemberPath = "LoaiTaiKhoan";
            Drawer.BankCbb.SelectedValuePath = "IdNganHang";
            Drawer.BankCbb.DisplayMemberPath = "TenNganHang";
            Drawer.AccountTypeCbb.ItemsSource = accountType;
            Drawer.BankCbb.ItemsSource = banks;

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Drawer.HienCoTxb.Text = "";
            Drawer.NoTxb.Text = "";
            Drawer.SoTaiKhoanTxb.Text = "";
            Drawer.TenTaiKhoanTxb.Text = "";
            Drawer.AccountTypeCbb.SelectedValue = 1;
            Drawer.BankCbb.SelectedValue = 1;
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
        private void AddBankAcount_Click(object sender, RoutedEventArgs e)
        {
            Drawer.HienCoTxb.Text = "";
            Drawer.NoTxb.Text = "";
            Drawer.SoTaiKhoanTxb.Text = "";
            Drawer.TenTaiKhoanTxb.Text = "";
            Drawer.AccountTypeCbb.SelectedValue = 1;
            Drawer.BankCbb.SelectedValue = 1;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveAddClick;
        }

        private void SaveAddClick(object sender, RoutedEventArgs e)
        {
            BankAccount bacc = new BankAccount();
            bacc.CreateDate = DateTime.Now;
            var today = DateOnly.FromDateTime((DateTime)bacc.CreateDate);
            DateTime start = new DateTime(today.Year, today.Month, today.Day);
            var finish = start.AddDays(1);
            int count = dbcontext.BankAccounts.Where(c => c.CreateDate >= start && c.CreateDate <= finish).ToList().Count;
            bacc.MaTaiKhoan = "BACC" + today.Year.ToString() + today.Month.ToString() + today.Day.ToString();
            if (count >= 0)
            {
                string countString = "";
                count++;
                if (count < 10)
                    countString += "00" + count;
                else if (count >= 10 && count < 100)
                    countString += "0" + count;
                else
                    countString += count;
                bacc.MaTaiKhoan += countString;
            }
            bacc.TenTaiKhoan = Drawer.TenTaiKhoanTxb.Text;
            bacc.SoTaiKhoan = Drawer.SoTaiKhoanTxb.Text;
            bacc.LoaiTaiKhoan = Drawer.AccountTypeCbb.SelectedValue.ToString();
            bacc.HienCo = int.Parse(Drawer.HienCoTxb.Text);
            bacc.TienNo = int.Parse(Drawer.NoTxb.Text);
            bacc.IdNganHang = int.Parse(Drawer.BankCbb.SelectedValue.ToString());
            dbcontext.Entry(bacc).State = EntityState.Detached;
            try
            {
                dbcontext.BankAccounts.Add(bacc);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                BankAccountGrid.ItemsSource = LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
        private void EditAccount(object sender, RoutedEventArgs e)
        {
            var selected = BankAccountGrid.SelectedItem;
            dynamic choosenBAcc = selected;
            string Id = choosenBAcc.MaTaiKhoan;
            var bankaccount = dbcontext.BankAccounts.Where(c => c.MaTaiKhoan == Id).AsNoTracking().FirstOrDefault();
            bankAccountId = bankaccount.MaTaiKhoan;
            Drawer.HienCoTxb.Text = bankaccount.HienCo.ToString();
            Drawer.NoTxb.Text = bankaccount.TienNo.ToString();
            Drawer.SoTaiKhoanTxb.Text = bankaccount.SoTaiKhoan;
            Drawer.TenTaiKhoanTxb.Text = bankaccount.TenTaiKhoan;
            Drawer.AccountTypeCbb.SelectedValue = bankaccount.LoaiTaiKhoan;
            Drawer.BankCbb.SelectedValue = bankaccount.IdNganHang;
            bankaccount = null;
            choosenBAcc = null;
            Id = null;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveEditAccount;
        }

        private void SaveEditAccount(object sender, RoutedEventArgs e)
        {
            BankAccount bacc = new BankAccount();
            bacc.TenTaiKhoan = Drawer.TenTaiKhoanTxb.Text;
            bacc.SoTaiKhoan = Drawer.SoTaiKhoanTxb.Text;
            bacc.LoaiTaiKhoan = Drawer.AccountTypeCbb.SelectedValue.ToString();
            bacc.HienCo = int.Parse(Drawer.HienCoTxb.Text);
            bacc.TienNo = int.Parse(Drawer.NoTxb.Text);
            bacc.IdNganHang = int.Parse(Drawer.BankCbb.SelectedValue.ToString());
            bacc.MaTaiKhoan = bankAccountId;
            bankAccountId = "";
            dbcontext.Entry(bacc).State = EntityState.Detached;
            try
            {
                dbcontext.Set<BankAccount>().Update(bacc);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                BankAccountGrid.ItemsSource = LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
        }
        private dynamic LoadData()
        {
            var data = dbcontext.BankAccounts.AsNoTracking().ToList();
            NumberOfPages = data.Count / 20;
            if (data.Count % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = dbcontext.BankAccounts.AsNoTracking().ToList().Skip(20 * (CurrentPage - 1)).Take(20);
            List<dynamic> response = new List<dynamic>();
            int stt = (CurrentPage - 1) * 20 + 1;
            foreach (var item in rs)
            {
                var bank = dbcontext.Banks.Where(c => c.IdNganHang == item.IdNganHang).FirstOrDefault();
                if(item.LoaiTaiKhoan == "IN")
                {
                    var bankacc = new
                    {
                        MaTaiKhoan = item.MaTaiKhoan,
                        STT = stt,
                        SoTaiKhoan = item.SoTaiKhoan,
                        HienCo = item.HienCo,
                        No = item.TienNo,
                        TenTaiKhoan = item.TenTaiKhoan,
                        LoaiTaiKhoan = "Thu",
                    };
                    stt++;
                    response.Add(bankacc);
                }
                else
                {
                    var bankacc = new
                    {
                        MaTaiKhoan = item.MaTaiKhoan,
                        STT = stt,
                        SoTaiKhoan = item.SoTaiKhoan,
                        HienCo = item.HienCo,
                        No = item.TienNo,
                        TenTaiKhoan = item.TenTaiKhoan,
                        LoaiTaiKhoan = "Chi",
                    };
                    stt++;
                    response.Add(bankacc);
                }
                
            }
            return response;
        }
        private dynamic FilterData(string filter)
        {
            var data = dbcontext.BankAccounts.Where(c => c.SoTaiKhoan.Contains(filter) || c.LoaiTaiKhoan.Contains(filter)).AsNoTracking().ToList();
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
                var bank = dbcontext.Banks.Where(c => c.IdNganHang == item.IdNganHang).FirstOrDefault();
                var bankacc = new
                {
                    STT = stt,
                    SoTaiKhoan = item.SoTaiKhoan,
                    HienCo = item.HienCo,
                    No = item.TienNo,
                    TenNganHang = bank,
                    LoaiTaiKhoan = item.LoaiTaiKhoan,
                };
                stt++;
                response.Add(bankacc);
            }
            return response;
        }
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != 1)
                CurrentPage--;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                BankAccountGrid.ItemsSource = LoadData();
            else
            {
                BankAccountGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = NumberOfPages;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                BankAccountGrid.ItemsSource = LoadData();
            else
            {
                BankAccountGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < NumberOfPages)
                CurrentPage++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                BankAccountGrid.ItemsSource = LoadData();
            else
            {
                BankAccountGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            if (!string.IsNullOrEmpty(FilterTxt.Text.Trim()))
            {
                globalFilter = FilterTxt.Text;
                BankAccountGrid.ItemsSource = FilterData(globalFilter);
            }
            else
            {
                globalFilter = "";
                BankAccountGrid.ItemsSource = LoadData();
            }

        }
        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                BankAccountGrid.ItemsSource = LoadData();
            else
            {
                BankAccountGrid.ItemsSource = FilterData(globalFilter);
            }
        }
    }
}
