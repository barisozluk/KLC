using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using AYP.ViewModel;
using AYP.ViewModel.Node;
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
    public partial class CableLengthPopupWindow : Window
    {


        public ConnectViewModel connect;

        public CableLengthPopupWindow(ConnectViewModel connect)
        {
            this.connect = connect;
            InitializeComponent();    
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonCableLengthPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
            
        }

        private void Save_CableLengthPopup(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Adet.Text) && !String.IsNullOrWhiteSpace(Adet.Text))
            {
                
            }
            else
            {
                Adet.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        
    }
}
