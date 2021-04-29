using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using AYP.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    public partial class EditSelectedNodeNamePopupWindow : Window
    {
        List<NodeViewModel> selectedNodes = null;
        public EditSelectedNodeNamePopupWindow(List<NodeViewModel> selectedNodes)
        {
            this.selectedNodes = selectedNodes;
            InitializeComponent();
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonRenamePopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void Rename(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(Ad.Text))
            {
                foreach(var selectedNode in this.selectedNodes)
                {
                    selectedNode.Name = Ad.Text;

                    if(selectedNode.TypeId == (int)TipEnum.Group)
                    {
                        var group = selectedNode.NodesCanvas.GroupList.Where(x => x.UniqueId == selectedNode.UniqueId).FirstOrDefault();
                        group.Name = selectedNode.Name;
                    }
                }

                ClosePopup();
            }
            else
            {
                Ad.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
