﻿using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Interaction logic for GucUreticiTurPopupWindow.xaml
    /// </summary>
    public partial class GucUreticiTurPopupWindow : Window
    {
        MainWindow window;

        private AYPContext context;

        private IGucUreticiService service;

        GucUreticiTur gucUreticiTur;
        public GucUreticiTurPopupWindow(MainWindow window)
        {
            this.window = window;
            this.context = new AYPContext();
            service = new GucUreticiService(this.context);
            gucUreticiTur = new GucUreticiTur();

            InitializeComponent();
            DataContext = gucUreticiTur;
        }

        private void Save_GucUreticiTur(object sender, RoutedEventArgs e)
        {
            var validationContext = new ValidationContext(gucUreticiTur, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucUreticiTur, validationContext, results, true))
            {
                var response = service.SaveGucUreticiTur(gucUreticiTur);

                if (!response.HasError)
                {
                    Hide();
                    Owner.IsEnabled = true;
                }
            }
            else
            {
                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        if (memberName == "Ad")
                        {
                            Ad.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }
        }


        private void ButtonGucUreticiTurPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.window.IsEnabled = true;
        }
    }
}
