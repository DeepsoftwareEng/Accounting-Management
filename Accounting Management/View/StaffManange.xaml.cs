using Accounting_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for StaffManange.xaml
    /// </summary>
    public partial class StaffManange : Page
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        int NumberOfPages;
        int CurrentPage = 1;
        string globalFilter = "";
        string StaffId;
        public StaffManange()
        {
            InitializeComponent();
            CurrentPageTxb.Text = CurrentPage.ToString();
            StaffGrid.AutoGenerateColumns = false;
            StaffGrid.ItemsSource = LoadData();
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            var citys = dbcontext.Cities.ToList();
            var states = dbcontext.States.ToList();
            var communes = dbcontext.Communes.ToList();
            var roles = dbcontext.Roles.ToList();
            List<string> genders = new List<string>
            {
                "",
                "Nam",
                "Nữ"
            };
            Drawer.CityCbb.SelectedValuePath = "Id";
            Drawer.CityCbb.DisplayMemberPath = "TenThanhPho";
            Drawer.StateCbb.SelectedValuePath = "Id";
            Drawer.StateCbb.DisplayMemberPath = "TenHuyen";
            Drawer.CommuneCbb.SelectedValuePath = "Id";
            Drawer.CommuneCbb.DisplayMemberPath = "TenXa";
            Drawer.RoleCbb.SelectedValuePath = "IdChucVu";
            Drawer.RoleCbb.DisplayMemberPath = "TenChucVu";
            Drawer.GenderCbb.ItemsSource = genders;
            Drawer.CityCbb.ItemsSource = citys;
            Drawer.StateCbb.ItemsSource = states;
            Drawer.CommuneCbb.ItemsSource= communes;
            Drawer.RoleCbb.ItemsSource = roles;
        }
        private void AddStaff_Click(object sender, RoutedEventArgs e)
        {
            Drawer.StaffNameTxb.Text = "";
            Drawer.AddressTxb.Text = "";
            Drawer.SDTTxb.Text = "";
            Drawer.DobTxb.Clear();
            Drawer.CityCbb.SelectedValue = 1;
            Drawer.StateCbb.SelectedValue = 1;
            Drawer.CommuneCbb.SelectedValue = 1;
            Drawer.RoleCbb.SelectedValue = 1;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveAddClick;
        }

        private void SaveAddClick(object sender, RoutedEventArgs e)
        {
            Employee emp = new Employee();
            emp.CreateDate = DateTime.Now;
            var today = DateOnly.FromDateTime((DateTime)emp.CreateDate);
            DateTime start = new DateTime(today.Year, today.Month, today.Day);
            var finish = start.AddDays(1);
            int count = dbcontext.Employees.Where(c => c.CreateDate >= start && c.CreateDate <= finish).ToList().Count;
            DateTime dob = DateTime.ParseExact(Drawer.DobTxb.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            emp.MaNhanVien = "STAFF" + today.Year.ToString() + today.Month.ToString() + today.Day.ToString();
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
                emp.MaNhanVien += countString;
            }
            emp.TenNhanVien = Drawer.StaffNameTxb.Text;
            emp.SoDienThoai = Drawer.SDTTxb.Text;
            emp.DiaChiCuThe = Drawer.AddressTxb.Text;
            emp.IdThanhPho = (int?)Drawer.CityCbb.SelectedValue;
            emp.NgaySinh = dob;
            emp.IdXa = (int?)Drawer.CommuneCbb.SelectedValue;
            emp.IdHuyen = (int?)Drawer.StateCbb.SelectedValue;
            emp.IdChucVu = (int?)Drawer.RoleCbb.SelectedValue;
            emp.GioiTinh = Drawer.GenderCbb.SelectedItem.ToString();
            dbcontext.Entry(emp).State = EntityState.Detached;
            try
            {
                dbcontext.Employees.Add(emp);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                StaffGrid.ItemsSource = LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
        private void EditStaff(object sender, RoutedEventArgs e)
        {
            var selected = StaffGrid.SelectedItem;
            dynamic choosenStaff = selected;
            string Id = choosenStaff.MaNhanVien;
            var staff = dbcontext.Employees.Where(c => c.MaNhanVien == Id).AsNoTracking().FirstOrDefault();
            StaffId = staff.MaNhanVien;
            Drawer.StaffNameTxb.Text = staff.TenNhanVien;
            Drawer.AddressTxb.Text = staff.DiaChiCuThe;
            Drawer.SDTTxb.Text = staff.SoDienThoai;
            Drawer.CityCbb.SelectedValue = staff.IdThanhPho;
            Drawer.DobTxb.Text = staff.NgaySinh.ToString();
            Drawer.StateCbb.SelectedValue = staff.IdHuyen;
            Drawer.CommuneCbb.SelectedValue = staff.IdXa;
            Drawer.RoleCbb.SelectedValue = staff.IdChucVu;
            Drawer.GenderCbb.SelectedItem = staff.GioiTinh;
            staff = null;
            choosenStaff = null;
            Id = null;
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
            Drawer.SaveBtn.Click += SaveEditStaff;
        }

        private void SaveEditStaff(object sender, RoutedEventArgs e)
        {
            DateTime dob = DateTime.ParseExact(Drawer.DobTxb.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Employee emp = new();
            emp.TenNhanVien = Drawer.StaffNameTxb.Text;
            emp.SoDienThoai = Drawer.SDTTxb.Text;
            emp.DiaChiCuThe = Drawer.AddressTxb.Text;
            emp.IdThanhPho = (int?)Drawer.CityCbb.SelectedValue;
            emp.IdXa = (int?)Drawer.CommuneCbb.SelectedValue;
            emp.IdHuyen = (int?)Drawer.StateCbb.SelectedValue;
            emp.IdChucVu = (int?)Drawer.RoleCbb.SelectedValue;
            emp.GioiTinh = Drawer.GenderCbb.SelectedItem.ToString();
            emp.NgaySinh = dob;
            emp.MaNhanVien = StaffId;
            StaffId = "";
            dbcontext.Entry(emp).State = EntityState.Detached;
            try
            {
                dbcontext.Set<Employee>().Update(emp);
                dbcontext.SaveChanges();
                MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                StaffGrid.ItemsSource = LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
        private dynamic LoadData()
        {
            var data = dbcontext.Employees.AsNoTracking().ToList();
            NumberOfPages = data.Count / 20;
            if (data.Count % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            var rs = dbcontext.Employees.AsNoTracking().ToList().Skip(20 * (CurrentPage - 1)).Take(20);
            List<dynamic> response = new List<dynamic>();
            int stt = (CurrentPage - 1) * 20 + 1;
            foreach (var item in rs)
            {
                var chucVu = dbcontext.Roles.Where(c => c.IdChucVu == item.IdChucVu).AsNoTracking().FirstOrDefault();
                var customer = new
                {
                    STT = stt,
                    MaNhanVien = item.MaNhanVien,
                    TenNhanVien = item.TenNhanVien,
                    NgaySinh = item.NgaySinh.ToString(),
                    SoDienThoai = item.SoDienThoai,
                    DiaChi = item.DiaChiCuThe,
                    ChucVu = chucVu.TenChucVu,
                };
                stt++;
                response.Add(customer);
            }
            return response;
        }
        private dynamic FilterData(string filter)
        {
            var data = dbcontext.Employees.Where(c => c.MaNhanVien.Contains(filter) || c.TenNhanVien.Contains(filter)).AsNoTracking().ToList();
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
                var chucVu = dbcontext.Roles.Where(c => c.IdChucVu == item.IdChucVu).AsNoTracking().FirstOrDefault();
                var customer = new
                {
                    STT = stt,
                    MaNhanVien = item.MaNhanVien,
                    TenNhanVien = item.TenNhanVien,
                    NgaySinh = item.NgaySinh.ToString(),
                    SoDienThoai = item.SoDienThoai,
                    DiaChi = item.DiaChiCuThe,
                    ChuCVu = chucVu,
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
                StaffGrid.ItemsSource = LoadData();
            else
            {
                StaffGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = NumberOfPages;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                StaffGrid.ItemsSource = LoadData();
            else
            {
                StaffGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < NumberOfPages)
                CurrentPage++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                StaffGrid.ItemsSource = LoadData();
            else
            {
                StaffGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            if (!string.IsNullOrEmpty(FilterTxt.Text.Trim()))
            {
                globalFilter = FilterTxt.Text;
                StaffGrid.ItemsSource = FilterData(globalFilter);
            }
            else
            {
                globalFilter = "";
                StaffGrid.ItemsSource = LoadData();
            }

        }
        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                StaffGrid.ItemsSource = LoadData();
            else
            {
                StaffGrid.ItemsSource = FilterData(globalFilter);
            }
        }
    }
}
