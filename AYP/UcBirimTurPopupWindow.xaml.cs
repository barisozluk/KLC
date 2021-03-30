﻿using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
    public partial class UcBirimTurPopupWindow : Window
    {
        private AYPContext context;
        
        private IUcBirimService service;

        UcBirimTur ucBirimTur;
        public MainWindow MainWindow { get; set; }



        public UcBirimTurPopupWindow()
        {
            context = new AYPContext();
            service = new UcBirimService(context);
            ucBirimTur = new UcBirimTur();

            InitializeComponent();
            DataContext = ucBirimTur;
        }

        private void ButtonUcBirimTurPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void Save_UcBirimTur(object sender, RoutedEventArgs e)
        {
            NotificationManager notificationManager = new NotificationManager();

            var validationContext = new ValidationContext(ucBirimTur, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(ucBirimTur, validationContext, results, true))
            {
                var response = service.SaveUcBirimTur(ucBirimTur);
                
                if (!response.HasError)
                {
                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();

                    Close();
                    Owner.IsEnabled = true;
                    Owner.Effect = null;
                }
                else
                {
                    NotifyWarningPopup nfp = new NotifyWarningPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                foreach(var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        if (memberName == "Ad")
                        {
                            Ad.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, zorunlu alanları doldurunuz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }
    }
}
