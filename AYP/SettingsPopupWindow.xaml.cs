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
    public partial class SettingsPopupWindow : Window
    {
        public SettingsPopupWindow()
        {
            InitializeComponent();
            this.Versiyon.Content = "Versiyon : 1.0.13";
        }

        private void VersiyonPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

    }
}
