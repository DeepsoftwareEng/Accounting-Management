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
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Page
    {
        public AdminMenu()
        {
            InitializeComponent();
        }

        private void AccountManage(object sender, MouseButtonEventArgs e)
        {
            FunctionUI.NavigationService.Navigate(new AccountManage());
        }

        private void AccountantManage(object sender, MouseButtonEventArgs e)
        {
            FunctionUI.NavigationService.Navigate(new AccountantManage());
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
