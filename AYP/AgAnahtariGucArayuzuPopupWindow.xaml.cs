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
    /// Interaction logic for UcBirimAgArayuzuWindow.xaml
    /// </summary>
    public partial class AgAnahtariGucArayuzuPopupWindow : Window
    {
        private AYPContext context;

        private IAgAnahtariService service;

        private IKodListeService kodListeService;

        private NotificationManager notificationManager;
        public MainWindow MainWindow { get; set; }


        GucArayuzu gucArayuzu;
        public AgAnahtariGucArayuzuPopupWindow()
        {
            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new AgAnahtariService(this.context);
            kodListeService = new KodListeService(this.context);
            gucArayuzu = new GucArayuzu();
            InitializeComponent();
            ListAgAnahtari();
            ListKullanimAmaci();
            ListGerilimTipi();
            DataContext = gucArayuzu;

            if (gucArayuzu.AgAnahtariList.Count() == 0)
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

        private void ButtonAgAnahtariGucArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void AgAnahtariGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                ag1.IsEnabled = false; ag1.Opacity = 0.25;
                ag2.IsEnabled = false; ag2.Opacity = 0.25;
                ag3.IsEnabled = false; ag3.Opacity = 0.25;
                ag4.IsEnabled = false; ag4.Opacity = 0.25;
                ag5.IsEnabled = false; ag5.Opacity = 0.25;
                ag6.IsEnabled = false; ag6.Opacity = 0.25;
                ag7.IsEnabled = false; ag7.Opacity = 0.25;
                ag8.IsEnabled = false; ag8.Opacity = 0.25;
                ag9.IsEnabled = false; ag9.Opacity = 0.25;
                ag10.IsEnabled = false; ag10.Opacity = 0.25;
                ag11.IsEnabled = false; ag11.Opacity = 0.25;
                ag12.IsEnabled = false; ag12.Opacity = 0.25;

                ag13.IsEnabled = true; ag13.Opacity = 1;
                ag14.IsEnabled = true; ag14.Opacity = 1;
                ag15.IsEnabled = true; ag15.Opacity = 1;
                ag16.IsEnabled = true; ag16.Opacity = 1;
            }
            else
            {
                ag1.IsEnabled = true; ag1.Opacity = 1;
                ag2.IsEnabled = true; ag2.Opacity = 1;
                ag3.IsEnabled = true; ag3.Opacity = 1;
                ag4.IsEnabled = true; ag4.Opacity = 1;
                ag5.IsEnabled = true; ag5.Opacity = 1;
                ag6.IsEnabled = true; ag6.Opacity = 1;
                ag7.IsEnabled = true; ag7.Opacity = 1;
                ag8.IsEnabled = true; ag8.Opacity = 1;
                ag9.IsEnabled = true; ag9.Opacity = 1;
                ag10.IsEnabled = true; ag10.Opacity = 1;
                ag11.IsEnabled = true; ag11.Opacity = 1;
                ag12.IsEnabled = true; ag12.Opacity = 1;

                ag13.IsEnabled = false; ag13.Opacity = 0.25;
                ag14.IsEnabled = false; ag14.Opacity = 0.25;
                ag15.IsEnabled = false; ag15.Opacity = 0.25;
                ag16.IsEnabled = false; ag16.Opacity = 0.25;
            }
        }

        private void Save_AgAnahtariGucAarayuzu(object sender, RoutedEventArgs e)
        {
            gucArayuzu.TipId = (int)TipEnum.AgAnahtariGucArayuzu;

            var validationContext = new ValidationContext(gucArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
            {
                var response = service.SaveAgAnahtariGucArayuzu(gucArayuzu);

                if (!response.HasError)
                {
                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                    ClosePopup();
                }
                else
                {
                    NotifyWarningPopup nfp = new NotifyWarningPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                    ClosePopup();
                }
            }
        }

        private void ListGerilimTipi()
        {
            gucArayuzu.GerilimTipiList = kodListeService.ListGerilimTipi();
            if (gucArayuzu.GerilimTipiList.Count() > 0)
            {
                gucArayuzu.GerilimTipiId = gucArayuzu.GerilimTipiList[0].Id;
            }
        }

        private void ListKullanimAmaci()
        {
            gucArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
            if (gucArayuzu.KullanimAmaciList.Count() > 0)
            {
                gucArayuzu.KullanimAmaciId = gucArayuzu.KullanimAmaciList[0].Id;
            }
        }

        private void ListAgAnahtari()
        {
            gucArayuzu.AgAnahtariList = service.ListAgAnahtari();
            if (gucArayuzu.AgAnahtariList.Count() > 0)
            {
                gucArayuzu.AgAnahtariId = gucArayuzu.AgAnahtariList[0].Id;
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, en az bir ağ anahtarı tanımlayınız.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            foreach (char ch in e.Text)
            {
                if (!Char.IsDigit(ch))
                {
                    if (ch.Equals('.') || ch.Equals(','))
                    {
                        int seperatorCount = textBox.Text.Where(t => t.Equals('.') || t.Equals(',')).Count();

                        if (seperatorCount < 1)
                        {
                            if (textBox.Text.Length > 0)
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = false;
                }
            }
        }
    }
}
