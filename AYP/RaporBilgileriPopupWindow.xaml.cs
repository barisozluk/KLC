using AYP.Models;
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
    /// Interaction logic for RaporBilgileriPopupWindow.xaml
    /// </summary>
    public partial class RaporBilgileriPopupWindow : Window
    {
        RaporModel model;
        public RaporBilgileriPopupWindow()
        {
            InitializeComponent();
            model = new RaporModel();
            DataContext = model;
        }

        #region PopupCloseEvents
        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonRaporBilgileriPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }
        #endregion
    }
}
