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
            if (!string.IsNullOrEmpty(Ad.Text))
            {
                bool flag = true;
                foreach (var node in selectedNodes.First().NodesCanvas.Nodes.Items)
                {
                    string temp = "";

                    if (node.Name.Contains("#"))
                    {
                        temp = node.Name.Substring(0, node.Name.IndexOf("#") - 1);
                    }
                    else
                    {
                        temp = node.Name;
                    }

                    if (temp.ToLower() == Ad.Text.ToLower())
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    selectedNodes.First().Name = Ad.Text + " #1";

                    if (selectedNodes.First().TypeId == (int)TipEnum.Group)
                    {
                        var group = selectedNodes.First().NodesCanvas.GroupList.Where(x => x.UniqueId == selectedNodes.First().UniqueId).FirstOrDefault();
                        group.Name = selectedNodes.First().Name;
                    }


                    ClosePopup();
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Aynı isimle tanımlı bir cihaz bulunmaktadır.";
                    nfp.Owner = this;
                    nfp.Show();
                }
            }
            else
            {
                Ad.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void NameValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            foreach (char ch in e.Text)
            {
                if (!Char.IsDigit(ch))
                {
                    if (ch.Equals('#'))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = false;
                }
            }
        }
    }
}
