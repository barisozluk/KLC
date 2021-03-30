using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace AYP
{
    /// <summary>
    /// Interaction logic for UcBirimTurPopupWindow.xaml
    /// </summary>
    public partial class NewAppPopupWindow : Window
    {
        public bool returnvalue;
        public NewAppPopupWindow()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            returnvalue = true;
            this.Close();
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            returnvalue = false;
            this.Close();
            //Owner.Effect = null;
            //Owner.IsEnabled = true;
        }

    }
}
