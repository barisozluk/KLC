using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using AYP.ViewModel;
using AYP.ViewModel.Node;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace AYP
{
    /// <summary>
    /// Interaction logic for UcBirimTurPopupWindow.xaml
    /// </summary>
    public partial class SelectGucUreticiForOtoConnect : Window
    {
        List<NodeViewModel> selectedNodes;
        NodeViewModel gucUretici;

        public SelectGucUreticiForOtoConnect(List<NodeViewModel> selectedNodes)
        {
            this.selectedNodes = selectedNodes;

            InitializeComponent();
            GucUretici.ItemsSource = selectedNodes;
            GucUretici.SelectedItem = selectedNodes.First();
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void Save_GucUretici(object sender, RoutedEventArgs e)
        {
            (Owner as MainWindow).NodesCanvas.ViewModel.CommandGucUreticiOtoConnectLogic.Execute(this.gucUretici);
            ClosePopup();
        }

        #region SelectionChangedEvent
        private void Node_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            gucUretici = (NodeViewModel)combo.SelectedItem;
        }
        #endregion
    }
}
