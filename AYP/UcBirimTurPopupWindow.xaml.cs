using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            Hide();
            Owner.IsEnabled = true;
        }

        private void Save_UcBirimTur(object sender, RoutedEventArgs e)
        {
            var validationContext = new ValidationContext(ucBirimTur, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(ucBirimTur, validationContext, results, true))
            {
                var response = service.SaveUcBirimTur(ucBirimTur);

                if (!response.HasError)
                {
                    Hide();
                    Owner.IsEnabled = true;
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
            }
        }
    }
}
