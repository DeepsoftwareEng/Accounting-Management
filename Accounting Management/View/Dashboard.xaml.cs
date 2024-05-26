using Accounting_Management.Models;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace Accounting_Management.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        AccountingManagementContext dbcontext = new AccountingManagementContext();
        string[] Labels = new string[]
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10","11","12"
            };
        SeriesCollection ChiTieuCollection = new SeriesCollection();
        SeriesCollection LoiNhuanCollection = new SeriesCollection();
        public Func<double, string> Values { get; set; }
        public Dashboard()
        {
            InitializeComponent();
            Values = value => value.ToString("N");
            DataContext = this;
            LoadData();
            LoadChart();
        }
        private void LoadData()
        {
            float loiNhuan = 0;
            float chiTieu = 0;
            var firstDay = new DateTime(DateTime.Now.Year, 1, 1);
            var lastDay = new DateTime(DateTime.Now.Year, 12, 31);
            var LoiNhuanAccount = dbcontext.BankLogs.Where(c => c.CreateDate >= firstDay && c.CreateDate <= lastDay).ToList();
            foreach(var i in LoiNhuanAccount)
            {
                var checkInput = dbcontext.BankAccounts.Where(c => c.MaTaiKhoan == i.MaTaiKhoan).FirstOrDefault();
                if (checkInput.LoaiTaiKhoan == "IN")
                    loiNhuan += (float)i.SoTien;
                else
                    chiTieu += (float)i.SoTien;
            }
            var CustomerCount = dbcontext.Customers.Count();
            var averageLoiNhuan = loiNhuan / (DateTime.Now.Month);
            var averageChiTieu = chiTieu / (DateTime.Now.Month);
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime finish;
            switch (DateTime.Now.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 31);
                    break;
                case 2:
                    if(DateTime.Now.Year % 4 == 0)
                    {
                        finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 29);
                    }else
                    finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
                    break;
                default:
                    finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);
                    break;
            }
            DateTime previousStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month -1, 1);
            DateTime previousFinish;
            switch (DateTime.Now.Month -1)
            {
                case 0:
                    previousStart = new DateTime(DateTime.Now.Year - 1, 12, 1);
                    previousFinish = new DateTime(DateTime.Now.Year - 1, 12, 31);
                    break;
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    previousFinish = new DateTime(DateTime.Now.Year, DateTime.Now.Month -1, 31);
                    break;
                case 2:
                    if (DateTime.Now.Year % 4 == 0)
                    {
                        previousFinish = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 29);
                    }else
                    previousFinish = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 28);
                    break;
                default:
                    previousFinish = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 30);
                    break;
            }
            float currentLoiNhuan = 0;
            float currentChiTieu = 0;
            float previousLoiNhuan = 0;
            float previousChiTieu = 0;
            var currentMonthAccount = dbcontext.BankLogs.Where(c => c.CreateDate >= start && c.CreateDate <= finish).ToList();
            foreach (var i in currentMonthAccount)
            {
                var checkInput = dbcontext.BankAccounts.Where(c => c.MaTaiKhoan == i.MaTaiKhoan).FirstOrDefault();
                if (checkInput.LoaiTaiKhoan == "IN")
                    currentLoiNhuan += (float)i.SoTien;
                else
                    currentChiTieu += (float)i.SoTien;
            }
            var previousMonthAccount = dbcontext.BankLogs.Where(c => c.CreateDate >= previousStart && c.CreateDate <= previousFinish).ToList();
            foreach (var i in previousMonthAccount)
            {
                var checkInput = dbcontext.BankAccounts.Where(c => c.MaTaiKhoan == i.MaTaiKhoan).FirstOrDefault();
                if (checkInput.LoaiTaiKhoan == "IN")
                    previousLoiNhuan += (float)i.SoTien;
                else
                    previousChiTieu += (float)i.SoTien;
            }
            int countRemain = 0;
            var remainInvoice = dbcontext.Invoices.Where(c => c.IsPayed == 0).ToList();
            foreach(var i in remainInvoice)
            {
                var checkValidNhap = dbcontext.PhieuNhaps.Where(c => c.MaHoaDon == i.MaHoaDon).FirstOrDefault();
                var checkValidXuat = dbcontext.PhieuXuats.Where(c => c.MaHoaDon == i.MaHoaDon).FirstOrDefault();
                if (checkValidNhap == null && checkValidXuat == null)
                    countRemain++;
            }
            float growth = (currentLoiNhuan / previousLoiNhuan) * 100;
            LoiNhuanTxb.Text = loiNhuan.ToString();
            ChiTieuTxb.Text= chiTieu.ToString();
            AvarageChiTieuTxb.Text = "Trung bình: " + averageChiTieu.ToString();
            AvarageLoiNhuan.Text = "Trung bình: " + averageLoiNhuan.ToString();
            CurrenMonthTxb.Text = "Hiện tại: "+currentLoiNhuan.ToString();
            PreviousMonthxb.Text ="Tháng trước: "+ previousLoiNhuan.ToString();
            CurrentChiTieuTxb.Text = "Hiện tại: "+currentChiTieu.ToString();
            PreviousMonthChiTieuTxb.Text = "Tháng trước: "+previousChiTieu.ToString();
            RemainTxb.Text = "Hóa đơn tồn đọng: "+countRemain.ToString();
            KhachHangTxb.Text = CustomerCount.ToString();
            GrowthTxb.Text = "Tăng trưởng: "+growth.ToString();
        }
        private void LoadChart()
        {
            
            
            for(int i = 1; i<=12; i++)
            {
                var firstMonthDate = new DateTime(DateTime.Now.Year, i, 1);
                DateTime lastMonthDate;
                switch (i)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 31);
                        break;
                    case 2:
                        if (DateTime.Now.Year % 4 == 0)
                        {
                            lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 29);
                        }
                        else
                            lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
                        break;
                    default:
                        lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);
                        break;
                }
                float currentMonthLoiNhuan = 0;
                float currentMonthChiTieu = 0;
                var currentMonthAccount = dbcontext.BankLogs.Where(c => c.CreateDate >= firstMonthDate && c.CreateDate <= lastMonthDate).ToList();
                foreach (var item in currentMonthAccount)
                {
                    var checkInput = dbcontext.BankAccounts.Where(c => c.MaTaiKhoan == item.MaTaiKhoan).FirstOrDefault();
                    if (checkInput.LoaiTaiKhoan == "IN")
                        currentMonthLoiNhuan += (float)item.SoTien;
                    else
                        currentMonthChiTieu += (float)item.SoTien;
                }
                ColumnSeries temp = new ColumnSeries();
                temp.Title = i.ToString();
                temp.ColumnPadding = 0.5;
                temp.Values = new ChartValues<float> { currentMonthLoiNhuan };
                LoiNhuanCollection.Add(temp);
                temp.Values = new ChartValues<float> { currentMonthChiTieu };
                ChiTieuCollection.Add(temp);
            }
        }
    }
}
