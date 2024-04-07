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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Accounting_Management.View
{
    /// <summary>
    /// Interaction logic for StaffMenu.xaml
    /// </summary>
    public partial class StaffMenu : Page
    {
        public StaffMenu()
        {
            InitializeComponent();
        }

        private void DasboardClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void CustomerClick(object sender, MouseButtonEventArgs e)
        {
            FunctionUI.NavigationService.Navigate(new CustomerManage());
        }
        private void ProductClick(object sender, MouseButtonEventArgs e)
        {
            FunctionUI.NavigationService.Navigate(new ProductManage());
        }
        private void NoteClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void InvoiceClick(object sender, MouseButtonEventArgs e)
        {
            FunctionUI.NavigationService.Navigate(new InvoiceManage());
        }
        private void StaffClick(object sender, MouseButtonEventArgs e)
        {
            FunctionUI.NavigationService.Navigate(new StaffManange());
        }
        private void AccountSystemClick(object sender, MouseButtonEventArgs e)
        {

        }
         private void LogClick(object sender, MouseButtonEventArgs e)
        {
            FunctionUI.NavigationService.Navigate(new LogView());
        }
    }
}
