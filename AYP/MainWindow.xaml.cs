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
using Microsoft.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Html2pdf;
using iText.Layout.Element;
using iText.IO.Image;
using Image = iText.Layout.Element.Image;
using iText.Html2pdf.Resolver.Font;
using iText.Layout.Properties;
using log4net;
using System.Reflection;

namespace AYP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool agWorkspaceSeciliMi = true;
        public bool gucWorkspaceSeciliMi = true;

        public bool toggleRight = true;
        public bool toggleLeft = true;
        public bool isClose = false;
        public int midColSize;

        public int selectedTipId = 0;
        UcBirim selectedUcBirim;
        AgAnahtari selectedAgAnahtari;
        GucUretici selectedGucUretici;

        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainWindowViewModel)value; }
        }
        #endregion ViewModel

        private readonly IUcBirimService ucBirimService;
        private readonly IAgAnahtariService agAnahtariService;
        private readonly IGucUreticiService gucUreticiService;
        private readonly AYPContext context;

        public MainWindow()
        {
            InitializeComponent();
            AllowMultiSelection(ProjeHiyerarsi);
            ViewModel = new MainWindowViewModel(this.NodesCanvas.ViewModel);
            ViewModel.NodesCanvas.MainWindow = this;

            SetupSubscriptions();
            SetupBinding();
            SetupEvents();

            GucPlanlamaCheckBox.IsChecked = gucWorkspaceSeciliMi;
            AgPlanlamaCheckbox.IsChecked = agWorkspaceSeciliMi;

            context = new AYPContext();
            ucBirimService = new UcBirimService(context);
            agAnahtariService = new AgAnahtariService(context);
            gucUreticiService = new GucUreticiService(context);

            ListUcBirim();
            ListAgAnahtari();
            ListGucUretici();

        }

        #region Setup Binding

        private void SetupBinding()
        {

            this.WhenActivated(disposable =>
            {
                //this.OneWayBind(this.ViewModel, x => x.Tests, x => x.TableOfTransitions.ItemsSource).DisposeWith(disposable);

                //var SelectedItem = this.ObservableForProperty(x => x.MessageList.SelectedItem).Select(x => (x.Value as MessageViewModel)?.Text);
                //this.BindCommand(this.ViewModel, x => x.CommandCopyError, x => x.BindingCopyError, SelectedItem).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.CommandCopyError, x => x.ItemCopyError, SelectedItem).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.NodesCanvas.Dialog, x => x.Dialog.ViewModel).DisposeWith(disposable);
                //this.OneWayBind(this.ViewModel, x => x.Transitions, x => x.TableOfTransitions.ItemsSource).DisposeWith(disposable);
                //this.OneWayBind(this.ViewModel, x => x.Messages, x => x.MessageList.ItemsSource).DisposeWith(disposable);
                //this.OneWayBind(this.ViewModel, x => x.DebugEnable, x => x.LabelDebug.Visibility).DisposeWith(disposable);

                //this.OneWayBind(this.ViewModel, x => x.CountError, x => x.LabelError.Content, value => value.ToString() + " Error").DisposeWith(disposable);
                //this.OneWayBind(this.ViewModel, x => x.CountWarning, x => x.LabelWarning.Content, value => value.ToString() + " Warning").DisposeWith(disposable);
                //this.OneWayBind(this.ViewModel, x => x.CountInformation, x => x.LabelInformation.Content, value => value.ToString() + " Information").DisposeWith(disposable);
                //this.OneWayBind(this.ViewModel, x => x.CountDebug, x => x.LabelDebug.Content, value => value.ToString() + " Debug").DisposeWith(disposable);


                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandChangeTheme, x => x.ButtonChangeTheme).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.CommandCopySchemeName, x => x.ItemCopySchemeName).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSelectAll, x => x.ItemSelectAll).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZoomIn, x => x.ButtonZoomIn).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZoomOut, x => x.ButtonZoomOut).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZoomOriginalSize, x => x.ButtonZoomOriginalSize).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandCollapseUpAll, x => x.ButtonCollapseUpAll).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExpandDownAll, x => x.ButtonExpandDownAll).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandUndo, x => x.ItemUndo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandUndo, x => x.ButtonUndo).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandRedo, x => x.ItemRedo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandRedo, x => x.ButtonRedo).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.BindingExportToPNG).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.ItemExportToPNG).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.ButtonExportToPNG).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.BindingExportToJPEG).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.ItemExportToJPEG).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandNew, x => x.BindingNew).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandNew, x => x.ItemNew).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandNew, x => x.ButtonNew).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandOpen, x => x.BindingOpen).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandOpen, x => x.ItemOpen).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandOpen, x => x.ButtonOpen).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSave, x => x.BindingSave).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSave, x => x.ItemSave).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSave, x => x.ButtonSave).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.BindingSaveAs).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.ItemSaveAs).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.ButtonSaveAs).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExit, x => x.BindingExit).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExit, x => x.ItemExit).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandCopy, x => x.ButtonCopy).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandPaste, x => x.ButtonPaste).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandAlignLeft, x => x.ButtonAlignLeft).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandAlignRight, x => x.ButtonAlignRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandAlignCenter, x => x.ButtonAlignCenter).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandEditSelected, x => x.ButtonEditSelected).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandGroup, x => x.ButtonGroup).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandUngroup, x => x.ButtonUngroup).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandYildizTopolojiOlustur, x => x.ButtonYildizTopoloji).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandHalkaTopolojiOlustur, x => x.ButtonHalkaTopoloji).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZincirTopolojiOlustur, x => x.ButtonZincirTopoloji).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandHalkaTopolojiOlustur, x => x.ButtonHalkaTopoloji).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandYildizTopolojiOlustur, x => x.ButtonYildizTopoloji).DisposeWith(disposable);

            });
        }
        #endregion Setup Binding

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel.NodesCanvas.SchemePath).Subscribe(value => UpdateSchemeName(value)).DisposeWith(disposable);
                this.WhenAnyValue(x => x.NodesCanvas.ViewModel.NeedExit).Where(x => x).Subscribe(_ => this.Close()).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.CountError).Buffer(2, 1).Where(x => x[1] > x[0]).Subscribe(_ => ShowError()).DisposeWith(disposable);

                //this.WhenAnyValue(x => x.ActualWidth).Subscribe(value => TableOfTransitionsColumn.MaxWidth = value - 50).DisposeWith(disposable);
                //this.WhenAnyValue(x => x.ActualHeight).Subscribe(value => Fotter.MaxHeight = value - 150).DisposeWith(disposable);
            });
        }
        private void UpdateSchemeName(string newName)
        {
            //this.LabelSchemeName.Visibility = string.IsNullOrEmpty(newName) ? Visibility.Hidden : Visibility.Visible;
            //if (!string.IsNullOrEmpty(newName))
            //{
            //    this.LabelSchemeName.Content = Path.GetFileNameWithoutExtension(newName);
            //    this.LabelSchemeName.ToolTip = newName;
            //}
        }
        #endregion Setup Subscriptions

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                //this.MessageList.Events().MouseDoubleClick.Subscribe(_ => ViewModel.CommandCopyError.ExecuteWithSubscribe((MessageList.SelectedItem as MessageViewModel)?.Text)).DisposeWith(disposable);
                //this.LabelSchemeName.Events().MouseDoubleClick.WithoutParameter().InvokeCommand(ViewModel.CommandCopySchemeName).DisposeWith(disposable);
                //this.Header.Events().PreviewMouseLeftButtonDown.Subscribe(e => HeaderClick(e)).DisposeWith(disposable);
                //this.ErrorListExpander.Events().Collapsed.Subscribe(_ => ErrorListCollapse()).DisposeWith(disposable);
                //this.ErrorListExpander.Events().Expanded.Subscribe(_ => ErrorListExpanded()).DisposeWith(disposable);

                //this.TableOfTransitionsExpander.Events().Collapsed.Subscribe(_ => TableOfTransitionsCollapse()).DisposeWith(disposable);
                //this.TableOfTransitionsExpander.Events().Expanded.Subscribe(_ => TableOfTransitionsExpanded()).DisposeWith(disposable);

                //this.LabelError.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Error)).DisposeWith(disposable);
                //this.LabelWarning.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Warning)).DisposeWith(disposable);
                //this.LabelInformation.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Information)).DisposeWith(disposable);
                //this.LabelDebug.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Debug)).DisposeWith(disposable);
                //this.LabelErrorList.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.All)).DisposeWith(disposable);
                //this.LabelErrorListUpdate.Events().MouseLeftButtonDown.WithoutParameter().InvokeCommand(NodesCanvas.ViewModel.CommandErrorListUpdate).DisposeWith(disposable);

                //this.ButtonAddNode.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonAddNode, NodeCanvasClickMode.AddNode, e)).DisposeWith(disposable);
                //this.ButtonDeleteNode.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonDeleteNode, NodeCanvasClickMode.Delete, e)).DisposeWith(disposable);
                this.ButtonStartSelect.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonStartSelect, NodeCanvasClickMode.Select, e)).DisposeWith(disposable);
                this.ButtonStartCut.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonStartCut, NodeCanvasClickMode.Cut, e)).DisposeWith(disposable);
            });
        }

        private void SetDisplayMessageType(MouseButtonEventArgs e, TypeMessage typeMessage)
        {
            //if ((ErrorListExpander.IsExpanded) && (this.ViewModel.NodesCanvas.DisplayMessageType != typeMessage))
            //    e.Handled = true;

            //this.ViewModel.NodesCanvas.DisplayMessageType = typeMessage;
        }
        private void RadioButtonUnChecked(RadioButton radioButton, NodeCanvasClickMode clickMode, MouseButtonEventArgs e)
        {
            if (radioButton.IsChecked == true)
            {
                radioButton.IsChecked = false;
                e.Handled = true;

                ViewModel.NodesCanvas.ClickMode = NodeCanvasClickMode.Default;
            }
            else
            {
                ViewModel.NodesCanvas.ClickMode = clickMode;
            }
        }
        private void ErrorListCollapse()
        {
            //this.ErrorListSplitter.IsEnabled = false;
            //this.Fotter.Height = new GridLength();
            //this.Fotter.MinHeight = 18;

        }
        private void ErrorListExpanded()
        {
            //this.ErrorListSplitter.IsEnabled = true;
            //this.Fotter.Height = new GridLength(this.ViewModel.DefaultHeightMessagePanel);
            //this.Fotter.MinHeight = 52;
        }

        private void TableOfTransitionsCollapse()
        {
            //this.TableOfTransitionsSplitter.IsEnabled = false;
            //this.TableOfTransitionsColumn.Width = new GridLength();
            //this.TableOfTransitionsColumn.MinWidth = 18;
        }
        private void TableOfTransitionsExpanded()
        {
            //this.TableOfTransitionsSplitter.IsEnabled = true;
            //this.TableOfTransitionsColumn.Width = new GridLength(this.ViewModel.DefaultWidthTransitionsTable);
            //this.TableOfTransitionsColumn.MinWidth = 52;
        }
        private void ShowError()
        {
            //if (!this.ErrorListExpander.IsExpanded)
            //{
            //    this.ErrorListExpander.IsExpanded = true;
            //    ErrorListExpanded();
            //}
        }

        #endregion SetupEvents

        #region CollapseExpandEvents
        private void collapseExpandRight(object sender, RoutedEventArgs e)
        {
            if (toggleRight && toggleLeft)
            {
                toggleRight = false;

                this.RightColumn.Width = new GridLength(0, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1468, GridUnitType.Pixel);
                this.midRec.Width = 1468;

            }
            else if (!toggleRight && toggleLeft)
            {
                this.RightColumn.Width = new GridLength(362, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1106, GridUnitType.Pixel);
                this.midRec.Width = 1106;

                toggleRight = true;
            }
            else if (toggleRight && !toggleLeft)
            {
                toggleRight = false;
                this.RightColumn.Width = new GridLength(0, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1830, GridUnitType.Pixel);
                this.midRec.Width = 1830;
            }

            else if (!toggleRight && !toggleLeft)
            {
                this.RightColumn.Width = new GridLength(362, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1468, GridUnitType.Pixel);
                this.midRec.Width = 1468;

                toggleRight = true;
            }

            if (!toggleRight)
            {
                this.ButtonCloseColumn3.Visibility = Visibility.Visible;
            }
            else
            {
                this.ButtonCloseColumn3.Visibility = Visibility.Hidden;
            }
        }

        private void collapseExpandLeft(object sender, RoutedEventArgs e)
        {
            if (toggleRight && toggleLeft)
            {
                this.LeftColumn.Width = new GridLength(0, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1468, GridUnitType.Pixel);
                this.midRec.Width = 1468;

                toggleLeft = false;
            }
            else if (toggleRight && !toggleLeft)
            {
                this.LeftColumn.Width = new GridLength(362, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1106, GridUnitType.Pixel);
                this.midRec.Width = 1106;

                toggleLeft = true;
            }
            else if (!toggleRight && toggleLeft)
            {
                toggleLeft = false;
                this.LeftColumn.Width = new GridLength(0, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1830, GridUnitType.Pixel);
                this.midRec.Width = 1830;
            }
            else if (!toggleRight && !toggleLeft)
            {
                this.LeftColumn.Width = new GridLength(362, GridUnitType.Pixel);
                this.MiddleColumn.Width = new GridLength(1468, GridUnitType.Pixel);
                this.midRec.Width = 1468;

                toggleLeft = true;
            }
        }


        #endregion CollapseExpandEvents

        #region DescribingPopupEvents

        private void ButtonDescribing_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = true;
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;
        }

        private void ButtonDescribingPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            IsEnabled = true;
            this.Effect = null;
        }

        #endregion

        #region UcBirimPopupEvents
        private void ButtonUcBirimTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            UcBirimTurPopupWindow popup = new UcBirimTurPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonUcBirim_Click(object sender, RoutedEventArgs e)
        {
            bool fromNode = false;

            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            UcBirimPopupWindow popup = new UcBirimPopupWindow(null, fromNode);
            popup.Owner = this;
            popup.ShowDialog();
        }
        #endregion

        #region AgAnahtariPopupEvents
        private void ButtonAgAnahtariTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;
            AgAnahtariTurPopupWindow popup = new AgAnahtariTurPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonAgAnahtari_Click(object sender, RoutedEventArgs e)
        {
            bool fromNode = false;

            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            AgAnahtariPopupWindow popup = new AgAnahtariPopupWindow(null, fromNode);
            popup.Owner = this;
            popup.ShowDialog();
        }
        #endregion

        #region GucUreticiPopupEvents
        private void ButtonGucUreticiTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            GucUreticiTurPopupWindow popup = new GucUreticiTurPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonGucUretici_Click(object sender, RoutedEventArgs e)
        {
            bool fromNode = false;

            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            GucUreticiPopupWindow popup = new GucUreticiPopupWindow(null, fromNode, this);
            popup.Owner = this;
            popup.ShowDialog();
        }
        #endregion

        #region SettingsPopupEvents
        private void Versiyon_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            SettingsPopupWindow popup = new SettingsPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }
        #endregion

        #region MinimizeAppEvent
        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region UcBirimPanelEvents
        public void ListUcBirim()
        {
            UcBirimDataGrid.ItemsSource = ucBirimService.ListUcBirim();

            Tanim.Text = null;
            StokNo.Text = null;
            Uretici.Text = null;
            TurAd.Text = null;
        }

        private void UcBirim_Click(object sender, RoutedEventArgs e)
        {
            var ucBirimId = Convert.ToInt32(((Button)sender).Tag);
            selectedUcBirim = ucBirimService.GetUcBirimById(ucBirimId);
            selectedTipId = selectedUcBirim.TipId;

            Tanim.Text = selectedUcBirim.Tanim;
            StokNo.Text = selectedUcBirim.StokNo;
            Uretici.Text = selectedUcBirim.UreticiAdi;
            TurAd.Text = selectedUcBirim.UcBirimTur.Ad;
        }

        #endregion

        #region AgAnahtariPanelEvents
        public void ListAgAnahtari()
        {
            AgAnahtariDataGrid.ItemsSource = agAnahtariService.ListAgAnahtari();

            Tanim.Text = null;
            StokNo.Text = null;
            Uretici.Text = null;
            TurAd.Text = null;
        }

        private void AgAnahtari_Click(object sender, RoutedEventArgs e)
        {
            var agAnahtariId = Convert.ToInt32(((Button)sender).Tag);
            selectedAgAnahtari = agAnahtariService.GetAgAnahtariById(agAnahtariId);
            selectedTipId = selectedAgAnahtari.TipId;

            Tanim.Text = selectedAgAnahtari.Tanim;
            StokNo.Text = selectedAgAnahtari.StokNo;
            Uretici.Text = selectedAgAnahtari.UreticiAdi;
            TurAd.Text = selectedAgAnahtari.AgAnahtariTur.Ad;
        }

        #endregion

        #region GucUreticiPanelEvents
        public void ListGucUretici()
        {
            GucUreticiDataGrid.ItemsSource = null;
            GucUreticiDataGrid.ItemsSource = gucUreticiService.ListGucUretici();

            Tanim.Text = null;
            StokNo.Text = null;
            Uretici.Text = null;
            TurAd.Text = null;
        }

        private void GucUretici_Click(object sender, RoutedEventArgs e)
        {
            var gucUreticiId = Convert.ToInt32(((Button)sender).Tag);
            selectedGucUretici = gucUreticiService.GetGucUreticiById(gucUreticiId);
            selectedTipId = selectedGucUretici.TipId;

            Tanim.Text = selectedGucUretici.Tanim;
            StokNo.Text = selectedGucUretici.StokNo;
            Uretici.Text = selectedGucUretici.UreticiAdi;
            TurAd.Text = selectedGucUretici.GucUreticiTur.Ad;
        }

        #endregion

        #region BilgiKartiDetayEvent

        private void BilgiKartiDetay_Click(object sender, RoutedEventArgs e)
        {
            bool fromNode = false;

            if (selectedTipId == (int)TipEnum.UcBirim)
            {
                this.IsEnabled = false;
                System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                blur.Radius = 2;
                this.Effect = blur;
                UcBirimPopupWindow popup = new UcBirimPopupWindow(selectedUcBirim, fromNode);
                popup.Owner = this;
                popup.ShowDialog();
            }
            else if (selectedTipId == (int)TipEnum.AgAnahtari)
            {
                this.IsEnabled = false;
                System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                blur.Radius = 2;
                this.Effect = blur;

                AgAnahtariPopupWindow popup = new AgAnahtariPopupWindow(selectedAgAnahtari, fromNode);
                popup.Owner = this;
                popup.ShowDialog();
            }
            else if (selectedTipId == (int)TipEnum.GucUretici)
            {
                this.IsEnabled = false;
                System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                blur.Radius = 2;
                this.Effect = blur;

                GucUreticiPopupWindow popup = new GucUreticiPopupWindow(selectedGucUretici, fromNode, this);
                popup.Owner = this;
                popup.ShowDialog();
            }
        }
        #endregion

        #region CloseAppPopupEvent
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            CloseAppPopupWindow popup = new CloseAppPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }
        #endregion

        #region Drag/Drop Events
        Button ClickedElement = null;
        bool IsDragDropEvent = false;
        private void cihaz_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (!e.Source.Equals(sender))
            {
                IsDragDropEvent = true;
                ClickedElement = (Button)sender;
                DragDrop.DoDragDrop(sender as DependencyObject, ClickedElement, DragDropEffects.Move);

                e.Handled = true;
            }
        }

        private void target_Drop(object sender, DragEventArgs e)
        {
            if (IsDragDropEvent)
            {
                System.Windows.Point droppedpoint = e.GetPosition(sender as NodesCanvas);
                this.NodesCanvas.PositionMove = droppedpoint;
                var dataCxtx = ClickedElement.DataContext;
                var type = dataCxtx.GetType();

                NodeViewModel model = new NodeViewModel();
                if (type.Name == "UcBirim")
                {
                    var ucBirim = (UcBirim)dataCxtx;
                    model.Id = ucBirim.Id;
                    model.TypeId = ucBirim.TipId;
                    model.AgArayuzuList = ucBirimService.ListUcBirimAgArayuzu(ucBirim.Id);
                    model.GucArayuzuList = ucBirimService.ListUcBirimGucArayuzu(ucBirim.Id);
                    model.Sembol = ucBirim.Sembol;
                    model.Tanim = ucBirim.Tanim;
                    model.StokNo = ucBirim.StokNo;
                    model.UreticiAdi = ucBirim.UreticiAdi;
                    model.UreticiParcaNo = ucBirim.UreticiParcaNo;
                    model.TurAd = ucBirimService.GetUcBirimTurById(ucBirim.UcBirimTurId).Ad;
                }
                else if (type.Name == "AgAnahtari")
                {
                    var agAnahtari = (AgAnahtari)dataCxtx;
                    model.Id = agAnahtari.Id;
                    model.TypeId = agAnahtari.TipId;
                    model.AgArayuzuList = agAnahtariService.ListAgAnahtariAgArayuzu(agAnahtari.Id);
                    model.GucArayuzuList = agAnahtariService.ListAgAnahtariGucArayuzu(agAnahtari.Id);
                    model.Sembol = agAnahtari.Sembol;
                    model.Tanim = agAnahtari.Tanim;
                    model.StokNo = agAnahtari.StokNo;
                    model.UreticiAdi = agAnahtari.UreticiAdi;
                    model.UreticiParcaNo = agAnahtari.UreticiParcaNo;
                    model.TurAd = agAnahtariService.GetAgAnahtariTurById(agAnahtari.AgAnahtariTurId).Ad;
                }
                else if (type.Name == "GucUretici")
                {
                    var gucUretici = (GucUretici)dataCxtx;
                    model.Id = gucUretici.Id;
                    model.TypeId = gucUretici.TipId;
                    model.AgArayuzuList = new List<AgArayuzu>();
                    model.GucArayuzuList = gucUreticiService.ListGucUreticiGucArayuzu(gucUretici.Id);
                    model.VerimlilikOrani = gucUretici.VerimlilikDegeri.HasValue ? gucUretici.VerimlilikDegeri.Value : 0;
                    model.DahiliGucTuketimDegeri = gucUretici.DahiliGucTuketimDegeri.HasValue ? gucUretici.DahiliGucTuketimDegeri.Value : 0;
                    model.Sembol = gucUretici.Sembol;
                    model.Tanim = gucUretici.Tanim;
                    model.StokNo = gucUretici.StokNo;
                    model.UreticiAdi = gucUretici.UreticiAdi;
                    model.UreticiParcaNo = gucUretici.UreticiParcaNo;
                    model.TurAd = gucUreticiService.GetGucUreticiTurById(gucUretici.GucUreticiTurId).Ad;
                }

                if (model.TypeId == (int)TipEnum.AgAnahtari || model.TypeId == (int)TipEnum.UcBirim)
                {
                    if (agWorkspaceSeciliMi)
                    {
                        ExternalNode data = new ExternalNode();
                        data.Node = model;
                        data.Point = NodesCanvas.PositionMove;
                        this.NodesCanvas.ViewModel.CommandAddNodeWithUndoRedo.Execute(data);
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Şu an Ağ Planlama üzerine çalışmamaktasınız, Uç Birim veya Ağ Anahtarı ekleyemezsiniz.";
                        nfp.Owner = this;
                        nfp.Show();
                    }
                }
                else
                {
                    if (gucWorkspaceSeciliMi)
                    {
                        ExternalNode data = new ExternalNode();
                        data.Node = model;
                        data.Point = NodesCanvas.PositionMove;
                        this.NodesCanvas.ViewModel.CommandAddNodeWithUndoRedo.Execute(data);
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Şu an Güç Planlama üzerine çalışmamaktasınız, Güç Üretici ekleyemezsiniz.";
                        nfp.Owner = this;
                        nfp.Show();
                    }

                }

                IsDragDropEvent = false;
            }

            e.Handled = true;
        }
        #endregion

        #region WorkspaceEvents
        private void AgPlanlama_Checked(object sender, RoutedEventArgs e)
        {
            agWorkspaceSeciliMi = true;
            ShowAgComponents();
        }

        private void AgPlanlama_Unchecked(object sender, RoutedEventArgs e)
        {
            agWorkspaceSeciliMi = false;
            HideAgComponents();
        }

        private void GucPlanlama_Checked(object sender, RoutedEventArgs e)
        {
            gucWorkspaceSeciliMi = true;
            ShowGucComponents();
        }

        private void GucPlanlama_Unchecked(object sender, RoutedEventArgs e)
        {
            gucWorkspaceSeciliMi = false;
            HideGucComponents();
        }

        private void ShowGucComponents()
        {
            if (this.ViewModel != null)
            {
                var nodes = this.ViewModel.NodesCanvas.Nodes.Items;

                if (agWorkspaceSeciliMi)
                {
                    foreach (var node in nodes)
                    {
                        node.IsVisible = true;
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        connect.IsVisible = true;
                    }
                }
                else
                {
                    foreach (var node in nodes)
                    {
                        node.IsVisible = true;
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        if (connect.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu || connect.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            connect.IsVisible = false;
                        }

                        if (connect.ToConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu || connect.ToConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            connect.IsVisible = false;
                        }
                    }
                }
            }
        }

        private void HideGucComponents()
        {
            if (this.ViewModel != null)
            {
                var nodes = this.ViewModel.NodesCanvas.Nodes.Items;

                if (agWorkspaceSeciliMi)
                {
                    foreach (var node in nodes)
                    {
                        if (node.TypeId != (int)TipEnum.GucUretici)
                        {
                            node.IsVisible = true;
                        }
                        else
                        {
                            node.IsVisible = false;
                        }
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        if (connect.FromConnector.Node.TypeId != (int)TipEnum.GucUretici && connect.ToConnector.Node.TypeId != (int)TipEnum.GucUretici)
                        {
                            connect.IsVisible = true;
                        }
                        else
                        {
                            connect.IsVisible = false;
                        }
                    }
                }
                else
                {
                    foreach (var node in nodes)
                    {
                        node.IsVisible = false;
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        connect.IsVisible = false;
                    }
                }
            }
        }

        private void ShowAgComponents()
        {
            if (this.ViewModel != null)
            {
                var nodes = this.ViewModel.NodesCanvas.Nodes.Items;

                if (agWorkspaceSeciliMi && gucWorkspaceSeciliMi)
                {
                    foreach (var node in nodes)
                    {
                        node.IsVisible = true;
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        connect.IsVisible = true;
                    }
                }
                else
                {
                    foreach (var node in nodes)
                    {
                        if (node.TypeId != (int)TipEnum.GucUretici)
                        {
                            node.IsVisible = true;
                        }
                        else
                        {
                            node.IsVisible = false;
                        }
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        if (connect.FromConnector.Node.TypeId != (int)TipEnum.GucUretici && connect.ToConnector.Node.TypeId != (int)TipEnum.GucUretici)
                        {
                            connect.IsVisible = true;
                        }
                        else
                        {
                            connect.IsVisible = false;
                        }
                    }
                }

            }
        }

        private void HideAgComponents()
        {
            if (this.ViewModel != null)
            {
                var nodes = this.ViewModel.NodesCanvas.Nodes.Items;
                if (gucWorkspaceSeciliMi)
                {
                    foreach (var node in nodes)
                    {
                        node.IsVisible = true;
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        if (connect.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu || connect.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            connect.IsVisible = false;
                        }

                        if (connect.ToConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu || connect.ToConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            connect.IsVisible = false;
                        }
                    }
                }
                else
                {
                    foreach (var node in nodes)
                    {
                        node.IsVisible = false;
                    }

                    foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                    {
                        connect.IsVisible = false;
                    }
                }
            }
        }

        #endregion

        #region ProjectHierarchy

        //Duracak
        public void ProjectHierarchyAdd(TreeViewItem AddedItem)
        {
            ProjeHiyerarsi.ItemsSource = null;
            ProjeHiyerarsi.Items.Add(AddedItem);
        }

        //Duracak
        public void ProjectHierarchyDeleteWithRedo(NodeViewModel DeletedItem)
        {
            ProjeHiyerarsi.ItemsSource = null;
            List<TreeViewItem> treeList = new List<TreeViewItem>();
            foreach (TreeViewItem searchfordelete in ProjeHiyerarsi.Items)
            {
                treeList.Add(searchfordelete);
            }
            foreach (TreeViewItem searchfordelete2 in treeList)
            {
                if (searchfordelete2.Header.ToString() == DeletedItem.Name)
                {
                    ProjeHiyerarsi.Items.Remove(searchfordelete2);
                }
            }
        }

        public void ProjectHierarchyDelete(List<NodeViewModel> NodesToDelete)
        {
            List<TreeViewItem> treeList = new List<TreeViewItem>();
            ProjeHiyerarsi.ItemsSource = null;

            foreach (TreeViewItem searchfordelete in ProjeHiyerarsi.Items)
            {
                treeList.Add(searchfordelete);
            }
            foreach (TreeViewItem searchfordelete2 in treeList)
            {
                foreach (var nodeViewModels in NodesToDelete)
                {
                    if (searchfordelete2.Header.ToString() == nodeViewModels.Name)
                    {
                        ProjeHiyerarsi.Items.Remove(searchfordelete2);
                    }
                }
            }
        }

        public void ProjectHierarchyDeleteForChild(string deleteNodeForChild)
        {
            List<TreeViewItem> treeList = new List<TreeViewItem>();
            ProjeHiyerarsi.ItemsSource = null;

            foreach (TreeViewItem searchfordelete in ProjeHiyerarsi.Items)
            {
                treeList.Add(searchfordelete);
            }
            foreach (TreeViewItem searchfordelete2 in treeList)
            {
                if (searchfordelete2.Header.ToString() == deleteNodeForChild)
                {
                    ProjeHiyerarsi.Items.Remove(searchfordelete2);
                }
            }
        }

        public void ProjectHierarchyAddChild(string rootName, NodeViewModel childNode)
        {
            List<TreeViewItem> treeList = new List<TreeViewItem>();
            ProjeHiyerarsi.ItemsSource = null;

            foreach (TreeViewItem searchfordelete in ProjeHiyerarsi.Items)
            {
                treeList.Add(searchfordelete);
            }

            foreach (TreeViewItem searchfordelete2 in treeList)
            {
                if (searchfordelete2.Header.ToString() == rootName)
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Header = childNode.Name;
                    searchfordelete2.Items.Add(newItem);
                }
            }
        }

        private static readonly System.Reflection.PropertyInfo IsSelectionChangeActiveProperty =
            typeof(TreeView).GetProperty("IsSelectionChangeActive", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        public void AllowMultiSelection(TreeView treeView)
        {
            if (IsSelectionChangeActiveProperty == null) return;

            var selectedItems = new List<TreeViewItem>();
            treeView.SelectedItemChanged += (a, b) =>
            {
                var treeViewItem = treeView.SelectedItem as TreeViewItem;
                if (treeViewItem == null) return;

                // allow multiple selection
                // when control key is pressed
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    // suppress selection change notification
                    // select all selected items
                    // then restore selection change notifications
                    var isSelectionChangeActive = IsSelectionChangeActiveProperty.GetValue(treeView, null);

                    IsSelectionChangeActiveProperty.SetValue(treeView, true, null);
                    selectedItems.ForEach(item => item.IsSelected = true);

                    IsSelectionChangeActiveProperty.SetValue(treeView, isSelectionChangeActive, null);
                }
                else
                {
                    // deselect all selected items except the current one
                    selectedItems.ForEach(item => item.IsSelected = (item == treeViewItem));
                    selectedItems.Clear();
                }

                if (!selectedItems.Contains(treeViewItem))
                {
                    selectedItems.Add(treeViewItem);
                }
                else
                {
                    // deselect if already selected
                    treeViewItem.IsSelected = false;
                    selectedItems.Remove(treeViewItem);
                }
                NodeSelect(selectedItems);
            };
        }

        private void NodeSelect(List<TreeViewItem> selectedItems)
        {
            foreach (var nodes in this.ViewModel.NodesCanvas.Nodes.Items)
            {
                nodes.Selected = false;
                foreach (var item in selectedItems)
                {
                    if (nodes.Name == item.Header.ToString())
                        nodes.Selected = true;
                }
            }
        }
        #endregion

        #region ExpanderCloser
        private void e1_Expanded(object sender, RoutedEventArgs e)
        {
            e2.IsExpanded = false;
            e3.IsExpanded = false;
        }

        private void e2_Expanded(object sender, RoutedEventArgs e)
        {
            e1.IsExpanded = false;
            e3.IsExpanded = false;
        }

        private void e3_Expanded(object sender, RoutedEventArgs e)
        {
            e1.IsExpanded = false;
            e2.IsExpanded = false;
        }
        #endregion

        #region HelpEvent
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var pdfOpenProcess = new System.Diagnostics.Process();
            //string pdfPath = Directory.GetCurrentDirectory() + "\\SEMA_Data\\StreamingAssets\\AYP\\SEMA-AYP-KK_v2.0_Kullanım Kılavuzu(001).pdf";
            string pdfPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"KullanimKilavuzu\SEMA-AYP-KK_v2.0_Kullanım Kılavuzu(001).pdf");
            if (!File.Exists(pdfPath))
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Kullanım kılavuzu henüz yüklenmemiştir!";
                nfp.Owner = Owner;
                nfp.Show();
            }
            else
            {
                pdfOpenProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(pdfPath)
                {
                    UseShellExecute = true
                };
                pdfOpenProcess.Start();
            }
        }
        #endregion

        #region RaporlamaEvents

        private void OnAgPlanlamaRaporuClick(object sender, RoutedEventArgs e)
        {
            var agPlanlamaNodes = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId != (int)TipEnum.GucUretici);

            if (agPlanlamaNodes.Count() > 0)
            {
                RaporBilgileriPopupWindow popup = new RaporBilgileriPopupWindow("Ağ Planlama Raporu", CaptureNodesCanvas());
                popup.Owner = this;
                popup.ShowDialog();
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Ağ planlama alanında bir çalışma yapılmadığından rapor alınamamaktadır.";
                nfp.Owner = this;
                nfp.Show();
            }
        }

        private void OnGucPlanlamaRaporuClick(object sender, RoutedEventArgs e)
        {
            var grouplar = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.Group).ToList();
            var internalConnectList = new List<ConnectViewModel>();
            var externalConnectList = new List<ConnectViewModel>();

            var gucTuketiciler = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.UcBirim || x.TypeId == (int)TipEnum.AgAnahtari).ToList();
            var gucUreticiler = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.GucUretici).ToList();

            foreach (var group in grouplar)
            {
                var nodeList = this.ViewModel.NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.NodeList).FirstOrDefault();
                var temp = this.ViewModel.NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.InternalConnectList).FirstOrDefault();
                var temp2 = this.ViewModel.NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.ExternalConnectList).FirstOrDefault();

                if (nodeList != null)
                {
                    foreach (var node in nodeList)
                    {
                        if (node.TypeId != (int)TipEnum.GucUretici)
                        {
                            gucTuketiciler.Add(node);
                        }
                        else
                        {
                            gucUreticiler.Add(node);
                        }
                    }
                }

                if (temp != null)
                {
                    foreach (var item in temp)
                    {
                        internalConnectList.Add(item);
                    }
                }

                if (temp2 != null)
                {
                    foreach (var item in temp2)
                    {
                        externalConnectList.Add(item);
                    }
                }
            }

            if (gucUreticiler.Count > 0 || gucTuketiciler.Count > 0)
            {
                RaporBilgileriPopupWindow popup = new RaporBilgileriPopupWindow("Güç Planlama Raporu", CaptureNodesCanvas());
                popup.Owner = this;
                popup.ShowDialog();
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Güç planlama alanında bir çalışma yapılmadığından rapor alınamamaktadır.";
                nfp.Owner = this;
                nfp.Show();
            }
        }

        private byte[] CaptureNodesCanvas()
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(NodesCanvas);
            RenderTargetBitmap rtb = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(NodesCanvas);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }

            rtb.Render(dv);
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);

            return stream.ToArray();
        }
        #endregion

        #region Import/ExportEvents
        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;
            ExportLibraryPopupWindow popup = new ExportLibraryPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();

        }
        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;
            ImportLibraryPopupWindow popup = new ImportLibraryPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }
        #endregion

        #region DogrulamaPaneliEvents
        private void OpenAgAkisPanel(object sender, MouseButtonEventArgs e)
        {
            var context = (sender as Label).DataContext;
            var obj = (context as DogrulamaModel);

            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            if (obj.MesajTipi == "AgAkis")
            {
                if (obj.Connector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                {
                    AgAnahtariAgAkisPopupWindow popup = new AgAnahtariAgAkisPopupWindow(obj.Connector);
                    popup.Owner = this;
                    popup.ShowDialog();
                }
                else if (obj.Connector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                {
                    UcBirimAgAkisPopupWindow popup = new UcBirimAgAkisPopupWindow(obj.Connector);
                    popup.Owner = this;
                    popup.ShowDialog();
                }
            }
        }
        #endregion
    }

}
