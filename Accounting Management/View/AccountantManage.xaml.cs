﻿using System;
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
    /// Interaction logic for AccountantManage.xaml
    /// </summary>
    public partial class AccountantManage : Page
    {
        public AccountantManage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Drawer.IsEnabled = true;
            Drawer.Opacity = 1;
            Drawer.Visibility = Visibility.Visible;
            Drawer.Back.Click += Back_Click;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Drawer.IsEnabled = false;
            Drawer.Visibility = Visibility.Hidden;
            Drawer.Opacity = 0;
        }
    }
}
