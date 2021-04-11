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

namespace AYP.View
{
    /// <summary>
    /// Interaction logic for ViewConnect.xaml
    /// </summary>
    public partial class Connect : UserControl, IViewFor<ConnectViewModel>
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
        #endregion
    }
}
