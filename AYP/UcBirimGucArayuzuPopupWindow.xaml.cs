using AYP.DbContext.AYP.DbContexts;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
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
    public partial class UcBirimGucArayuzuPopupWindow : Window
    {
        private AYPContext context;

        private IUcBirimService service;

        public UcBirimGucArayuzuPopupWindow()
        {
            this.context = new AYPContext();
            service = new UcBirimService(this.context);

            InitializeComponent();
        }

        private void ButtonUcBirimGucArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void UcBirimGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string kullanimAmaci = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (kullanimAmaci == "Çıktı")
            {
                ug1.IsEnabled = false; ug1.Opacity = 0.25;
                ug2.IsEnabled = false; ug2.Opacity = 0.25;
                ug3.IsEnabled = false; ug3.Opacity = 0.25;
                ug4.IsEnabled = false; ug4.Opacity = 0.25;
                ug5.IsEnabled = false; ug5.Opacity = 0.25;
                ug6.IsEnabled = false; ug6.Opacity = 0.25;
                ug7.IsEnabled = false; ug7.Opacity = 0.25;
                ug8.IsEnabled = false; ug8.Opacity = 0.25;
                ug9.IsEnabled = false; ug9.Opacity = 0.25;
                ug10.IsEnabled = false; ug10.Opacity = 0.25;
                ug11.IsEnabled = false; ug11.Opacity = 0.25;
                ug12.IsEnabled = false; ug12.Opacity = 0.25;

                ug13.IsEnabled = true; ug13.Opacity = 1;
                ug14.IsEnabled = true; ug14.Opacity = 1;
                ug15.IsEnabled = true; ug15.Opacity = 1;
                ug16.IsEnabled = true; ug16.Opacity = 1;
            }
            else
            {
                ug1.IsEnabled = true; ug1.Opacity = 1;
                ug2.IsEnabled = true; ug2.Opacity = 1;
                ug3.IsEnabled = true; ug3.Opacity = 1;
                ug4.IsEnabled = true; ug4.Opacity = 1;
                ug5.IsEnabled = true; ug5.Opacity = 1;
                ug6.IsEnabled = true; ug6.Opacity = 1;
                ug7.IsEnabled = true; ug7.Opacity = 1;
                ug8.IsEnabled = true; ug8.Opacity = 1;
                ug9.IsEnabled = true; ug9.Opacity = 1;
                ug10.IsEnabled = true; ug10.Opacity = 1;
                ug11.IsEnabled = true; ug11.Opacity = 1;
                ug12.IsEnabled = true; ug12.Opacity = 1;

                ug13.IsEnabled = false; ug13.Opacity = 0.25;
                ug14.IsEnabled = false; ug14.Opacity = 0.25;
                ug15.IsEnabled = false; ug15.Opacity = 0.25;
                ug16.IsEnabled = false; ug16.Opacity = 0.25;
            }
        }
    }
}
