using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
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

            context = new AYPContext();
            agAnahtariService = new AgAnahtariService(context);
            ucBirimService = new UcBirimService(context);
            gucUreticiService = new GucUreticiService(context);

            InitializeComponent();
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTipId == (int)TipEnum.AgAnahtari)
            {
                agAnahtariService.DeleteAgAnahtari((AgAnahtari)selectedItem);
                (Owner as MainWindow).ListAgAnahtari();
            }
            if (selectedTipId == (int)TipEnum.UcBirim)
            {
                ucBirimService.DeleteUcBirim((UcBirim)selectedItem);
                (Owner as MainWindow).ListUcBirim();

            }
            if (selectedTipId == (int)TipEnum.GucUretici)
            {
                gucUreticiService.DeleteGucUretici((GucUretici)selectedItem);
                (Owner as MainWindow).ListGucUretici();
            }

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
