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
    public partial class MultipleCopyPopupWindow : Window
    {


        public NodeViewModel selectedNode;

        public MultipleCopyPopupWindow(NodeViewModel _selectedNode)
        {
            this.selectedNode = _selectedNode;
            InitializeComponent();
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonMultipleCopyPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();

        }

        private void Save_MultipleCopy(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Adet.Text) && !String.IsNullOrWhiteSpace(Adet.Text))
            {
                for (int i = 1; i <= Convert.ToInt32(Adet.Text); i++)
                {
                    ExternalNode data = new ExternalNode();
                    var nodePoint = new Point(selectedNode.Point1.X + (15 * i), selectedNode.Point1.Y + (15 * i));

                    data.Node = selectedNode;
                    data.Point = nodePoint;
                    selectedNode.NodesCanvas.CommandAddNodeWithUndoRedo.Execute(data);
                }




                this.ClosePopup();
            }
            else
            {
                Adet.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        #region NumberValidation
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
    }
}
