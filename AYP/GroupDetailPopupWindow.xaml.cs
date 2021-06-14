using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Enums;
using AYP.Helpers.Extensions;
using AYP.Interfaces;
using AYP.Models;
using AYP.Services;
using AYP.View;
using AYP.ViewModel;
using AYP.ViewModel.Node;
using DynamicData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AYP
{
    /// <summary>
    /// Interaction logic for GroupDetailPopupWindow.xaml
    /// </summary>
    public partial class GroupDetailPopupWindow : Window
    {
        GroupUngroupModel group = null;

        public GroupDetailPopupWindow(GroupUngroupModel group)
        {
            this.group = new GroupUngroupModel();
            this.group.UniqueId = group.UniqueId;
            this.group.Name = group.Name;
            this.group.NodeList = new List<NodeViewModel>();
            foreach (var node in group.NodeList)
            {
                this.group.NodeList.Add(node);
            }

            this.group.InternalConnectList = group.InternalConnectList;
            this.group.ExternalConnectList = group.ExternalConnectList;
            this.group.GucArayuzuList = group.GucArayuzuList;
            this.group.AgArayuzuList = group.AgArayuzuList;
            

            InitializeComponent();
            Loaded += Window_Loaded;
        }

        #region WindowLoadedEvent
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainTitle.Content = this.group.Name + " Detayı";
            foreach (var groupedNode in group.NodeList)
            {
                NodesCanvas.ViewModel.Nodes.Add(groupedNode);

                foreach (var output in groupedNode.Transitions.Items)
                {
                    foreach (var internalConnect in group.InternalConnectList)
                    {
                        if (internalConnect.FromConnector == output)
                        {
                            ConnectViewModel connect = new ConnectViewModel(NodesCanvas.ViewModel, output);
                            connect.ToConnector = internalConnect.ToConnector;
                            connect.FromConnector.Connect = connect;
                            connect.AgYuku = internalConnect.AgYuku;
                            connect.GucMiktari = internalConnect.GucMiktari;
                            connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                            connect.Uzunluk = internalConnect.Uzunluk;
                            //connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                            //connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                            NodesCanvas.ViewModel.Connects.Add(connect);
                        }
                    }
                }

                foreach (var input in groupedNode.InputList)
                {
                    foreach (var internalConnect in group.InternalConnectList)
                    {
                        if (internalConnect.ToConnector == input)
                        {
                            ConnectViewModel connect = new ConnectViewModel(NodesCanvas.ViewModel, internalConnect.FromConnector);
                            connect.ToConnector = input;
                            connect.FromConnector.Connect = connect;
                            connect.AgYuku = internalConnect.AgYuku;
                            connect.GucMiktari = internalConnect.GucMiktari;
                            connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                            connect.Uzunluk = internalConnect.Uzunluk;
                            //connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                            //connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                            NodesCanvas.ViewModel.Connects.Add(connect);
                        }

                    }
                }
            }
        }
        #endregion

        #region PopupCloseEvents
        private void ButtonNodeDetailPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }
        #endregion
    }
}
