using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariAgArayuzuPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariAgArayuzuPopupWindow : Window
    {
        private AYPContext context;

        private IAgAnahtariService service;

        private IKodListeService kodListeService;

        private NotificationManager notificationManager;
        public MainWindow MainWindow { get; set; }

        AgArayuzu agArayuzu;

        public AgAnahtariAgArayuzuPopupWindow()
        {
            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new AgAnahtariService(this.context);
            kodListeService = new KodListeService(this.context);
            agArayuzu = new AgArayuzu();
            InitializeComponent();
            ListAgAnahtari();
            ListKullanimAmaci();
            ListFizikselOrtam();
            ListKapasite();
            DataContext = agArayuzu;

            if (agArayuzu.AgAnahtariList.Count() == 0)
            {
                Loaded += (s, e) => ClosePopup();
            }

        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonAgAnahtariAgArayuzuPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void Save_AgAnahtariAgArayuzu(object sender, RoutedEventArgs e)
        {
            agArayuzu.TipId = (int)TipEnum.AgAnahtariAgArayuzu;

            var validationContext = new ValidationContext(agArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agArayuzu, validationContext, results, true))
            {
                var response = service.SaveAgAnahtariAgArayuzu(agArayuzu);

                if (!response.HasError)
                {
                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
                else
                {
                    NotifyWarningPopup nfp = new NotifyWarningPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                    
                }
            }
            //else
            //{
            //    foreach (var result in results)
            //    {
            //        foreach (var memberName in result.MemberNames)
            //        {
            //            if (memberName == "UcBirimTurId")
            //            {
            //                UcBirimTur.BorderBrush = new SolidColorBrush(Colors.Red);
            //            }

            //            if (memberName == "StokNo")
            //            {
            //                StokNo.BorderBrush = new SolidColorBrush(Colors.Red);
            //            }

            //            if (memberName == "Tanim")
            //            {
            //                Tanim.BorderBrush = new SolidColorBrush(Colors.Red);
            //            }

            //            if (memberName == "UreticiAdi")
            //            {
            //                Uretici.BorderBrush = new SolidColorBrush(Colors.Red);
            //            }

            //            if (memberName == "UreticiParcaNo")
            //            {
            //                UreticiParcaNo.BorderBrush = new SolidColorBrush(Colors.Red);
            //            }
            //        }
            //    }
            //}
        }

        private void ListKapasite()
        {
            agArayuzu.KapasiteList = kodListeService.ListKapasite();
            if (agArayuzu.KapasiteList.Count() > 0)
            {
                agArayuzu.KapasiteId = agArayuzu.KapasiteList[0].Id;
            }
        }

        private void ListFizikselOrtam()
        {
            agArayuzu.FizikselOrtamList = kodListeService.ListFizikselOrtam();
            if (agArayuzu.FizikselOrtamList.Count() > 0)
            {
                agArayuzu.FizikselOrtamId = agArayuzu.FizikselOrtamList[0].Id;
            }
        }

        private void ListKullanimAmaci()
        {
            agArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
            if (agArayuzu.KullanimAmaciList.Count() > 0)
            {
                agArayuzu.KullanimAmaciId = agArayuzu.KullanimAmaciList[0].Id;
            }
        }
        private void ListAgAnahtari()
        {
            agArayuzu.AgAnahtariList = service.ListAgAnahtari();
            if (agArayuzu.AgAnahtariList.Count() > 0)
            {
                agArayuzu.AgAnahtariId = agArayuzu.AgAnahtariList[0].Id;
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, en az bir ağ anahtarı tanımlayınız.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }
    }
}
