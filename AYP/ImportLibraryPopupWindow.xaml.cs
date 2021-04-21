using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Models;
using AYP.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariTurPopWindow.xaml
    /// </summary>
    public partial class ImportLibraryPopupWindow : Window
    {

        private string fileName;
        AYPContext context;
        IUcBirimService ucBirimService;
        IAgAnahtariService agAnahtariService;
        IGucUreticiService gucUreticiService;

        public ImportLibraryPopupWindow()
        {
            context = new AYPContext();
            ucBirimService = new UcBirimService(context);
            agAnahtariService = new AgAnahtariService(context);
            gucUreticiService = new GucUreticiService(context);

            InitializeComponent();
        }

        private void Import_ProjectLibrary(object sender, RoutedEventArgs e)
        {
            List<DbImportExportObjectModel> models = JsonConvert.DeserializeObject<List<DbImportExportObjectModel>>(File.ReadAllText(fileName));

            var gucArayuzuList = new List<GucArayuzu>();
            var gucUreticiList = new List<GucUretici>();
            var ucBirimList = new List<UcBirim>();
            var agAnahtariAgArayuzuList = new List<AgAnahtariAgArayuzu>();
            var agAnahtariGucArayuzuList = new List<AgAnahtariGucArayuzu>();
            var gucUreticiGucArayuzuList = new List<GucUreticiGucArayuzu>();
            var ucBirimAgArayuzuList = new List<UcBirimAgArayuzu>();
            var ucBirimGucArayuzuList = new List<UcBirimGucArayuzu>();
            var agAnahtariTurList = new List<AgAnahtariTur>();
            var gucUreticiTurList = new List<GucUreticiTur>();
            var ucBirimTurList = new List<UcBirimTur>();
            var agAnahtariList = new List<AgAnahtari>();
            var agArayuzuList = new List<AgArayuzu>();

            foreach (var model in models)
            {
                if (model.tableName == "GucArayuzu")
                {
                    foreach (var row in model.rows)
                    {
                        gucArayuzuList.Add(row.ToObject<GucArayuzu>());
                    }
                }
                else if (model.tableName == "GucUretici")
                {
                    foreach (var row in model.rows)
                    {
                        gucUreticiList.Add(row.ToObject<GucUretici>());
                    }
                }
                else if (model.tableName == "UcBirim")
                {
                    foreach (var row in model.rows)
                    {
                        ucBirimList.Add(row.ToObject<UcBirim>());
                    }
                }
                else if (model.tableName == "AgAnahtariAgArayuzu")
                {
                    foreach (var row in model.rows)
                    {
                        agAnahtariAgArayuzuList.Add(row.ToObject<AgAnahtariAgArayuzu>());
                    }
                }
                else if (model.tableName == "AgAnahtariGucArayuzu")
                {
                    foreach (var row in model.rows)
                    {
                        agAnahtariGucArayuzuList.Add(row.ToObject<AgAnahtariGucArayuzu>());
                    }
                }
                else if (model.tableName == "GucUreticiGucArayuzu")
                {
                    foreach (var row in model.rows)
                    {
                        gucUreticiGucArayuzuList.Add(row.ToObject<GucUreticiGucArayuzu>());
                    }
                }
                else if (model.tableName == "UcBirimAgArayuzu")
                {
                    foreach (var row in model.rows)
                    {
                        ucBirimAgArayuzuList.Add(row.ToObject<UcBirimAgArayuzu>());
                    }
                }
                else if (model.tableName == "UcBirimGucArayuzu")
                {
                    foreach (var row in model.rows)
                    {
                        ucBirimGucArayuzuList.Add(row.ToObject<UcBirimGucArayuzu>());
                    }
                }
                else if (model.tableName == "AgAnahtariTur")
                {
                    foreach (var row in model.rows)
                    {
                        agAnahtariTurList.Add(row.ToObject<AgAnahtariTur>());
                    }
                }
                else if (model.tableName == "GucUreticiTur")
                {
                    foreach (var row in model.rows)
                    {
                        gucUreticiTurList.Add(row.ToObject<GucUreticiTur>());
                    }
                }
                else if (model.tableName == "UcBirimTur")
                {
                    foreach (var row in model.rows)
                    {
                        ucBirimTurList.Add(row.ToObject<UcBirimTur>());
                    }
                }
                else if (model.tableName == "AgAnahtari")
                {
                    foreach (var row in model.rows)
                    {
                        agAnahtariList.Add(row.ToObject<AgAnahtari>());
                    }
                }
                else if (model.tableName == "AgArayuzu")
                {
                    foreach (var row in model.rows)
                    {
                        agArayuzuList.Add(row.ToObject<AgArayuzu>());
                    }
                }
            }

            foreach (var ucBirimTur in ucBirimTurList)
            {
                var tempUcBirimList = new List<UcBirim>();
                var tempUcBirimAgArayuzuList = new List<UcBirimAgArayuzu>();
                var tempAgArayuzuList = new List<AgArayuzu>();
                var tempUcBirimGucArayuzuList = new List<UcBirimGucArayuzu>();
                var tempGucArayuzuList = new List<GucArayuzu>();

                tempUcBirimList = ucBirimList.Where(ub => ub.UcBirimTurId == ucBirimTur.Id).ToList();
                foreach (var ucBirim in tempUcBirimList)
                {
                    tempUcBirimAgArayuzuList.AddRange(ucBirimAgArayuzuList.Where(ubaa => ubaa.UcBirimId == ucBirim.Id).ToList());
                    tempUcBirimGucArayuzuList.AddRange(ucBirimGucArayuzuList.Where(ubga => ubga.UcBirimId == ucBirim.Id).ToList());
                }

                foreach(var ucBirimAgArayuzu in tempUcBirimAgArayuzuList)
                {
                    tempAgArayuzuList.AddRange(agArayuzuList.Where(aa => aa.Id == ucBirimAgArayuzu.AgArayuzuId).ToList());
                }

                foreach (var ucBirimGucArayuzu in tempUcBirimGucArayuzuList)
                {
                    tempGucArayuzuList.AddRange(gucArayuzuList.Where(ga => ga.Id == ucBirimGucArayuzu.GucArayuzuId).ToList());
                }

                ucBirimService.ImportUcBirimLibrary(ucBirimTur, tempUcBirimList, tempUcBirimAgArayuzuList, tempAgArayuzuList, tempUcBirimGucArayuzuList, tempGucArayuzuList);
            }

            foreach (var agAnahtariTur in agAnahtariTurList)
            {
                var tempAgAnahtariList = new List<AgAnahtari>();
                var tempAgAnahtariAgArayuzuList = new List<AgAnahtariAgArayuzu>();
                var tempAgArayuzuList = new List<AgArayuzu>();
                var tempAgAnahtariGucArayuzuList = new List<AgAnahtariGucArayuzu>();
                var tempGucArayuzuList = new List<GucArayuzu>();

                tempAgAnahtariList = agAnahtariList.Where(ub => ub.AgAnahtariTurId == agAnahtariTur.Id).ToList();
                foreach (var agAnahtari in tempAgAnahtariList)
                {
                    tempAgAnahtariAgArayuzuList.AddRange(agAnahtariAgArayuzuList.Where(ubaa => ubaa.AgAnahtariId == agAnahtari.Id).ToList());
                    tempAgAnahtariGucArayuzuList.AddRange(agAnahtariGucArayuzuList.Where(ubga => ubga.AgAnahtariId == agAnahtari.Id).ToList());
                }

                foreach (var agAnahtariAgArayuzu in tempAgAnahtariAgArayuzuList)
                {
                    tempAgArayuzuList.AddRange(agArayuzuList.Where(aa => aa.Id == agAnahtariAgArayuzu.AgArayuzuId).ToList());
                }

                foreach (var agAnahtariGucArayuzu in tempAgAnahtariGucArayuzuList)
                {
                    tempGucArayuzuList.AddRange(gucArayuzuList.Where(ga => ga.Id == agAnahtariGucArayuzu.GucArayuzuId).ToList());
                }

                agAnahtariService.ImportAgAnahtariLibrary(agAnahtariTur, tempAgAnahtariList, tempAgAnahtariAgArayuzuList, tempAgArayuzuList, tempAgAnahtariGucArayuzuList, tempGucArayuzuList);
            }

            foreach (var gucUreticiTur in gucUreticiTurList)
            {
                var tempGucUreticiList = new List<GucUretici>();
                var tempGucUreticiGucArayuzuList = new List<GucUreticiGucArayuzu>();
                var tempGucArayuzuList = new List<GucArayuzu>();

                tempGucUreticiList = gucUreticiList.Where(ub => ub.GucUreticiTurId == gucUreticiTur.Id).ToList();
                foreach (var gucUretici in tempGucUreticiList)
                {
                    tempGucUreticiGucArayuzuList.AddRange(gucUreticiGucArayuzuList.Where(ubga => ubga.GucUreticiId == gucUretici.Id).ToList());
                }

                foreach (var gucUreticiGucArayuzu in tempGucUreticiGucArayuzuList)
                {
                    tempGucArayuzuList.AddRange(gucArayuzuList.Where(ga => ga.Id == gucUreticiGucArayuzu.GucArayuzuId).ToList());
                }

                gucUreticiService.ImportGucUreticiLibrary(gucUreticiTur, tempGucUreticiList, tempGucUreticiGucArayuzuList, tempGucArayuzuList);
            }

            var mainWindow = Owner as MainWindow;
            mainWindow.ListUcBirim();
            mainWindow.ListAgAnahtari();
            mainWindow.ListGucUretici();

            NotifySuccessPopup nfp = new NotifySuccessPopup();
            nfp.msg.Text = "İşlem başarı ile gerçekleştirildi.";
            nfp.Owner = Owner;
            nfp.Show();

            ClosePopup();
        }

        private void ButtonImportLibraryPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ImportJSONFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                ImportJSON.Text = fileName;
            }
        }
    }
}
