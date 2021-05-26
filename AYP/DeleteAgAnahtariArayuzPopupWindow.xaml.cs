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
    /// Interaction logic for DeleteAgAnahtariArayuzPopupWindow.xaml
    /// </summary>
    public partial class DeleteAgAnahtariArayuzPopupWindow : Window
    {
        bool agArayuzuMu;
        bool girdiMi;
        bool ciktiMi;
        bool gucArayuzuMu;
        public DeleteAgAnahtariArayuzPopupWindow(bool agArayuzuMu, bool girdiMi, bool ciktiMi, bool gucArayuzuMu)
        {
            this.agArayuzuMu = agArayuzuMu;
            this.girdiMi = girdiMi;
            this.ciktiMi = ciktiMi;
            this.gucArayuzuMu = gucArayuzuMu;

            InitializeComponent();

            if(agArayuzuMu && gucArayuzuMu)
            {
                if (girdiMi && ciktiMi)
                {
                    Mesaj.Content = "Daha önce tanımladığınız arayüzler silinecektir, emin misiniz?";
                }
                else if (girdiMi)
                {
                    Mesaj.Content = "Daha önce tanımladığınız girdi ağ ve güç arayüzleri silinecektir, emin misiniz?";
                }
                else if (ciktiMi)
                {
                    Mesaj.Content = "Daha önce tanımladığınız çıktı ağ ve güç arayüzleri silinecektir, emin misiniz?";
                }
            }
            else if(agArayuzuMu)
            {
                if (girdiMi && ciktiMi)
                {
                    Mesaj.Content = "Daha önce tanımladığınız ağ arayüzleri silinecektir, emin misiniz?";
                }
                else if (girdiMi)
                {
                    Mesaj.Content = "Daha önce tanımladığınız girdi ağ arayüzleri silinecektir, emin misiniz?";
                }
                else if (ciktiMi)
                {
                    Mesaj.Content = "Daha önce tanımladığınız çıktı ağ arayüzleri silinecektir, emin misiniz?";
                }
            }
            else if(gucArayuzuMu)
            {
                Mesaj.Content = "Daha önce tanımladığınız güç arayüzleri silinecektir, emin misiniz?";
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var owner = Owner as AgAnahtariPopupWindow;

            if(gucArayuzuMu)
            {
                owner.gucArayuzuList.Clear();
                owner.AgAnahtariGucArayuzDataGrid.ItemsSource = null;
                owner.AgAnahtariGucArayuzDataGrid.ItemsSource = owner.gucArayuzuList;
                owner.AgAnahtariGucArayuzDataGrid.Visibility = Visibility.Hidden;
                owner.GucArayuzuNoDataRow.Visibility = Visibility.Visible;
            }

            if (agArayuzuMu)
            {
                if (girdiMi)
                {
                    owner.agArayuzuList = owner.agArayuzuList.Where(x => x.KullanimAmaciId != (int)KullanimAmaciEnum.Girdi).ToList();
                }

                if (ciktiMi)
                {
                    owner.agArayuzuList = owner.agArayuzuList.Where(x => x.KullanimAmaciId != (int)KullanimAmaciEnum.Cikti).ToList();
                }

                owner.AgAnahtariAgArayuzDataGrid.ItemsSource = null;
                owner.AgAnahtariAgArayuzDataGrid.ItemsSource = owner.agArayuzuList;

                if (owner.agArayuzuList != null && owner.agArayuzuList.Count() > 0)
                {
                    owner.AgAnahtariAgArayuzDataGrid.Visibility = Visibility.Visible;
                    owner.AgArayuzuNoDataRow.Visibility = Visibility.Hidden;
                }
                else
                {
                    owner.AgAnahtariAgArayuzDataGrid.Visibility = Visibility.Hidden;
                    owner.AgArayuzuNoDataRow.Visibility = Visibility.Visible;
                }
            }
            
            owner.ShowAgArayuzuTab();
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
