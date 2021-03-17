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
    public partial class UcBirimAgArayuzuWindow : Window
    {
        private AYPContext context;

        private IUcBirimService service;

        public UcBirimAgArayuzuWindow()
        {
            this.context = new AYPContext();
            service = new UcBirimService(this.context);

            InitializeComponent();
        }

        private void ButtonUcBirimAgArayuzuPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }
    }
}
