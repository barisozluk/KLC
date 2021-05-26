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
using System.Linq;
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
    /// Interaction logic for DeleteGucUreticiGucArayuzuPopupWindow.xaml
    /// </summary>
    public partial class DeleteGucUreticiGucArayuzuPopupWindow : Window
    {
        bool girdiMi;
        bool ciktiMi;
        public DeleteGucUreticiGucArayuzuPopupWindow(bool girdiMi, bool ciktiMi)
        {
            this.girdiMi = girdiMi;
            this.ciktiMi = ciktiMi;

            InitializeComponent();

            if (girdiMi && ciktiMi)
            {
                Mesaj.Content = "Daha önce tanımladığınız güç arayüzleri silinecektir, emin misiniz?";
            }
            else if (girdiMi)
            {
                Mesaj.Content = "Daha önce tanımladığınız girdi güç arayüzleri silinecektir, emin misiniz?";
            }
            else if (ciktiMi)
            {
                Mesaj.Content = "Daha önce tanımladığınız çıktı güç arayüzleri silinecektir, emin misiniz?";
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var owner = Owner as GucUreticiPopupWindow;
            if (girdiMi)
            {
                owner.gucArayuzuList = owner.gucArayuzuList.Where(x => x.KullanimAmaciId != (int)KullanimAmaciEnum.Girdi).ToList();
            }

            if (ciktiMi)
            {
                owner.gucArayuzuList = owner.gucArayuzuList.Where(x => x.KullanimAmaciId != (int)KullanimAmaciEnum.Cikti).ToList();
            }

            owner.GucUreticiGucArayuzDataGrid.ItemsSource = null;
            owner.GucUreticiGucArayuzDataGrid.ItemsSource = owner.gucArayuzuList;

            if (owner.gucArayuzuList != null && owner.gucArayuzuList.Count() > 0)
            {
                owner.GucUreticiGucArayuzDataGrid.Visibility = Visibility.Visible;
                owner.GucArayuzuNoDataRow.Visibility = Visibility.Hidden;
            }
            else
            {
                owner.GucUreticiGucArayuzDataGrid.Visibility = Visibility.Hidden;
                owner.GucArayuzuNoDataRow.Visibility = Visibility.Visible;
            }

            owner.ShowGucUreticiTab();
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
