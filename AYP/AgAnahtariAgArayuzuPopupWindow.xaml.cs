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
    /// Interaction logic for AgAnahtariAgArayuzuPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariAgArayuzuPopupWindow : Window
    {
        private AYPContext context;

        private IAgAnahatariService service;
        public AgAnahtariAgArayuzuPopupWindow()
        {
            this.context = new AYPContext();
            service = new AgAnahatariService(this.context);

            InitializeComponent();
        }

        private void ButtonAgAnahtariAgArayuzuPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }
    }
}
