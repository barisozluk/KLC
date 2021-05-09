using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;


namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariTurPopWindow.xaml
    /// </summary>
    public partial class AgAnahtariTurPopupWindow : Window
    {

        private IAgAnahtariService service;

        AgAnahtariTur agAnahtariTur;

        public MainWindow MainWindow { get; set; }

        public AgAnahtariTurPopupWindow()
        {
            service = new AgAnahtariService();
            agAnahtariTur = new AgAnahtariTur();

            InitializeComponent();
            DataContext = agAnahtariTur;
        }

        private void Save_AgAnahtariTur(object sender, RoutedEventArgs e)
        {
            NotificationManager notificationManager = new NotificationManager();

            var validationContext = new ValidationContext(agAnahtariTur, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAnahtariTur, validationContext, results, true))
            {
                var response = service.SaveAgAnahtariTur(agAnahtariTur);

                if (!response.HasError)
                {
                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = "İşlem başarı ile gerçekleştirildi.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();

                    Close();
                    Owner.IsEnabled = true;
                    Owner.Effect = null;
                }
                else
                {
                    NotifyWarningPopup nfp = new NotifyWarningPopup();
                    nfp.msg.Text = "İşlem başarısız oldu.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            
            }
            else
            {
                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        if (memberName == "Ad")
                        {
                            Ad.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }
        }

        private void ButtonAgAnahtariTurPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

    }
}
