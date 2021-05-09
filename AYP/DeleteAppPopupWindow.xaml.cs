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
    public partial class DeleteAppPopupWindow : Window
    {
        AYPContext context;

        IAgAnahtariService agAnahtariService;
        IUcBirimService ucBirimService;
        IGucUreticiService gucUreticiService;

        public int selectedTipId = 0;
        object selectedItem = null;
        public DeleteAppPopupWindow(object selectedItem, int selectedTipId)
        {
            this.selectedItem = selectedItem;
            this.selectedTipId = selectedTipId;

            agAnahtariService = new AgAnahtariService();
            ucBirimService = new UcBirimService();
            gucUreticiService = new GucUreticiService();

            InitializeComponent();
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var response = new ResponseModel();

            if (selectedTipId == (int)TipEnum.AgAnahtari)
            {
                response = agAnahtariService.DeleteAgAnahtari((AgAnahtari)selectedItem);
                (Owner as MainWindow).ListAgAnahtari();
            }
            if (selectedTipId == (int)TipEnum.UcBirim)
            {
                response = ucBirimService.DeleteUcBirim((UcBirim)selectedItem);
                (Owner as MainWindow).ListUcBirim();

            }
            if (selectedTipId == (int)TipEnum.GucUretici)
            {
                response = gucUreticiService.DeleteGucUretici((GucUretici)selectedItem);
                (Owner as MainWindow).ListGucUretici();
            }

            if (!response.HasError)
            {
                NotifySuccessPopup nfp = new NotifySuccessPopup();
                nfp.msg.Text = response.Message;
                nfp.Owner = Owner;
                nfp.Show();

                ClosePopup();
            }
            else
            {
                NotifyWarningPopup nfp = new NotifyWarningPopup();
                nfp.msg.Text = response.Message;
                nfp.Owner = Owner;
                nfp.Show();
            }
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
