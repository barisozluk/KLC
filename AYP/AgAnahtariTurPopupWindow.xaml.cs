using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
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
        private AYPContext context;

        private IAgAnahatariService service;

        AgAnahtariTur agAnahtariTur;
        public AgAnahtariTurPopupWindow()
        {
            this.context = new AYPContext();
            service = new AgAnahatariService(this.context);
            agAnahtariTur = new AgAnahtariTur();

            InitializeComponent();
            DataContext = agAnahtariTur;
        }

        private void Save_AgAnahtariTur(object sender, RoutedEventArgs e)
        {
            var validationContext = new ValidationContext(agAnahtariTur, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAnahtariTur, validationContext, results, true))
            {
                var response = service.SaveAgAnahtariTur(agAnahtariTur);

                if (!response.HasError)
                {
                    Hide();
                    Owner.IsEnabled = true;
                    Owner.Effect = null;
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
            Hide();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

    }
}
