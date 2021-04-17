using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReactiveUI;
using System;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Linq;
using System.Windows.Markup;
using AYP.ViewModel;
using AYP.Helpers;
using AYP.Enums;
using AYP.Helpers.Transformations;
using AYP.Helpers.Enums;

namespace AYP.View
{
    /// <summary>
    /// Interaction logic for ViewConnect.xaml
    /// </summary>
    public partial class Connect : UserControl, IViewFor<ConnectViewModel>, CanBeMove
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ConnectViewModel), typeof(Connect), new PropertyMetadata(null));

        public ConnectViewModel ViewModel
        {
            get { return (ConnectViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ConnectViewModel)value; }
        }
        #endregion ViewModel
        public Connect()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                Canvas.SetZIndex((UIElement)this.VisualParent, 999);

                this.OneWayBind(this.ViewModel, x => x.Stroke, x => x.PathElement.Stroke).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.StartPoint, x => x.PathFigureElement.StartPoint).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point1, x => x.BezierSegmentElement.Point1).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point2, x => x.BezierSegmentElement.Point2).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.EndPoint, x => x.BezierSegmentElement.Point3).DisposeWith(disposable);

                if (this.ViewModel.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || this.ViewModel.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                {
                    if (this.ViewModel.FromConnector.FizikselOrtamId == (int)FizikselOrtamEnum.Bakir)
                    {
                        this.OneWayBind(this.ViewModel, x => x.StrokeDashArrayBakir, x => x.PathElement.StrokeDashArray).DisposeWith(disposable);

                    }
                    else if (this.ViewModel.FromConnector.FizikselOrtamId == (int)FizikselOrtamEnum.FiberOptik)
                    {
                        this.OneWayBind(this.ViewModel, x => x.StrokeDashArrayFiber, x => x.PathElement.StrokeDashArray).DisposeWith(disposable);
                    }
                }
                else
                {
                    this.OneWayBind(this.ViewModel, x => x.StrokeDashArrayGuc, x => x.PathElement.StrokeDashArray).DisposeWith(disposable);
                }

                this.WhenAnyValue(x => x.ViewModel.ToConnector).Where(x=>x!=null).Subscribe(_ => UpdateZindex()).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.FromConnector.TypeId).Where(x => x != (int)TipEnum.AgAnahtariGucArayuzu &&
                                        x != (int)TipEnum.UcBirimGucArayuzu && 
                                        x != (int)TipEnum.GucUreticiGucArayuzu)
                                .Subscribe(_ => AgYukuBorder.Visibility = Visibility.Visible).DisposeWith(disposable);

            });
        }

        private void UpdateZindex()
        {
            int toIndex = this.ViewModel.ToConnector.Node.Zindex;
            int fromIndex = this.ViewModel.FromConnector.Node.Zindex;
           
            Canvas.SetZIndex((UIElement)this.VisualParent, Math.Min(toIndex, fromIndex));
        }
        #endregion SetupBinding

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.ViewModel.WhenAnyValue(x => x.IsVisible).Subscribe(value => OnEventVisible(value)).DisposeWith(disposable);
            });

            this.WhenActivated(disposable =>
            {
                this.ViewModel.WhenAnyValue(x => x.StartPoint).Subscribe(value => OnEventStartPoint(value)).DisposeWith(disposable);
            });

            this.WhenActivated(disposable =>
            {
                this.ViewModel.WhenAnyValue(x => x.EndPoint).Subscribe(value => OnEventEndPoint(value)).DisposeWith(disposable);
            });

            this.WhenActivated(disposable =>
            {
                this.ViewModel.WhenAnyValue(x => x.Uzunluk).Subscribe(value => OnEventUzunluk(value)).DisposeWith(disposable);
            });

            this.WhenActivated(disposable =>
            {
                this.ViewModel.WhenAnyValue(x => x.AgYuku).Subscribe(value => OnEventAgYuku(value)).DisposeWith(disposable);
            });

            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.IsMouseOver).Subscribe(value => OnEventMouseOver(value)).DisposeWith(disposable);
            });

            this.WhenActivated(disposable =>
            {
                this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseClick(e)).DisposeWith(disposable);
            });
        }

        private void OnEventMouseOver(bool value)
        {
           
                if (this.ViewModel.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu || this.ViewModel.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                {
                    if (this.ViewModel.FromConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                    {
                        this.ViewModel.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorEthernet"] as SolidColorBrush;
                    }
                    else if (this.ViewModel.FromConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                    {
                        this.ViewModel.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorFastEthernet"] as SolidColorBrush;
                    }
                    else if (this.ViewModel.FromConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                    {
                        this.ViewModel.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorGigabitEthernet"] as SolidColorBrush;
                    }
                    else if (this.ViewModel.FromConnector.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
                    {
                        this.ViewModel.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnector10GigabitEthernet"] as SolidColorBrush;
                    }
                    else if (this.ViewModel.FromConnector.KapasiteId == (int)KapasiteEnum._40GigabitEthernet)
                    {
                        this.ViewModel.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnector40GigabitEthernet"] as SolidColorBrush;
                    }
                }
                else
                {
                    if (this.ViewModel.FromConnector.GerilimTipiId == (int)GerilimTipiEnum.AC)
                    {
                        this.ViewModel.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorAC"] as SolidColorBrush;
                    }
                    else if (this.ViewModel.FromConnector.GerilimTipiId == (int)GerilimTipiEnum.DC)
                    {
                        this.ViewModel.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorDC"] as SolidColorBrush;
                    }
                }
        }

        private void OnEventMouseClick(MouseButtonEventArgs e)
        {
            if (this.ViewModel.NodesCanvas.ClickMode == NodeCanvasClickMode.Default)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    this.ViewModel.NodesCanvas.MainWindow.IsEnabled = false;
                    System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                    blur.Radius = 2;
                    this.ViewModel.NodesCanvas.MainWindow.Effect = blur;

                    CableLengthPopupWindow cl = new CableLengthPopupWindow(this.ViewModel);
                    cl.Owner = this.ViewModel.NodesCanvas.MainWindow;
                    cl.ShowDialog();
                }
                else
                {
                    if (this.ViewModel.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                    {
                        this.ViewModel.NodesCanvas.MainWindow.IsEnabled = false;
                        System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                        blur.Radius = 2;
                        this.ViewModel.NodesCanvas.MainWindow.Effect = blur;

                        UcBirimAgAkisPopupWindow agAkisiPopup = new UcBirimAgAkisPopupWindow(this.ViewModel.FromConnector);
                        agAkisiPopup.Owner = this.ViewModel.NodesCanvas.MainWindow;
                        agAkisiPopup.ShowDialog();
                    }
                    else if (this.ViewModel.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                    {
                        bool isConnectExist = false;
                        foreach(var input in this.ViewModel.FromConnector.Node.InputList)
                        {
                            foreach (var connect in this.ViewModel.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == input)
                                {
                                    if (connect.ToConnector.AgAkisList.Count > 0)
                                    {
                                        isConnectExist = true;
                                        break;
                                    }
                                }
                            }

                            if (isConnectExist)
                                break;
                        }

                        if (isConnectExist)
                        {
                            this.ViewModel.NodesCanvas.MainWindow.IsEnabled = false;
                            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                            blur.Radius = 2;
                            this.ViewModel.NodesCanvas.MainWindow.Effect = blur;

                            AgAnahtariAgAkisPopupWindow agAkisiPopup = new AgAnahtariAgAkisPopupWindow(this.ViewModel.FromConnector);
                            agAkisiPopup.Owner = this.ViewModel.NodesCanvas.MainWindow;
                            agAkisiPopup.ShowDialog();
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Ağ anahtarı için tanımlanmış bir ağ akışı olmadığıdan bu bağlantı için ağ akışı oluşturulamaz.";
                            nfp.Owner = this.ViewModel.NodesCanvas.MainWindow;
                            nfp.Show();
                        }
                    }
                }
            }

            e.Handled = true;
        }

        private void OnEventVisible(bool isVisible)
        {
            if (!isVisible)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
        }

        private void OnEventStartPoint(Point value)
        {
            double middleX = (value.X + this.ViewModel.EndPoint.X) / 2;
            double middleY = (value.Y + this.ViewModel.EndPoint.Y) / 2;
            UzunlukBorder.Margin = new Thickness(middleX , middleY, 0, 0);
            AgYukuBorder.Margin = new Thickness(value.X + 10, value.Y - 10, 0, 0);
        }

        private void OnEventEndPoint(Point value)
        {
            double middleX = (value.X + this.ViewModel.StartPoint.X) / 2;
            double middleY = (value.Y + this.ViewModel.StartPoint.Y) / 2;
            UzunlukBorder.Margin = new Thickness(middleX, middleY, 0, 0);
            AgYukuBorder.Margin = new Thickness(this.ViewModel.StartPoint.X + 10, this.ViewModel.StartPoint.Y - 10, 0, 0);
        }

        private void OnEventUzunluk(decimal value)
        {
            Uzunluk.Text = value.ToString() + " m";
        }
        private void OnEventAgYuku(decimal value)
        {
            AgYuku.Text = value.ToString() + " mbps";
        }


        #endregion
    }
}
