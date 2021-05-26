using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Models;
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
    public partial class DeleteAgArayuzuPopupWindow : Window
    {
        public int selectedTipId = 0;
        public DeleteAgArayuzuPopupWindow(int selectedTipId)
        {
            this.selectedTipId = selectedTipId;
            InitializeComponent();
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

            ClosePopup();
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

    }
}
