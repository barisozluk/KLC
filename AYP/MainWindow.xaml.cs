using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using AYP.ViewModel;
using AYP.Helpers.Enums;
using System.IO;
using System.Linq;
using AYP.DbContext.AYP.DbContexts;
using AYP.Interfaces;
using AYP.Services;
using AYP.Entities;
using AYP.Enums;
using Microsoft.Win32;
using System.Collections.Generic;

namespace AYP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        public bool toggleRight = true;
        public bool toggleLeft = true;
        public bool isClose = false;
        public int midColSize ;

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
            ViewModel = new MainWindowViewModel(this.NodesCanvas.ViewModel);
            ViewModel.NodesCanvas.mainWindow = this;
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
                //this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZoomOriginalSize, x => x.ButtonZoomOriginalSize).DisposeWith(disposable);
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
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandAlignLeft, x => x.ButtonAlignLeft).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandAlignRight, x => x.ButtonAlignRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandAlignCenter, x => x.ButtonAlignCenter).DisposeWith(disposable);



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
                //this.ButtonStartSelect.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonStartSelect, NodeCanvasClickMode.Select, e)).DisposeWith(disposable);
                //this.ButtonStartCut.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonStartCut, NodeCanvasClickMode.Cut, e)).DisposeWith(disposable);
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
            this.Effect = new System.Windows.Media.Effects.BlurEffect();
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
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            UcBirimTurPopupWindow popup = new UcBirimTurPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonUcBirim_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            UcBirimPopupWindow popup = new UcBirimPopupWindow(null);
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonUcBirimAgArayuzu_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            UcBirimAgArayuzuWindow popup = new UcBirimAgArayuzuWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonUcBirimGucArayuz_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            UcBirimGucArayuzuPopupWindow popup = new UcBirimGucArayuzuPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        #endregion

        #region AgAnahtariPopupEvents
        private void ButtonAgAnahtariTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            AgAnahtariTurPopupWindow popup = new AgAnahtariTurPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonAgAnahtari_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            AgAnahtariPopupWindow popup = new AgAnahtariPopupWindow(null);
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonAgAnahtariAgArayuzu_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            AgAnahtariAgArayuzuPopupWindow popup = new AgAnahtariAgArayuzuPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonAgAnahtariGucArayuz_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            AgAnahtariGucArayuzuPopupWindow popup = new AgAnahtariGucArayuzuPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        #endregion

        #region GucUreticiPopupEvents
        private void ButtonGucUreticiTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            GucUreticiTurPopupWindow popup = new GucUreticiTurPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonGucUretici_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            GucUreticiPopupWindow popup = new GucUreticiPopupWindow(null);
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void ButtonGucUreticiGucArayuz_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            GucUreticiGucArayuzuPopupWindow popup = new GucUreticiGucArayuzuPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }

        #endregion

        #region SettingsPopupEvents
        private void Versiyon_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

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
            if(selectedTipId == (int)TipEnum.UcBirim)
            {
                this.IsEnabled = false;
                this.Effect = new System.Windows.Media.Effects.BlurEffect();

                UcBirimPopupWindow popup = new UcBirimPopupWindow(selectedUcBirim);
                popup.Owner = this;
                popup.ShowDialog();
            }
            else if(selectedTipId == (int)TipEnum.AgAnahtari)
            {
                this.IsEnabled = false;
                this.Effect = new System.Windows.Media.Effects.BlurEffect();

                AgAnahtariPopupWindow popup = new AgAnahtariPopupWindow(selectedAgAnahtari);
                popup.Owner = this;
                popup.ShowDialog();
            }
            else if(selectedTipId == (int)TipEnum.GucUretici)
            {
                this.IsEnabled = false;
                this.Effect = new System.Windows.Media.Effects.BlurEffect();

                GucUreticiPopupWindow popup = new GucUreticiPopupWindow(selectedGucUretici);
                popup.Owner = this;
                popup.ShowDialog();
            }
        }

        #endregion

        #region CloseAppPopupEvent
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();

            CloseAppPopupWindow popup = new CloseAppPopupWindow();
            popup.Owner = this;
            popup.ShowDialog();
        }
        #endregion
    }
}
