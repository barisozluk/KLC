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

        public MainWindow MainWindow { get; set; }

        public ImportLibraryPopupWindow()
        {
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
        }

        private void ButtonImportLibraryPopupClose_Click(object sender, RoutedEventArgs e)
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
