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
    public partial class AgAnahtariGucArayuzuPopupWindow : Window
    {
        private AYPContext context;

        private IAgAnahtariService service;

        public AgAnahtariGucArayuzuPopupWindow()
        {
            this.context = new AYPContext();
            service = new AgAnahtariService(this.context);

            InitializeComponent();
        }

        private void ButtonAgAnahtariGucArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void AgAnahtariGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string kullanimAmaci = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (kullanimAmaci == "Çıktı")
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
    }
}
