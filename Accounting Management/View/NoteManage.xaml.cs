using Accounting_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
    /// Interaction logic for NoteManage.xaml
    /// </summary>
    public partial class NoteManage : Page
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        int NumberOfPages;
        int CurrentPage = 1;
        string globalFilter = "";
        string NoteId;
        bool isPhieuNhap = false;
        DateTime NgayLap;
        public NoteManage()
        {
            InitializeComponent();
            CurrentPageTxb.Text = CurrentPage.ToString();
            NoteGrid.AutoGenerateColumns = false;
            NoteGrid.ItemsSource = LoadData();
            LoadCombobox();
        }
        #region data
        private void LoadCombobox()
        {
            var ketoans = dbcontext.Employees.Where(c => c.IdChucVu == 1).AsNoTracking().ToList();
            var ketoantruongs = dbcontext.Employees.Where(c => c.IdChucVu == 2).AsNoTracking().ToList();
            var giamdocs = dbcontext.Employees.Where(c => c.IdChucVu == 3).AsNoTracking().ToList();
            var invoices = dbcontext.Invoices.Where(c => c.IsPayed == 0).AsNoTracking().ToList();
            List<dynamic> validInvoices = new();
            foreach(var i in invoices)
            {
                var checkvalid = dbcontext.PhieuNhaps.Where(c => c.MaHoaDon == i.MaHoaDon).AsNoTracking().FirstOrDefault();
                var checkvalid2 = dbcontext.PhieuXuats.Where(c => c.MaHoaDon == i.MaHoaDon).AsNoTracking().FirstOrDefault();
                if(checkvalid == null && checkvalid2 == null)
                {
                    validInvoices.Add(i);
                }
            }

            PhieuNhap.CreateByCbb.SelectedValuePath = "MaNhanVien";
            PhieuNhap.GiamDocCbb.SelectedValuePath = "MaNhanVien";
            PhieuNhap.KeToanTruongCbb.SelectedValuePath = "MaNhanVien";
            PhieuNhap.InvoiceCbb.SelectedValuePath = "MaHoaDon";
            PhieuNhap.CreateByCbb.DisplayMemberPath = "TenNhanVien";
            PhieuNhap.GiamDocCbb.DisplayMemberPath = "TenNhanVien";
            PhieuNhap.KeToanTruongCbb.DisplayMemberPath = "TenNhanVien";
            PhieuNhap.InvoiceCbb.DisplayMemberPath = "MaHoaDon";

            PhieuXuat.NguoiLapCbb.SelectedValuePath = "MaNhanVien";
            PhieuXuat.GiamDocCbb.SelectedValuePath = "MaNhanVien";
            PhieuXuat.KeToanTruongCbb.SelectedValuePath = "MaNhanVien";
            PhieuXuat.InvoiceCbb.SelectedValuePath = "MaHoaDon";
            PhieuXuat.NguoiLapCbb.DisplayMemberPath = "TenNhanVien";
            PhieuXuat.GiamDocCbb.DisplayMemberPath = "TenNhanVien";
            PhieuXuat.KeToanTruongCbb.DisplayMemberPath = "TenNhanVien";
            PhieuXuat.InvoiceCbb.DisplayMemberPath = "MaHoaDon";

            PhieuNhap.CreateByCbb.ItemsSource = ketoans;
            PhieuNhap.GiamDocCbb.ItemsSource = giamdocs;
            PhieuNhap.KeToanTruongCbb.ItemsSource = ketoantruongs;
            PhieuNhap.InvoiceCbb.ItemsSource = validInvoices;

            PhieuXuat.NguoiLapCbb.ItemsSource = ketoans;
            PhieuXuat.GiamDocCbb.ItemsSource = giamdocs;
            PhieuXuat.KeToanTruongCbb.ItemsSource = ketoantruongs;
            PhieuXuat.InvoiceCbb.ItemsSource = validInvoices;
        }
        private dynamic LoadData()
        {
            var phieuNhap = dbcontext.PhieuNhaps.AsNoTracking().OrderBy(c => c.NgayLap).ToList();
            var phieuXuat = dbcontext.PhieuXuats.AsNoTracking().OrderBy(c => c.NgayLap).ToList();
            List<dynamic> rs = new List<dynamic>();
            NumberOfPages = (phieuNhap.Count + phieuXuat.Count) / 20;
            if ((phieuNhap.Count + phieuXuat.Count)% 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            phieuNhap = phieuNhap.Take(20).ToList();
            phieuXuat = phieuXuat.Take(20).ToList();
            int stt = (CurrentPage - 1) * 20 + 1; ;
            foreach(var i in phieuNhap)
            {
                var note = new
                {
                    SoPhieu = i.MaPhieu,
                    NgayLap = i.NgayLap.ToString(),
                    NguoiLap = i.NguoiLap,
                    SoHoaDon = i.MaHoaDon,
                    Type = "Phiếu nhập"
                };
                rs.Add(note);
            }
            foreach (var i in phieuXuat)
            {
                var note = new
                {
                    SoPhieu = i.MaPhieu,
                    NgayLap = i.NgayLap.ToString(),
                    NguoiLap = i.NguoiLap,
                    SoHoaDon = i.MaHoaDon,
                    Type = "Phiếu xuất"
                };
                rs.Add(note);
            }
            rs = rs.OrderBy(x => x.NgayLap).Take(20).ToList();   
            return rs;
        }
        private dynamic FilterData(string filter)
        {
            var phieuNhap = dbcontext.PhieuNhaps.Where(c => c.MaPhieu.Contains(filter) || c.MaHoaDon.Contains(filter)).AsNoTracking().OrderBy(c => c.NgayLap).ToList();
            var phieuXuat = dbcontext.PhieuXuats.Where(c => c.MaPhieu.Contains(filter) || c.MaHoaDon.Contains(filter)).AsNoTracking().OrderBy(c => c.NgayLap).ToList();
            List<dynamic> rs = new List<dynamic>();
            if ((phieuNhap.Count + phieuXuat.Count) == 0)
                return rs;
            NumberOfPages = (phieuNhap.Count + phieuXuat.Count) / 20;
            if ((phieuNhap.Count + phieuXuat.Count) % 20 != 0)
                NumberOfPages++;
            TotalPageTxb.Text = NumberOfPages.ToString();
            TotalPageTxb.Text = NumberOfPages.ToString();
            phieuNhap = phieuNhap.Take(20).ToList();
            phieuXuat = phieuXuat.Take(20).ToList();
            int stt = (CurrentPage - 1) * 20 + 1; ;
            foreach (var i in phieuNhap)
            {
                var note = new
                {
                    STT = 0,
                    SoPhieu = i.MaPhieu,
                    NgayLap = i.NgayLap.ToString(),
                    NguoiLap = i.NguoiLap,
                    SoHoaDon = i.MaHoaDon,
                    Type = "Phiếu nhập"
                };
                rs.Add(note);
            }
            foreach (var i in phieuXuat)
            {
                var note = new
                {
                    STT = 0,
                    SoPhieu = i.MaPhieu,
                    NgayLap = i.NgayLap.ToString(),
                    NguoiLap = i.NguoiLap,
                    SoHoaDon = i.MaHoaDon,
                    Type = "Phiếu xuất"
                };
                rs.Add(note);
            }
            rs = rs.OrderBy(x => x.NgayLap).Take(20).ToList();
            foreach (var i in rs)
            {
                i.STT = stt;
                stt++;
            }
            return rs;
        }
        #endregion
        #region Add
        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            PhieuNhap.CompanyTxb.Text = "";
            PhieuNhap.ContentTxb.Text = "";
            PhieuNhap.DiaChiTxb.Text = "";
            PhieuNhap.NguoiGiaoTxb.Text = "";
            PhieuNhap.NguoiNhanTxb.Text = "";
            PhieuNhap.NoiNhapTxb.Text = "";
            PhieuNhap.ThuKhoTxb.Text = "";
            PhieuNhap.GiamDocCbb.SelectedValue = 1;
            PhieuNhap.InvoiceCbb.SelectedValue = 1;
            PhieuNhap.CreateByCbb.SelectedValue = 1;
            PhieuNhap.IsEnabled = true;
            PhieuNhap.Opacity = 1;
            PhieuNhap.Visibility = Visibility.Visible;
            PhieuNhap.Back.Click += Back_Click;
            PhieuXuat.Back.Click += Back_Click;
            PhieuNhap.ChangeToPhieuXuatBtn.Click += ChangeToPhieuXuatBtn_Click;
            PhieuXuat.ChangeToPhieuNhapBtn.Click += ChangeToPhieuNhapBtn_Click;
            
            PhieuNhap.SaveBtn.Click += SaveAddNote;
            PhieuXuat.SaveBtn.Click += SaveAddNote;
            isPhieuNhap = true;
            PhieuNhap.PayBtn.IsEnabled = false;
            PhieuXuat.PayBtn.IsEnabled = false;
            PhieuNhap.InvoiceCbb.SelectionChanged += ChooseInvoice;
            PhieuXuat.InvoiceCbb.SelectionChanged += ChooseInvoice;
        }

        private void ChooseInvoice(object sender, SelectionChangedEventArgs e)
        {
            float TongTien = 0;
            List<dynamic> product = new List<dynamic>();
            var productInvoice = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == (sender as ComboBox).SelectedValue).AsNoTracking().ToList();
            foreach(var i in productInvoice)
            {
                Product prod = dbcontext.Products.Where(c => c.MaHangHoa == i.MaHangHoa).AsNoTracking().FirstOrDefault();
                float thanhtien = (float)(i.SoLuong * prod.DonGia);
                var temp = new
                {
                    MaHangHoa = prod.MaHangHoa,
                    SoLuong = i.SoLuong,
                    TenHangHoa = prod.TenHangHoa,
                    DonGia = prod.DonGia,
                    ThanhTien = thanhtien
                };
                TongTien += thanhtien;
                product.Add(temp);
            }
            if (isPhieuNhap)
            {
                PhieuNhap.NhapProductGrid.AutoGenerateColumns = false;
                PhieuNhap.NhapProductGrid.ItemsSource = product;
                PhieuNhap.TongTxb.Text = "Tổng: " + TongTien;
            }
            else
            {
                PhieuXuat.XuatProductGrid.AutoGenerateColumns = false ;
                PhieuXuat.XuatProductGrid.ItemsSource = product;
                PhieuXuat.TongTxb.Text = "Tổng: " + TongTien;
            }
        }

        private void ChangeToPhieuNhapBtn_Click(object sender, RoutedEventArgs e)
        {
            PhieuNhap.CompanyTxb.Text = "";
            PhieuNhap.ContentTxb.Text = "";
            PhieuNhap.DiaChiTxb.Text = "";
            PhieuNhap.NguoiGiaoTxb.Text = "";
            PhieuNhap.NguoiNhanTxb.Text = "";
            PhieuNhap.NoiNhapTxb.Text = "";
            PhieuNhap.ThuKhoTxb.Text = "";
            PhieuNhap.GiamDocCbb.SelectedValue = 1;
            PhieuNhap.InvoiceCbb.SelectedValue = 1;
            PhieuNhap.CreateByCbb.SelectedValue = 1;
            PhieuXuat.IsEnabled = false;
            PhieuXuat.Opacity = 0;
            PhieuXuat.Visibility = Visibility.Hidden;
            PhieuNhap.IsEnabled = true;
            PhieuNhap.Opacity = 1;
            PhieuNhap.Visibility = Visibility.Visible;
            isPhieuNhap = true;
        }

        private void ChangeToPhieuXuatBtn_Click(object sender, RoutedEventArgs e)
        {
            PhieuNhap.IsEnabled = false;
            PhieuNhap.Opacity = 0;
            PhieuNhap.Visibility = Visibility.Hidden;
            PhieuXuat.IsEnabled = true;
            PhieuXuat.Opacity = 1;
            PhieuXuat.Visibility = Visibility.Visible;
            isPhieuNhap = false;
        }
        private void SaveAddNote(object sender, RoutedEventArgs e)
        {
            if (isPhieuNhap)
            {
                PhieuNhap note = new PhieuNhap();
                note.NgayLap = DateTime.Now;
                var today = DateOnly.FromDateTime((DateTime)note.NgayLap);
                DateTime start = new DateTime(today.Year, today.Month, today.Day);
                var finish = start.AddDays(1);
                int count = dbcontext.Customers.Where(c => c.CreateDate >= start && c.CreateDate <= finish).ToList().Count;
                note.MaPhieu = "NOTEIN" + today.Year.ToString() + today.Month.ToString() + today.Day.ToString();
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
                    note.MaPhieu += countString;
                }
                note.DonVi = PhieuNhap.CompanyTxb.Text;
                note.DiaChi = PhieuNhap.DiaChiTxb.Text;
                note.NoiNhap = PhieuNhap.NoiNhapTxb.Text;
                note.NoiDung = PhieuNhap.ContentTxb.Text;
                note.ThuKho = PhieuNhap.ThuKhoTxb.Text;
                note.GiamDoc = PhieuNhap.GiamDocCbb.SelectedValue.ToString();
                note.KeToanTruong = PhieuNhap.KeToanTruongCbb.SelectedValue.ToString();
                note.NguoiLap = PhieuNhap.CreateByCbb.SelectedValue.ToString();
                note.MaHoaDon = PhieuNhap.InvoiceCbb.SelectedValue.ToString();
                note.NguoiGiao = PhieuNhap.NguoiGiaoTxb.Text;
                note.NguoiNhan = PhieuNhap.NguoiNhanTxb.Text;
                note.IsThanhToan = 0;
                dbcontext.Entry(note).State = EntityState.Detached;
                try
                {
                    dbcontext.Set<PhieuNhap>().Add(note);
                    dbcontext.SaveChanges();
                    MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    NoteGrid.ItemsSource = LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                PhieuXuat note = new PhieuXuat();
                note.NgayLap = DateTime.Now;
                var today = DateOnly.FromDateTime((DateTime)note.NgayLap);
                DateTime start = new DateTime(today.Year, today.Month, today.Day);
                var finish = start.AddDays(1);
                int count = dbcontext.Customers.Where(c => c.CreateDate >= start && c.CreateDate <= finish).ToList().Count;
                note.MaPhieu = "NOTEOUT" + today.Year.ToString() + today.Month.ToString() + today.Day.ToString();
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
                    note.MaPhieu += countString;
                }
                note.DonVi = PhieuXuat.CompanyTxb.Text;
                note.DiaChi = PhieuXuat.DiaChiTxb.Text;
                note.LiDo = PhieuXuat.ContentTxb.Text;
                note.ThuKho = PhieuXuat.ThuKhoTxb.Text;
                note.GiamDoc = PhieuXuat.GiamDocCbb.SelectedValue.ToString();
                note.KeToanTruong = PhieuXuat.KeToanTruongCbb.SelectedValue.ToString();
                note.NguoiLap = PhieuXuat.NguoiLapCbb.SelectedValue.ToString();
                dbcontext.Entry(note).State = EntityState.Detached;
                note.NhanVienGiao = PhieuXuat.NguoiGiaoTxb.Text;
                note.MaHoaDon = PhieuXuat.InvoiceCbb.SelectedValue.ToString();
                note.NguoiNhan = PhieuXuat.NguoiNhanTxb.Text;
                note.IsThanhToan = 0;
                try
                {
                    dbcontext.Set<PhieuXuat>().Add(note);
                    dbcontext.SaveChanges();
                    MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    NoteGrid.ItemsSource = LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            PhieuXuat.Reset();
            PhieuNhap.Reset();
            PhieuNhap.IsEnabled = false;
            PhieuNhap.Opacity = 0;
            PhieuNhap.Visibility = Visibility.Hidden;
            PhieuXuat.IsEnabled = false;
            PhieuXuat.Opacity = 0;
            PhieuXuat.Visibility = Visibility.Hidden;
        }
        #endregion
        private void PayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isPhieuNhap)
            {
                ThanhToanUI thanhtoan = new ThanhToanUI(NoteId, "IN");
                thanhtoan.Show();
            }
            else
            {
                ThanhToanUI thanhtoan = new ThanhToanUI(NoteId, "OUT");
                thanhtoan.Show();
            }
        }
        #region Edit
        private void EditNote(object sender, RoutedEventArgs e)
        {
            string maHoaDon = "";
            var selected = NoteGrid.SelectedItem;
            dynamic choosenNote = selected;
            string Id = choosenNote.SoPhieu;
            PhieuNhap.PayBtn.IsEnabled = true;
            PhieuXuat.PayBtn.IsEnabled = true;
            PhieuNhap.PayBtn.Click += PayBtn_Click;
            PhieuXuat.PayBtn.Click += PayBtn_Click;
            if (choosenNote.Type == "Phiếu nhập")
            {
                var note = dbcontext.PhieuNhaps.Where(c => c.MaPhieu == Id).AsNoTracking().FirstOrDefault();
                if(note.IsThanhToan == 1)
                {
                    return;
                }
                NoteId = note.MaPhieu;
                NgayLap = (DateTime)note.NgayLap;
                PhieuNhap.CompanyTxb.Text = note.DonVi;
                PhieuNhap.ContentTxb.Text = note.NoiDung;
                PhieuNhap.DiaChiTxb.Text = note.DiaChi;
                PhieuNhap.NguoiGiaoTxb.Text = note.NguoiGiao;
                PhieuNhap.NguoiNhanTxb.Text = note.NguoiNhan;
                PhieuNhap.NoiNhapTxb.Text = note.NoiNhap;
                PhieuNhap.ThuKhoTxb.Text = note.ThuKho;
                PhieuNhap.GiamDocCbb.SelectedValue = note.GiamDoc;
                PhieuNhap.InvoiceCbb.SelectedValue = note.MaHoaDon;
                PhieuNhap.CreateByCbb.SelectedValue = note.NguoiLap;
                PhieuNhap.KeToanTruongCbb.SelectedValue = note.KeToanTruong;
                maHoaDon= note.MaHoaDon;
                note = null;
                Id = null;
                PhieuNhap.IsEnabled = true;
                PhieuNhap.Opacity = 1;
                PhieuNhap.Visibility = Visibility.Visible;
                PhieuNhap.Back.Click += Back_Click;
                PhieuNhap.SaveBtn.Click += SaveEditNote;
                isPhieuNhap = true;
                PhieuNhap.ChangeToPhieuXuatBtn.IsEnabled = false;
                
            }
            else
            {
                var note = dbcontext.PhieuXuats.Where(c => c.MaPhieu == Id).AsNoTracking().FirstOrDefault();
                if (note.IsThanhToan == 1)
                {
                    return;
                }
                NoteId = note.MaPhieu;
                NgayLap = (DateTime)note.NgayLap;
                PhieuXuat.CompanyTxb.Text = note.DonVi;
                PhieuXuat.ContentTxb.Text = note.LiDo;
                PhieuXuat.DiaChiTxb.Text = note.DiaChi;
                PhieuXuat.NguoiGiaoTxb.Text = note.NhanVienGiao;
                PhieuXuat.NguoiNhanTxb.Text = note.NguoiNhan;
                PhieuXuat.ThuKhoTxb.Text = note.ThuKho;
                PhieuXuat.NguoiNhanTxb.Text = note.NguoiNhan;
                PhieuXuat.GiamDocCbb.SelectedValue = note.GiamDoc;
                PhieuXuat.InvoiceCbb.SelectedValue = note.MaHoaDon;
                PhieuXuat.NguoiLapCbb.SelectedValue = note.NguoiLap;
                PhieuNhap.KeToanTruongCbb.SelectedValue = note.KeToanTruong;
                maHoaDon = note.MaHoaDon;
                note = null;
                Id = null;
                PhieuXuat.IsEnabled = true;
                PhieuXuat.Opacity = 1;
                PhieuXuat.Visibility = Visibility.Visible;
                PhieuXuat.Back.Click += Back_Click;
                PhieuXuat.SaveBtn.Click += SaveEditNote;
                isPhieuNhap = false;
                PhieuXuat.ChangeToPhieuNhapBtn.IsEnabled = false;
            }
            float TongTien = 0;
            List<dynamic> product = new List<dynamic>();
            var productInvoice = dbcontext.ProductInvoices.Where(c => c.MaHoaDon == maHoaDon).AsNoTracking().ToList();
            foreach (var i in productInvoice)
            {
                Product prod = dbcontext.Products.Where(c => c.MaHangHoa == i.MaHangHoa).AsNoTracking().FirstOrDefault();
                float thanhtien = (float)(i.SoLuong * prod.DonGia);
                var temp = new
                {
                    MaHangHoa = prod.MaHangHoa,
                    SoLuong = i.SoLuong,
                    TenHangHoa = prod.TenHangHoa,
                    DonGia = prod.DonGia,
                    ThanhTien = thanhtien
                };
                TongTien += thanhtien;
                product.Add(temp);
            }
            if (choosenNote.Type == "Phiếu nhập")
            {
                PhieuNhap.NhapProductGrid.AutoGenerateColumns = false;
                PhieuNhap.NhapProductGrid.ItemsSource = product;
                PhieuNhap.TongTxb.Text = "Tổng: " + TongTien;
            }
            else
            {
                PhieuXuat.XuatProductGrid.AutoGenerateColumns = false;
                PhieuXuat.XuatProductGrid.ItemsSource = product;
                PhieuXuat.TongTxb.Text = "Tổng: " + TongTien;
            }
            choosenNote = null;
        }
        private void SaveEditNote(object sender, RoutedEventArgs e)
        {
            if (isPhieuNhap)
            {
                PhieuNhap note = new PhieuNhap();
                note.MaPhieu = NoteId;
                note.DonVi = PhieuNhap.CompanyTxb.Text;
                note.DiaChi = PhieuNhap.DiaChiTxb.Text;
                note.NoiNhap = PhieuNhap.NoiNhapTxb.Text;
                note.NoiDung = PhieuNhap.ContentTxb.Text;
                note.NgayLap = NgayLap;
                note.ThuKho = PhieuNhap.ThuKhoTxb.Text;
                note.GiamDoc = PhieuNhap.GiamDocCbb.SelectedValue.ToString();
                note.KeToanTruong = PhieuNhap.KeToanTruongCbb.SelectedValue.ToString();
                note.NguoiLap = PhieuNhap.CreateByCbb.SelectedValue.ToString();
                dbcontext.Entry(note).State = EntityState.Detached;
                try
                {
                    dbcontext.Set<PhieuNhap>().Update(note);
                    dbcontext.SaveChanges();
                    MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    NoteGrid.ItemsSource = LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                PhieuXuat note = new PhieuXuat();
                note.MaPhieu = NoteId;
                note.DonVi = PhieuXuat.CompanyTxb.Text;
                note.DiaChi = PhieuXuat.DiaChiTxb.Text;
                note.LiDo = PhieuXuat.ContentTxb.Text;
                note.NgayLap = NgayLap;
                note.ThuKho = PhieuXuat.ThuKhoTxb.Text;
                note.GiamDoc = PhieuXuat.GiamDocCbb.SelectedValue.ToString();
                note.KeToanTruong = PhieuXuat.KeToanTruongCbb.SelectedValue.ToString();
                note.NguoiLap = PhieuXuat.NguoiLapCbb.SelectedValue.ToString();
                dbcontext.Entry(note).State = EntityState.Detached;
                note.NhanVienGiao = PhieuXuat.NguoiGiaoTxb.Text;
                try
                {
                    dbcontext.Set<PhieuXuat>().Update(note);
                    dbcontext.SaveChanges();
                    MessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    NoteGrid.ItemsSource = LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            PhieuNhap.Reset();
            PhieuXuat.Reset();
            PhieuNhap.IsEnabled = false;
            PhieuNhap.Opacity = 0;
            PhieuNhap.Visibility = Visibility.Hidden;
            PhieuXuat.IsEnabled = false;
            PhieuXuat.Opacity = 0;
            PhieuXuat.Visibility = Visibility.Hidden;
        }
        #endregion

        private void ViewNote(object sender, RoutedEventArgs e)
        {
            var selected = NoteGrid.SelectedItem;
            dynamic choosenNote = selected;
            string Id = choosenNote.SoPhieu;
            if (choosenNote.Type == "Phiếu nhập")
            {
                ViewNote viewNote = new ViewNote(Id);
                viewNote.Show();
            }
            else
            {
                ViewOutNote viewNote = new ViewOutNote(Id);
                viewNote.Show();
            }
        }

        #region Paginator
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PhieuNhap.Reset();
            PhieuXuat.Reset();
            PhieuNhap.IsEnabled = false;
            PhieuNhap.Opacity = 0;
            PhieuNhap.Visibility = Visibility.Hidden;
            PhieuXuat.IsEnabled = false;
            PhieuXuat.Opacity = 0;
            PhieuXuat.Visibility = Visibility.Hidden;
        }
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != 1)
                CurrentPage--;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                NoteGrid.ItemsSource = LoadData();
            else
            {
                NoteGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = NumberOfPages;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                NoteGrid.ItemsSource = LoadData();
            else
            {
                NoteGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < NumberOfPages)
                CurrentPage++;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                NoteGrid.ItemsSource = LoadData();
            else
            {
                NoteGrid.ItemsSource = FilterData(globalFilter);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            if (!string.IsNullOrEmpty(FilterTxt.Text.Trim()))
            {
                globalFilter = FilterTxt.Text;
                NoteGrid.ItemsSource = FilterData(globalFilter);
            }
            else
            {
                globalFilter = "";
                NoteGrid.ItemsSource = LoadData();
            }

        }
        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            CurrentPageTxb.Text = CurrentPage.ToString();
            if (string.IsNullOrEmpty(globalFilter))
                NoteGrid.ItemsSource = LoadData();
            else
            {
                NoteGrid.ItemsSource = FilterData(globalFilter);
            }
        }
        #endregion
    }
}
