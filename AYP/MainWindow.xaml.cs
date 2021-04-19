using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Enums;
using AYP.Interfaces;
using AYP.Services;
using AYP.View;
using AYP.ViewModel;
using AYP.ViewModel.Node;
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

namespace AYP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        private bool agWorkspaceSeciliMi = true;
        private bool gucWorkspaceSeciliMi = true;

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

            GucUreticiPopupWindow popup = new GucUreticiPopupWindow(null, fromNode);
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

                GucUreticiPopupWindow popup = new GucUreticiPopupWindow(selectedGucUretici, fromNode);
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
                Point droppedpoint = e.GetPosition(sender as NodesCanvas);
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
                        nfp.msg.Text = "Şu an Güç Planlama üzerine çalışmaktasınız, Uç Birim veya Ağ Anahtarı ekleyemezsiniz.";
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
                        nfp.msg.Text = "Şu an Ağ Planlama üzerine çalışmaktasınız, Uç Birim veya Ağ Anahtarı ekleyemezsiniz.";
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

                foreach (var node in nodes)
                {
                    if (node.TypeId == (int)TipEnum.GucUretici)
                    {
                        //Node visible true
                        foreach (var connect in node.NodesCanvas.Connects.ToList())
                        {
                            connect.IsVisible = true;
                        }
                        node.IsVisible = true;
                    }
                }
            }
        }

        private void HideGucComponents()
        {
            if (this.ViewModel != null)
            {
                var nodes = this.ViewModel.NodesCanvas.Nodes.Items;

                foreach (var node in nodes)
                {
                    if (node.TypeId == (int)TipEnum.GucUretici)
                    {
                        //Node visible false
                        foreach (var connect in node.NodesCanvas.Connects.ToList())
                        {
                            connect.IsVisible = false;
                        }
                        node.IsVisible = false;
                    }
                }
            }
        }

        private void ShowAgComponents()
        {
            if (this.ViewModel != null)
            {
                var nodes = this.ViewModel.NodesCanvas.Nodes.Items;

                foreach (var node in nodes)
                {
                    if (node.TypeId != (int)TipEnum.GucUretici)
                    {
                        //Node visible true
                        foreach (var connect in node.NodesCanvas.Connects.ToList())
                        {
                            connect.IsVisible = true;
                        }
                        node.IsVisible = true;
                    }
                }
            }
        }

        private void HideAgComponents()
        {
            if (this.ViewModel != null)
            {
                var nodes = this.ViewModel.NodesCanvas.Nodes.Items;

                foreach (var node in nodes)
                {
                    if (node.TypeId != (int)TipEnum.GucUretici)
                    {
                        //Node visible false

                        foreach (var connect in node.NodesCanvas.Connects.ToList())
                        {
                            connect.IsVisible = false;
                        }
                        node.IsVisible = false;
                    }
                }
            }
        }
        #endregion

        #region ProjectHierarchy
        public void ProjectHierarchyAdd(TreeViewItem AddedItem)
        {
            //Style style = Application.Current.FindResource("StyleProjectHierarchy") as Style;
            //ProjeHiyerarsi.ItemContainerStyle= style;
            //ProjeHiyerarsi.Style = style;
            ProjeHiyerarsi.ItemsSource = null;
            ProjeHiyerarsi.Items.Add(AddedItem);
        }

        public void ProjectHierarchyDeleteWithRedo(NodeViewModel DeletedItem)
        {
            //Style style = Application.Current.FindResource("StyleProjectHierarchy") as Style;
            //ProjeHiyerarsi.ItemContainerStyle= style;
            //ProjeHiyerarsi.Style = style;
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

        public void ProjectHierarchyAddChild(ConnectorViewModel connectorViewModel)
        {
            List<TreeViewItem> treeList = new List<TreeViewItem>();
            ProjeHiyerarsi.ItemsSource = null;
            string rootNode = connectorViewModel.Connect.FromConnector.Node.Name;
            string toNode = connectorViewModel.Connect.ToConnector.Node.Name;

            foreach (TreeViewItem searchfordelete in ProjeHiyerarsi.Items)
            {
                treeList.Add(searchfordelete);
                //treeList.Remove(toNode);
            }
            foreach (TreeViewItem searchfordelete2 in treeList)
            {
                //parameter.Connect.ToConnector.Node.Name;
                if (searchfordelete2.Header.ToString() == rootNode)
                {
                    searchfordelete2.Items.Add(toNode);
                    ProjectHierarchyDeleteForChild(toNode);

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
            //this.IsEnabled = false;
            //System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            //blur.Radius = 2;
            //this.Effect = blur;

            //HelpPopupWindow popup = new HelpPopupWindow();
            //popup.Owner = this;
            //popup.ShowDialog();

            //WebBrowser webBrowser = new WebBrowser();
            //string pdfPath = "file:///" + Directory.GetCurrentDirectory() + "\\KullanimKilavuzu.pdf";
            //webBrowser.Navigate(pdfPath);

            var pdfOpenProcess = new System.Diagnostics.Process();
            string pdfPath = Directory.GetCurrentDirectory() + "\\SEMA-AYP-KK_v1.0_Kullanım Kılavuzu(001).pdf";
            pdfOpenProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(pdfPath)
            {
                UseShellExecute = true
            };
            pdfOpenProcess.Start();
            //pdfOpenProcess.WaitForExit();
        }
        #endregion

        #region Raporlama
        private void OnAgPlanlamaRaporuClick(object sender, RoutedEventArgs e)
        {
            var agPlanlamaNodes = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId != (int)TipEnum.GucUretici);
            BaseFont arial = BaseFont.CreateFont("C:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font font = new Font(arial, 12, Font.NORMAL);
            if (agPlanlamaNodes.Count() > 0)
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string path = fbd.SelectedPath;

                        var ucBirimler = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.UcBirim).ToList();
                        var agAnahtarlari = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtari).ToList();
                        var kenarAgAnahtarlari = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TurAd == "Kenar").ToList();
                        var toplamaAgAnahtarlari = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TurAd == "Toplama").ToList();
                        var omurgaAgAnahtarlari = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TurAd == "Omurga").ToList();

                        //var kenarAnahtariSayisi = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtari).ToList().Where(kn => kn.)

                        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 0f, 0f, 0f, 0f);
                        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(path + "\\Ağ Planlama Raporu.pdf", FileMode.Create));
                        doc.Open();
                        string mainH = "AYP AĞ PLANLAMA RAPORU";
                        mainH = TurkceKarakter(mainH);
                        Paragraph mainHeader = new Paragraph(mainH, font);
                        mainHeader.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                        mainHeader.SpacingAfter = 30;
                        doc.Add(mainHeader);

                        if (ucBirimler.Count > 0)
                        {
                            string ucBirimH = "Uç Birimler Listesi";
                            ucBirimH = TurkceKarakter(ucBirimH);
                            PdfPCell headerUcBirim = new PdfPCell(new Phrase(ucBirimH, font));
                            headerUcBirim.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            headerUcBirim.BorderColor = new BaseColor(34, 45, 53);
                            PdfPTable tableUcBirimHeader = new PdfPTable(1);
                            tableUcBirimHeader.AddCell(headerUcBirim);
                            PdfPTable tableUcBirim = new PdfPTable(2);
                            string ucBirimAdi = "Adı";
                            string ucBirimTanimi = "Tanımı";
                            ucBirimAdi = TurkceKarakter(ucBirimAdi);
                            ucBirimTanimi = TurkceKarakter(ucBirimTanimi);
                            PdfPCell ucbirimAdBaslik = new PdfPCell(new Phrase(ucBirimAdi, font));
                            PdfPCell ucBirimTanimBaslik = new PdfPCell(new Phrase(ucBirimTanimi, font));
                            tableUcBirim.AddCell(ucbirimAdBaslik);
                            tableUcBirim.AddCell(ucBirimTanimBaslik);

                            for (int i = 0; i < ucBirimler.Count; i++)
                            {

                                string ucBirimName = ucBirimler[i].Name;
                                ucBirimName = TurkceKarakter(ucBirimName);
                                string ucBirimTanim = ucBirimler[i].Tanim;
                                ucBirimTanim = TurkceKarakter(ucBirimTanim);
                                PdfPCell cell = new PdfPCell(new Phrase(ucBirimName, font));
                                PdfPCell cell2 = new PdfPCell(new Phrase(ucBirimTanim, font));
                                cell.BorderColor = new BaseColor(34, 45, 53);
                                tableUcBirim.AddCell(cell);
                                tableUcBirim.AddCell(cell2);

                            }
                            PdfPTable tableToplamUcBirim = new PdfPTable(2);
                            PdfPCell cellToplam = new PdfPCell(new Phrase("Toplam"));
                            cellToplam.BorderColor = new BaseColor(34, 45, 53);
                            PdfPCell cellDeger = new PdfPCell(new Phrase(ucBirimler.Count.ToString()));
                            cellDeger.BorderColor = new BaseColor(34, 45, 53);

                            tableToplamUcBirim.AddCell(cellToplam);
                            tableToplamUcBirim.AddCell(cellDeger);
                            tableToplamUcBirim.SpacingAfter = 20;
                            doc.Add(tableUcBirimHeader);
                            doc.Add(tableUcBirim);
                            doc.Add(tableToplamUcBirim);
                        }

                        if (agAnahtarlari.Count > 0)
                        {
                            PdfPCell headerAgAnahtari = new PdfPCell(new Phrase("Ag Anahtarı Listesi"));
                            headerAgAnahtari.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            headerAgAnahtari.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            headerAgAnahtari.BorderColor = new BaseColor(34, 45, 53);
                            PdfPTable tableAgAnahtariHeader = new PdfPTable(1);

                            tableAgAnahtariHeader.AddCell(headerAgAnahtari);
                            string Adi = "Adı";
                            string Turu = "Türü";
                            Adi = TurkceKarakter(Adi);
                            Turu = TurkceKarakter(Turu);
                           
                            
                            PdfPTable tableAgAnahtari = new PdfPTable(2);
                            PdfPCell birinci = new PdfPCell(new Phrase(Adi, font));
                            PdfPCell ikinci = new PdfPCell(new Phrase(Turu, font));
                            tableAgAnahtari.AddCell(birinci);
                            tableAgAnahtari.AddCell(ikinci);

                            for (int i = 0; i < agAnahtarlari.Count; i++)
                            {
                                string agAdi = agAnahtarlari[i].Name;
                                string agTuru = agAnahtarlari[i].TurAd;
                                Adi = TurkceKarakter(Adi);
                                Turu = TurkceKarakter(Turu);
                                PdfPCell cell = new PdfPCell(new Phrase(agAdi, font));
                                PdfPCell cell2 = new PdfPCell(new Phrase(agTuru, font));
                                cell.BorderColor = new BaseColor(34, 45, 53);
                                tableAgAnahtari.AddCell(cell);
                                tableAgAnahtari.AddCell(cell2);
                            }
                            PdfPTable tableToplamAgAnahtari = new PdfPTable(2);
                            PdfPCell cellToplam = new PdfPCell(new Phrase("Toplam"));
                            cellToplam.BorderColor = new BaseColor(34, 45, 53);
                            PdfPCell cellDeger = new PdfPCell(new Phrase(agAnahtarlari.Count.ToString()));
                            cellDeger.BorderColor = new BaseColor(34, 45, 53);

                            tableToplamAgAnahtari.AddCell(cellToplam);
                            tableToplamAgAnahtari.AddCell(cellDeger);
                            tableToplamAgAnahtari.SpacingAfter = 20;

                            doc.Add(tableAgAnahtariHeader);
                            doc.Add(tableAgAnahtari);
                            doc.Add(tableToplamAgAnahtari);
                        }

                        if (ucBirimler.Count > 0 && agAnahtarlari.Count > 0)
                        {
                            string baslik = "Kenar Ağ Anahtarı ve Bağlı Olduğu Uç Birimler Listesi";
                            baslik = TurkceKarakter(baslik);
                            PdfPCell header = new PdfPCell(new Phrase(baslik,font));
                            header.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            PdfPTable tableHeader = new PdfPTable(1);
                            tableHeader.AddCell(header);

                            string agAdi = "Ağ Anahtarı Adı";
                            string ucAdi = "Uç Birim Adı";
                            agAdi = TurkceKarakter(agAdi);
                            ucAdi = TurkceKarakter(ucAdi);
                            PdfPTable tableKenarUcBirim = new PdfPTable(3);
                            PdfPCell header1 = new PdfPCell(new Phrase(agAdi, font));
                            PdfPCell header2 = new PdfPCell(new Phrase(ucAdi, font));
                            PdfPCell header3 = new PdfPCell(new Phrase("Yük Değeri",font));
                            
                            tableKenarUcBirim.AddCell(header1);
                            tableKenarUcBirim.AddCell(header2);
                            tableKenarUcBirim.AddCell(header3);
                            

                            foreach (var connect in this.ViewModel.NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                            {
                                if(kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                                {
                                    if(ucBirimler.Contains(connect.FromConnector.Node))
                                    {
                                            string agAdiNode = connect.ToConnector.Node.Name;
                                            string ucAdiNode = connect.FromConnector.Node.Name;
                                            agAdiNode = TurkceKarakter(agAdiNode);
                                            ucAdiNode = TurkceKarakter(ucAdiNode);
                                            PdfPCell ag = new PdfPCell(new Phrase(agAdiNode, font));
                                            PdfPCell uc = new PdfPCell(new Phrase(ucAdiNode, font));
                                            PdfPCell yuk = new PdfPCell(new Phrase(connect.AgYuku.ToString()));

                                            tableKenarUcBirim.AddCell(ag);
                                            tableKenarUcBirim.AddCell(uc);
                                            tableKenarUcBirim.AddCell(yuk);
                                    }
                                }
                                if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                                {
                                    if (ucBirimler.Contains(connect.ToConnector.Node))
                                    {
                                        string agAdiNode = connect.FromConnector.Node.Name;
                                        string ucAdiNode = connect.ToConnector.Node.Name;
                                        agAdiNode = TurkceKarakter(agAdiNode);
                                        ucAdiNode = TurkceKarakter(ucAdiNode);
                                        PdfPCell ag = new PdfPCell(new Phrase(agAdiNode, font));
                                        PdfPCell uc = new PdfPCell(new Phrase(ucAdiNode, font));
                                        PdfPCell yuk = new PdfPCell(new Phrase(connect.AgYuku.ToString()));

                                        tableKenarUcBirim.AddCell(ag);
                                        tableKenarUcBirim.AddCell(uc);
                                        tableKenarUcBirim.AddCell(yuk);
                                    }
                                }
                            }

                            tableKenarUcBirim.SpacingAfter = 20;
                            doc.Add(tableHeader);
                            
                            doc.Add(tableKenarUcBirim);
                        }

                        if(toplamaAgAnahtarlari.Count > 0)
                        {
                            string baslik = "Toplama Ağ Anahtarı ve Bağlı Olduğu Kenar Anahtarları Listesi";
                            baslik = TurkceKarakter(baslik);
                            PdfPCell header = new PdfPCell(new Phrase(baslik, font));
                            header.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            PdfPTable tableHeader = new PdfPTable(1);
                            tableHeader.AddCell(header);

                            PdfPTable tableToplamaKenar = new PdfPTable(2);
                            string h1 = "Kenar Ağ Anahtarı Adı";
                            string h2 = "Toplama Ağ Anahtarı Adı";
                            h1 = TurkceKarakter(h1);
                            h2 = TurkceKarakter(h2);
                            PdfPCell header1 = new PdfPCell(new Phrase(h1, font));
                            PdfPCell header2 = new PdfPCell(new Phrase(h2, font));

                            tableToplamaKenar.AddCell(header1);
                            tableToplamaKenar.AddCell(header2);


                            foreach (var connect in this.ViewModel.NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                            {
                                if (toplamaAgAnahtarlari.Contains(connect.ToConnector.Node))
                                {
                                    if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                                    {
                                        string kenarNode = connect.ToConnector.Node.Name;
                                        string toplamaNode = connect.FromConnector.Node.Name;
                                        kenarNode = TurkceKarakter(kenarNode);
                                        toplamaNode = TurkceKarakter(toplamaNode);

                                        PdfPCell kenar = new PdfPCell(new Phrase(kenarNode, font));
                                        PdfPCell toplama = new PdfPCell(new Phrase(toplamaNode, font));

                                        tableToplamaKenar.AddCell(toplama);
                                        tableToplamaKenar.AddCell(kenar);
                                    }
                                }
                                if (toplamaAgAnahtarlari.Contains(connect.FromConnector.Node))
                                {
                                    if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                                    {
                                        string kenarNode = connect.FromConnector.Node.Name;
                                        string toplamaNode = connect.ToConnector.Node.Name;
                                        kenarNode = TurkceKarakter(kenarNode);
                                        toplamaNode = TurkceKarakter(toplamaNode);

                                        PdfPCell kenar = new PdfPCell(new Phrase(kenarNode, font));
                                        PdfPCell toplama = new PdfPCell(new Phrase(toplamaNode, font));

                                        tableToplamaKenar.AddCell(toplama);
                                        tableToplamaKenar.AddCell(kenar);
                                    }
                                }


                            }
                            tableToplamaKenar.SpacingAfter = 20;
                            doc.Add(tableHeader);

                            doc.Add(tableToplamaKenar);
                        }

                        if (omurgaAgAnahtarlari.Count > 0)
                        {
                            string baslik = "Omurga Ağ Anahtarı ve bağlı olduğu Ağ anahtarları Listesi";
                            baslik = TurkceKarakter(baslik);
                            PdfPCell header = new PdfPCell(new Phrase(baslik, font));
                            header.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            PdfPTable tableHeader = new PdfPTable(1);
                            tableHeader.AddCell(header);

                            string h1 = "Omurga Ağ Anahtarı Adı";
                            string h2 = "Bağlı Olduğu Anahtarı Adı";
                            string h3 = "Bağlı Olduğu anahtar Tipi";
                            h1 = TurkceKarakter(h1);
                            h2 = TurkceKarakter(h2);
                            h3 = TurkceKarakter(h3);

                            PdfPTable tableOmurga = new PdfPTable(3);
                            PdfPCell header1 = new PdfPCell(new Phrase(h1,font));
                            PdfPCell header2 = new PdfPCell(new Phrase(h2, font));
                            PdfPCell header3 = new PdfPCell(new Phrase(h3, font));


                            tableOmurga.AddCell(header1);
                            tableOmurga.AddCell(header2);
                            tableOmurga.AddCell(header3);


                            foreach (var connect in this.ViewModel.NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                            {
                                if (omurgaAgAnahtarlari.Contains(connect.FromConnector.Node))
                                {
                                    if (agAnahtarlari.Contains(connect.ToConnector.Node))
                                    {
                                        string omurgaNode = connect.FromConnector.Node.Name;
                                        string anahtarNode = connect.ToConnector.Node.Name;
                                        string anahtarTuruNode = connect.ToConnector.Node.TurAd;
                                        omurgaNode = TurkceKarakter(omurgaNode);
                                        anahtarNode = TurkceKarakter(anahtarNode);
                                        anahtarTuruNode = TurkceKarakter(anahtarTuruNode);

                                        PdfPCell omurga = new PdfPCell(new Phrase(omurgaNode, font));
                                        PdfPCell anahtar = new PdfPCell(new Phrase(anahtarNode, font));
                                        PdfPCell anahtarTuru = new PdfPCell(new Phrase(anahtarTuruNode, font));

                                        tableOmurga.AddCell(omurga);
                                        tableOmurga.AddCell(anahtar);
                                        tableOmurga.AddCell(anahtarTuru); 
                                    }
                                }
                                if (omurgaAgAnahtarlari.Contains(connect.ToConnector.Node))
                                {
                                    if (agAnahtarlari.Contains(connect.FromConnector.Node))
                                    {
                                        string omurgaNode = connect.ToConnector.Node.Name;
                                        string anahtarNode = connect.FromConnector.Node.Name;
                                        string anahtarTuruNode = connect.FromConnector.Node.TurAd;
                                        omurgaNode = TurkceKarakter(omurgaNode);
                                        anahtarNode = TurkceKarakter(anahtarNode);
                                        anahtarTuruNode = TurkceKarakter(anahtarTuruNode);

                                        PdfPCell omurga = new PdfPCell(new Phrase(omurgaNode, font));
                                        PdfPCell anahtar = new PdfPCell(new Phrase(anahtarNode, font));
                                        PdfPCell anahtarTuru = new PdfPCell(new Phrase(anahtarTuruNode, font));

                                        tableOmurga.AddCell(omurga);
                                        tableOmurga.AddCell(anahtar);
                                        tableOmurga.AddCell(anahtarTuru);
                                    }
                                }
                            }
                            tableOmurga.SpacingAfter = 20;

                            doc.Add(tableHeader);

                            doc.Add(tableOmurga);
                        }

                        doc.Close();
                    }
                }
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
            var gucPlanlamaNodes = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.GucUretici);
            BaseFont arial = BaseFont.CreateFont("C:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(arial, 12, Font.NORMAL);

            //if (gucPlanlamaNodes.Count() > 0)
            //{
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                    string path = fbd.SelectedPath;

                    var ucBirimler = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.UcBirim).ToList();
                    var agAnahtarlari = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtari).ToList();
                    var gucUreticiler = this.ViewModel.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.GucUretici).ToList();

                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 0f, 0f, 0f, 0f);
                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(path + "\\Güç Planlama Raporu.pdf", FileMode.Create));
                    doc.Open();
                    string mainH = "AYP GÜÇ PLANLAMA RAPORU";
                    mainH = TurkceKarakter(mainH);
                    Paragraph mainHeader = new Paragraph(mainH, font);
                    mainHeader.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                    mainHeader.SpacingAfter = 30;
                    doc.Add(mainHeader);

                    if (ucBirimler.Count > 0 || agAnahtarlari.Count > 0)
                    {
                        string gucTuketiciH = "Güç Tüketiciler Listesi";
                        gucTuketiciH = TurkceKarakter(gucTuketiciH);
                        PdfPCell headerGucTuketici = new PdfPCell(new Phrase(gucTuketiciH, font));
                        headerGucTuketici.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        headerGucTuketici.BorderColor = new BaseColor(34, 45, 53);
                        PdfPTable tableGucTuketiciHeader = new PdfPTable(1);
                        tableGucTuketiciHeader.AddCell(headerGucTuketici);
                        PdfPTable tableGucTuketiciler = new PdfPTable(3);

                        string Adi = "Adı";
                        string Turu = "Türü";
                        string Tanimi = "Tanımı";

                        Adi = TurkceKarakter(Adi);
                        Turu = TurkceKarakter(Turu);
                        Tanimi = TurkceKarakter(Tanimi);



                        PdfPTable tableAgAnahtari = new PdfPTable(2);
                        PdfPCell birinci = new PdfPCell(new Phrase(Adi, font));
                        PdfPCell ikinci = new PdfPCell(new Phrase(Tanimi, font));
                        PdfPCell ucuncu = new PdfPCell(new Phrase(Turu, font));

                        tableGucTuketiciler.AddCell(birinci);
                        tableGucTuketiciler.AddCell(ikinci);
                        tableGucTuketiciler.AddCell(ucuncu);

                        for (int i = 0; i < ucBirimler.Count; i++)
                        {

                            string ucBirimName = ucBirimler[i].Name;
                            ucBirimName = TurkceKarakter(ucBirimName);
                            string ucBirimTanim = ucBirimler[i].Tanim;
                            ucBirimTanim = TurkceKarakter(ucBirimTanim);
                            string ucBirimTur = ucBirimler[i].TurAd;
                            ucBirimTur = TurkceKarakter(ucBirimTur);
                            PdfPCell cell = new PdfPCell(new Phrase(ucBirimName, font));
                            PdfPCell cell2 = new PdfPCell(new Phrase(ucBirimTanim, font));
                            PdfPCell cell3 = new PdfPCell(new Phrase(ucBirimTur, font));
                            cell.BorderColor = new BaseColor(34, 45, 53);
                            tableGucTuketiciler.AddCell(cell);
                            tableGucTuketiciler.AddCell(cell2);
                            tableGucTuketiciler.AddCell(cell3);
                        }
                        for(int j=0 ; j<agAnahtarlari.Count; j++)
                        {
                            string agAnahtariName = agAnahtarlari[j].Name;
                            string agAnahtariTanim = agAnahtarlari[j].Tanim;
                            string agAnahtariTur = agAnahtarlari[j].TurAd;
                            agAnahtariName = TurkceKarakter(agAnahtariName);
                            agAnahtariTanim = TurkceKarakter(agAnahtariTanim);
                            agAnahtariTur = TurkceKarakter(agAnahtariTur);
                            PdfPCell cell = new PdfPCell(new Phrase(agAnahtariName, font));
                            PdfPCell cell2 = new PdfPCell(new Phrase(agAnahtariTanim, font));
                            PdfPCell cell3 = new PdfPCell(new Phrase(agAnahtariTur, font));
                            cell.BorderColor = new BaseColor(34, 45, 53);
                            tableGucTuketiciler.AddCell(cell);
                            tableGucTuketiciler.AddCell(cell2);
                            tableGucTuketiciler.AddCell(cell3);
                        }
                        tableGucTuketiciler.SpacingAfter = 20;
                        doc.Add(tableGucTuketiciHeader);
                        doc.Add(tableGucTuketiciler);
                    }

                    if(gucUreticiler.Count > 0)
                    {
                        string gucUreticiH = "Güç Üreticiler Listesi";
                        gucUreticiH = TurkceKarakter(gucUreticiH);
                        PdfPCell headerGucUretici = new PdfPCell(new Phrase(gucUreticiH, font));
                        headerGucUretici.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        headerGucUretici.BorderColor = new BaseColor(34, 45, 53);
                        PdfPTable tableGucUreticiHeader = new PdfPTable(1);
                        tableGucUreticiHeader.AddCell(headerGucUretici);

                        PdfPTable tableGucUreticiler = new PdfPTable(2);
                        string Adi = "Adı";
                        string Turu = "Türü";
                        Adi = TurkceKarakter(Adi);
                        Turu = TurkceKarakter(Turu);


                        PdfPTable tableAgAnahtari = new PdfPTable(2);
                        PdfPCell birinci = new PdfPCell(new Phrase(Adi, font));
                        PdfPCell ikinci = new PdfPCell(new Phrase(Turu, font));
                        tableGucUreticiler.AddCell(birinci);
                        tableGucUreticiler.AddCell(ikinci);

                        for (int i=0;i<gucUreticiler.Count; i++)
                        {
                            string gucUreticiAdi = gucUreticiler[i].Name;
                            string gucUreticiTur = gucUreticiler[i].TurAd;
                            gucUreticiAdi = TurkceKarakter(gucUreticiAdi);
                            gucUreticiTur = TurkceKarakter(gucUreticiTur);
                            PdfPCell cell = new PdfPCell(new Phrase(gucUreticiAdi, font));
                            PdfPCell cell2 = new PdfPCell(new Phrase(gucUreticiTur, font));
                            cell.BorderColor = new BaseColor(34, 45, 53);
                            tableGucUreticiler.AddCell(cell);
                            tableGucUreticiler.AddCell(cell2);
                        }

                        tableGucUreticiler.SpacingAfter = 20;
                        doc.Add(tableGucUreticiHeader);
                        doc.Add(tableGucUreticiler);
                    }

                    if(gucUreticiler.Count > 0)
                    {
                        PdfPCell header = new PdfPCell(new Phrase("Güç Üreticiler ve Bağlı Oldukları Güç Tüketiciler Listesi"));
                        header.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        PdfPTable tableHeader = new PdfPTable(1);
                        tableHeader.AddCell(header);

                        string gucUretici = "Güç Üretici";
                        string gucTuketici = "Güç Tüketici";
                        string gucTuketiciTuru = "Güç Tüketici Türü";
                        string gucTuketiciGucDegeri = "Güç Tüketici Güç Değeri";

                        gucUretici = TurkceKarakter(gucUretici);
                        gucTuketici = TurkceKarakter(gucTuketici);
                        gucTuketiciTuru = TurkceKarakter(gucTuketiciTuru);
                        gucTuketiciGucDegeri = TurkceKarakter(gucTuketiciGucDegeri);

                        PdfPTable tableGucDegeri = new PdfPTable(4);
                        PdfPCell header1 = new PdfPCell(new Phrase(gucUretici, font));
                        PdfPCell header2 = new PdfPCell(new Phrase(gucTuketici, font));
                        PdfPCell header3 = new PdfPCell(new Phrase(gucTuketiciTuru, font));
                        PdfPCell header4 = new PdfPCell(new Phrase(gucTuketiciGucDegeri, font));
                        tableGucDegeri.AddCell(header1);
                        tableGucDegeri.AddCell(header2);
                        tableGucDegeri.AddCell(header3);
                        tableGucDegeri.AddCell(header4);



                        foreach (var connect in this.ViewModel.NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                        {
                            if (gucUreticiler.Contains(connect.FromConnector.Node))
                            {
                                if (ucBirimler.Contains(connect.ToConnector.Node))
                                {
                                    string gucUreticiNode = connect.FromConnector.Node.Name;
                                    string gucTuketiciNode = connect.ToConnector.Node.Name;
                                    string gucTuketiciTuruNode = connect.ToConnector.Node.TurAd;

                                    PdfPCell gu = new PdfPCell(new Phrase(gucUreticiNode, font));
                                    PdfPCell gt = new PdfPCell(new Phrase(gucTuketiciNode, font));
                                    PdfPCell gtt = new PdfPCell(new Phrase(gucTuketiciTuruNode, font));
                                    PdfPCell gd = new PdfPCell(new Phrase("-"));

                                    tableGucDegeri.AddCell(gu);
                                    tableGucDegeri.AddCell(gt);
                                    tableGucDegeri.AddCell(gtt);
                                    tableGucDegeri.AddCell(gd);

                                }

                                if (agAnahtarlari.Contains(connect.ToConnector.Node))
                                {
                                    string gucUreticiNode = connect.FromConnector.Node.Name;
                                    string gucTuketiciNode = connect.ToConnector.Node.Name;
                                    string gucTuketiciTuruNode = connect.ToConnector.Node.TurAd;

                                    PdfPCell gu = new PdfPCell(new Phrase(gucUreticiNode, font));
                                    PdfPCell gt = new PdfPCell(new Phrase(gucTuketiciNode, font));
                                    PdfPCell gtt = new PdfPCell(new Phrase(gucTuketiciTuruNode, font));
                                    PdfPCell gd = new PdfPCell(new Phrase("-"));

                                    tableGucDegeri.AddCell(gu);
                                    tableGucDegeri.AddCell(gt);
                                    tableGucDegeri.AddCell(gtt);
                                    tableGucDegeri.AddCell(gd);
                                }
                            }
                        }

                        tableGucDegeri.SpacingAfter = 20;
                        doc.Add(tableHeader);
                        doc.Add(tableGucDegeri);

                        //tableKenarUcBirim.SpacingAfter = 20;
                        //doc.Add(tableHeader);

                        //doc.Add(tableKenarUcBirim);
                    }
                        
                    doc.Close();
                }
            }
        }
        #endregion
        public string TurkceKarakter(string text)
        {

            text = text.Replace("İ", "\u0130");

            text = text.Replace("ı", "\u0131");

            text = text.Replace("Ş", "\u015e");

            text = text.Replace("ş", "\u015f");

            text = text.Replace("Ğ", "\u011e");

            text = text.Replace("ğ", "\u011f");

            text = text.Replace("Ö", "\u00d6");

            text = text.Replace("ö", "\u00f6");

            text = text.Replace("ç", "\u00e7");

            text = text.Replace("Ç", "\u00c7");

            text = text.Replace("ü", "\u00fc");

            text = text.Replace("Ü", "\u00dc");

            return text;
        }

        #region Import/Export
        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            //List<UcBirim> ucbirimler = new List<UcBirim>();
            //foreach (var ub in ucBirimService.ListUcBirim())
            //{
            //    ucbirimler.Add(ub);// ucBirimService.ListUcBirim();
            //}

            //string json = JsonConvert.SerializeObject(ucbirimler, Formatting.Indented);
            //File.WriteAllText(@"D:\path.json", json);

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
    }
}
