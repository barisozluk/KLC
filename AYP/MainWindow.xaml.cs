using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using ReactiveUI;
using System.Windows.Markup;
using AYP.ViewModel;
using AYP.Helpers.Enums;
using System.IO;
using System.Linq;
using AYP.Helpers.Extensions;

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
        public string version = "0.0.5";
        public Window darkwindow;

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

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel(this.NodesCanvas.ViewModel);
            SetupSubscriptions();
            SetupBinding();
            SetupEvents();
            //enabled();
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

        private void enabled()
        {
            darkwindow = new Window();
            darkwindow.Background = Brushes.Black;
            darkwindow.Opacity = 0.9;
            darkwindow.AllowsTransparency = true;
            darkwindow.WindowStyle = WindowStyle.None;
            darkwindow.WindowState = WindowState.Maximized;
            darkwindow.Topmost = true;
        }

        private void ButtonDescribing_Click(object sender, RoutedEventArgs e)
        {
            //darkwindow.Show();
            
            this.DescribingMenuPopup.IsOpen = true;
            this.IsEnabled = false;
            this.Effect = new System.Windows.Media.Effects.BlurEffect();
        }

        private void ButtonDescribingPopupClose_Click(object sender, RoutedEventArgs e)
        {
            //darkwindow.Hide();
            this.DescribingMenuPopup.IsOpen = false;
            IsEnabled = true;
            this.Effect = null;// new System.Windows.Media.Effects.BlurEffect();
        }

        private void ButtonUcBirimTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.UcBirimTurPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonUcBirimTurPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.UcBirimTurPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void ButtonUcBirim_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.UcBirimPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonUcBirimPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.UcBirimPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void ButtonUcBirimAgArayuzu_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.UcBirimAgArayuzuPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonUcBirimAgArayuzuPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.UcBirimAgArayuzuPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void ButtonAgAnahtariTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.AgAnahtariTurPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonAgAnahtariTurPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.AgAnahtariTurPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null; 
        }

        private void ButtonAgAnahtari_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.AgAnahtariPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonAgAnahtariPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.AgAnahtariPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void ButtonAgAnahtariAgArayuzu_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.AgAnahtariAgArayuzuPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonAgAnahtariAgArayuzuPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.AgAnahtariAgArayuzuPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void ButtonGucUreticiTur_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.GucUreticiTurPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonGucUreticiTurPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.GucUreticiTurPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void ButtonGucUretici_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.GucUreticiPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonGucUreticiPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.GucUreticiPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void ButtonGucUreticiGucArayuz_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.GucUreticiArayuzPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonGucUreticiArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.GucUreticiArayuzPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void GucUreticiGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string kullanimAmaci = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (kullanimAmaci == "Çıktı")
            {
                g1.IsEnabled = false; g1.Opacity = 0.25;
                g2.IsEnabled = false; g2.Opacity = 0.25;
                g3.IsEnabled = false; g3.Opacity = 0.25;
                g4.IsEnabled = false; g4.Opacity = 0.25;
                g5.IsEnabled = false; g5.Opacity = 0.25;
                g6.IsEnabled = false; g6.Opacity = 0.25;
                g7.IsEnabled = false; g7.Opacity = 0.25;
                g8.IsEnabled = false; g8.Opacity = 0.25;
                g9.IsEnabled = false; g9.Opacity = 0.25;
                g10.IsEnabled = false; g10.Opacity = 0.25;
                g11.IsEnabled = false; g11.Opacity = 0.25;
                g12.IsEnabled = false; g12.Opacity = 0.25;

                g13.IsEnabled = true; g13.Opacity = 1;
                g14.IsEnabled = true; g14.Opacity = 1;
                g15.IsEnabled = true; g15.Opacity = 1;
                g16.IsEnabled = true; g16.Opacity = 1;
            }
            else
            {
                g1.IsEnabled = true; g1.Opacity = 1;
                g2.IsEnabled = true; g2.Opacity = 1;
                g3.IsEnabled = true; g3.Opacity = 1;
                g4.IsEnabled = true; g4.Opacity = 1;
                g5.IsEnabled = true; g5.Opacity = 1;
                g6.IsEnabled = true; g6.Opacity = 1;
                g7.IsEnabled = true; g7.Opacity = 1;
                g8.IsEnabled = true; g8.Opacity = 1;
                g9.IsEnabled = true; g9.Opacity = 1;
                g10.IsEnabled = true; g10.Opacity = 1;
                g11.IsEnabled = true; g11.Opacity = 1;
                g12.IsEnabled = true; g12.Opacity = 1;

                g13.IsEnabled = false; g13.Opacity = 0.25;
                g14.IsEnabled = false; g14.Opacity = 0.25;
                g15.IsEnabled = false; g15.Opacity = 0.25;
                g16.IsEnabled = false; g16.Opacity = 0.25;
            }
        }

        private void ButtonAgAnahtariGucArayuz_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.AgAnahtariGucArayuzPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonAgAnahtariGucArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.AgAnahtariGucArayuzPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }

        private void AgAnahtariGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string kullanimAmaci = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (kullanimAmaci == "Çıktı")
            {
                ag1.IsEnabled = false; ag1.Opacity = 0.25;
                ag2.IsEnabled = false; ag2.Opacity = 0.25;
                ag3.IsEnabled = false; ag3.Opacity = 0.25;
                ag4.IsEnabled = false; ag4.Opacity = 0.25;
                ag5.IsEnabled = false; ag5.Opacity = 0.25;
                ag6.IsEnabled = false; ag6.Opacity = 0.25;
                ag7.IsEnabled = false; ag7.Opacity = 0.25;
                ag8.IsEnabled = false; ag8.Opacity = 0.25;
                ag9.IsEnabled = false; ag9.Opacity = 0.25;
                ag10.IsEnabled = false; ag10.Opacity = 0.25;
                ag11.IsEnabled = false; ag11.Opacity = 0.25;
                ag12.IsEnabled = false; ag12.Opacity = 0.25;

                ag13.IsEnabled = true; ag13.Opacity = 1;
                ag14.IsEnabled = true; ag14.Opacity = 1;
                ag15.IsEnabled = true; ag15.Opacity = 1;
                ag16.IsEnabled = true; ag16.Opacity = 1;
            }
            else
            {
                ag1.IsEnabled = true; ag1.Opacity = 1;
                ag2.IsEnabled = true; ag2.Opacity = 1;
                ag3.IsEnabled = true; ag3.Opacity = 1;
                ag4.IsEnabled = true; ag4.Opacity = 1;
                ag5.IsEnabled = true; ag5.Opacity = 1;
                ag6.IsEnabled = true; ag6.Opacity = 1;
                ag7.IsEnabled = true; ag7.Opacity = 1;
                ag8.IsEnabled = true; ag8.Opacity = 1;
                ag9.IsEnabled = true; ag9.Opacity = 1;
                ag10.IsEnabled = true; ag10.Opacity = 1;
                ag11.IsEnabled = true; ag11.Opacity = 1;
                ag12.IsEnabled = true; ag12.Opacity = 1;

                ag13.IsEnabled = false; ag13.Opacity = 0.25;
                ag14.IsEnabled = false; ag14.Opacity = 0.25;
                ag15.IsEnabled = false; ag15.Opacity = 0.25;
                ag16.IsEnabled = false; ag16.Opacity = 0.25;
            }
        }
        private void ButtonUcBirimGucArayuz_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingMenuPopup.IsOpen = false;
            this.AgAnahtariGucArayuzPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void ButtonUcBirimGucArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.AgAnahtariGucArayuzPopup.IsOpen = false;
            this.IsEnabled = true;
            this.Effect = null;
        }
        private void UcBirimGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string kullanimAmaci = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (kullanimAmaci == "Çıktı")
            {
                ug1.IsEnabled = false; ug1.Opacity = 0.25;
                ug2.IsEnabled = false; ug2.Opacity = 0.25;
                ug3.IsEnabled = false; ug3.Opacity = 0.25;
                ug4.IsEnabled = false; ug4.Opacity = 0.25;
                ug5.IsEnabled = false; ug5.Opacity = 0.25;
                ug6.IsEnabled = false; ug6.Opacity = 0.25;
                ug7.IsEnabled = false; ug7.Opacity = 0.25;
                ug8.IsEnabled = false; ug8.Opacity = 0.25;
                ug9.IsEnabled = false; ug9.Opacity = 0.25;
                ug10.IsEnabled = false; ug10.Opacity = 0.25;
                ug11.IsEnabled = false; ug11.Opacity = 0.25;
                ug12.IsEnabled = false; ug12.Opacity = 0.25;

                ug13.IsEnabled = true; ug13.Opacity = 1;
                ug14.IsEnabled = true; ug14.Opacity = 1;
                ug15.IsEnabled = true; ug15.Opacity = 1;
                ug16.IsEnabled = true; ug16.Opacity = 1;
            }
            else
            {
                ug1.IsEnabled = true; ug1.Opacity = 1;
                ug2.IsEnabled = true; ug2.Opacity = 1;
                ug3.IsEnabled = true; ug3.Opacity = 1;
                ug4.IsEnabled = true; ug4.Opacity = 1;
                ug5.IsEnabled = true; ug5.Opacity = 1;
                ug6.IsEnabled = true; ug6.Opacity = 1;
                ug7.IsEnabled = true; ug7.Opacity = 1;
                ug8.IsEnabled = true; ug8.Opacity = 1;
                ug9.IsEnabled = true; ug9.Opacity = 1;
                ug10.IsEnabled = true; ug10.Opacity = 1;
                ug11.IsEnabled = true; ug11.Opacity = 1;
                ug12.IsEnabled = true; ug12.Opacity = 1;

                ug13.IsEnabled = false; ug13.Opacity = 0.25;
                ug14.IsEnabled = false; ug14.Opacity = 0.25;
                ug15.IsEnabled = false; ug15.Opacity = 0.25;
                ug16.IsEnabled = false; ug16.Opacity = 0.25;
            }
        }

        #region CloseAppEvent
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingClosePopup.IsOpen = true;
            this.IsEnabled = false;

        }

        private void ButtonClosePopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.DescribingClosePopup.IsOpen = false;
            IsEnabled = true;
        }

        private void CloseConfirm(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CloseDeny(object sender, RoutedEventArgs e)
        {
            this.DescribingClosePopup.IsOpen = false;
            IsEnabled = true;
        }

        private void Versiyon_Click(object sender, RoutedEventArgs e)
        {
            this.VersiyonPopup.IsOpen = true;
            this.IsEnabled = false;
        }

        private void VersiyonPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.VersiyonPopup.IsOpen = false;
            IsEnabled = true;
        }





        #endregion

        #region MinimizeAppEvent
        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        #endregion

    }
}
