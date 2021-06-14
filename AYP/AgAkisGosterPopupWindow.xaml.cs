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
    public partial class AgAkisGosterPopupWindow : Window
    {
        NodeViewModel ucBirim;
        List<KeyValuePair<AgAkisTakipModel, List<AgAkisTakipModel>>> agAkisTakipList = new List<KeyValuePair<AgAkisTakipModel, List<AgAkisTakipModel>>>();
        public AgAkisGosterPopupWindow(NodeViewModel ucBirim)
        {
            this.ucBirim = ucBirim;
            InitializeComponent();

            Logic();
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

        #region LogicEvent
        private void Logic()
        {
            List<AgAkisTakipModel> tempKey = new List<AgAkisTakipModel>();

            foreach (var output in ucBirim.Transitions.Items)
            {
                if(output.Connect != null)
                {
                    if(output.AgAkisList != null && output.AgAkisList.Count() > 0)
                    {
                        foreach(var agAkis in output.AgAkisList)
                        {
                            foreach(var varisNoktasi in agAkis.VarisNoktasiIdNameList.Select(s => s.Value).ToList())
                            {
                                if(tempKey.Where(x => x.ToNode == varisNoktasi).Any())
                                {
                                    var item = tempKey.Where(x => x.ToNode == varisNoktasi).FirstOrDefault();
                                    item.Yuk += agAkis.Yuk;
                                    tempKey.Remove(item);
                                    tempKey.Add(item);
                                }
                                else
                                {
                                    tempKey.Add(new AgAkisTakipModel { FromNode = ucBirim.Name, ToNode = varisNoktasi, Yuk = agAkis.Yuk });
                                }
                            }
                        }
                    }
                }
            }

            var connects = ucBirim.NodesCanvas.Connects;

            foreach(var key in tempKey)
            {
                List<AgAkisTakipModel> tempValue = new List<AgAkisTakipModel>();

                foreach (var connect in connects)
                {
                    if (connect.ToConnector.Node.TypeId == (int)TipEnum.UcBirim)
                    {
                        if(connect.ToConnector.Node.Name == key.ToNode)
                        {
                            tempValue.Add(new AgAkisTakipModel { FromNode = connect.FromConnector.Node.Name, ToNode = connect.ToConnector.Node.Name, Yuk = connect.AgYuku });
                        }
                    }
                    else
                    {
                        if (connect.ToConnector.AgAkisList.Where(x => x.FromNodeUniqueId == ucBirim.UniqueId).Any())
                        {
                            foreach (var agAkis in connect.ToConnector.AgAkisList)
                            {
                                if (agAkis.VarisNoktasiIdNameList.Select(s => s.Value).ToList().Contains(key.ToNode))
                                {
                                    tempValue.Add(new AgAkisTakipModel { FromNode = connect.FromConnector.Node.Name, ToNode = connect.ToConnector.Node.Name, Yuk = connect.AgYuku });
                                    break;
                                }
                            }
                        }
                    }
                }

                agAkisTakipList.Add(new KeyValuePair<AgAkisTakipModel, List<AgAkisTakipModel>>(key, tempValue));
            }

            Console.WriteLine(agAkisTakipList);
            AgAkisDataGrid.ItemsSource = agAkisTakipList;
        }
        #endregion
    }

    public class AgAkisTakipModel
    {
        public string FromNode { get; set; }
        public string ToNode { get; set; }
        public decimal Yuk { get; set; }
    }
}
