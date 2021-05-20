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
    public partial class SelectGateWayForTopology : Window
    {
        List<NodeViewModel> selectedNodes;
        NodeViewModel gateWay;
        int topolojiId = 0;

        public SelectGateWayForTopology(List<NodeViewModel> selectedNodes, int topolojiId)
        {
            this.selectedNodes = selectedNodes;
            this.topolojiId = topolojiId;

            InitializeComponent();
            GateWay.ItemsSource = selectedNodes;
            GateWay.SelectedItem = selectedNodes.First();
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

        private void Save_GateWay(object sender, RoutedEventArgs e)
        {
            if (topolojiId == (int)TopolojiEnum.Yildiz)
            {
                (Owner as MainWindow).NodesCanvas.ViewModel.YildizTopolojiOlustur(this.gateWay);
            }
            else if (topolojiId == (int)TopolojiEnum.Halka)
            {
                (Owner as MainWindow).NodesCanvas.ViewModel.HalkaTopolojiOlustur(this.gateWay);
            }
            else
            {
                (Owner as MainWindow).NodesCanvas.ViewModel.ZincirTopolojiOlustur(this.gateWay);
            }

            ClosePopup();
        }

        #region SelectionChangedEvent
        private void Node_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            gateWay = (NodeViewModel)combo.SelectedItem;
        }
        #endregion
    }
}
