using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using AYP.Helpers;
using AYP.Helpers.Extensions;
using AYP.Enums;

namespace AYP.ViewModel
{
    public class ConnectViewModel : ReactiveObject
    {
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; }
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Point Point2 { get; set; }

        [Reactive] public Brush Stroke { get; set; } = Application.Current.Resources["ColorConnect"] as SolidColorBrush;

        [Reactive] public ConnectorViewModel FromConnector { get; set; }

        [Reactive] public ConnectorViewModel ToConnector { get; set; }

        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }

        [Reactive] public DoubleCollection StrokeDashArrayFiber { get; set; } = new DoubleCollection() { 0, 0 };
        [Reactive] public DoubleCollection StrokeDashArrayBakir { get; set; } = new DoubleCollection() { 10, 3 };
        [Reactive] public DoubleCollection StrokeDashArrayGuc { get; set; } = new DoubleCollection() { 20, 6 };

        [Reactive] public double StrokeThickness { get; set; } = 1;
        [Reactive] public bool IsVisible { get; set; } = true;
        [Reactive] public decimal? Uzunluk { get; set; }


        private IDisposable subscriptionOnConnectorPositionChange;
        private IDisposable subscriptionOnOutputPositionChange;

        public ConnectViewModel(NodesCanvasViewModel viewModelNodesCanvas, ConnectorViewModel fromConnector)
        {
            Initial(viewModelNodesCanvas, fromConnector);
            SetupSubscriptions();     
        }
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.StartPoint, x => x.EndPoint).Subscribe(_ => UpdateMedium());
            this.WhenAnyValue(x => x.FromConnector.Node.IsCollapse).Subscribe(value => UpdateSubscriptionForPosition(value));           
            this.WhenAnyValue(x => x.ToConnector.PositionConnectPoint).Subscribe(value => EndPointUpdate(value));
            this.WhenAnyValue(x => x.FromConnector.Selected).Subscribe(value => Select(value));
            this.WhenAnyValue(x => x.NodesCanvas.Theme).Subscribe(_ => Select(this.FromConnector.Selected));
        }
        private void UpdateSubscriptionForPosition(bool nodeIsCollapse)
        {
            if(!nodeIsCollapse)
            {
                subscriptionOnOutputPositionChange?.Dispose();
                subscriptionOnConnectorPositionChange = this.WhenAnyValue(x => x.FromConnector.PositionConnectPoint).Subscribe(value => StartPointUpdate(value));
                
            }
            else
            {
                subscriptionOnConnectorPositionChange?.Dispose();
                subscriptionOnOutputPositionChange = this.WhenAnyValue(x => x.FromConnector.Node.Output.PositionConnectPoint).Subscribe(value => StartPointUpdate(value));              
            }
        }
        private void Initial(NodesCanvasViewModel viewModelNodesCanvas, ConnectorViewModel fromConnector)
        {
            NodesCanvas = viewModelNodesCanvas;
            FromConnector = fromConnector;
            FromConnector.Connect = this;
            this.EndPoint = fromConnector.PositionConnectPoint;
        }
        private void Select(bool value)
        {
            
                if (this.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu || this.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                {
                    if (this.FromConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                    {
                        this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorEthernet"] as SolidColorBrush;
                    }
                    else if (this.FromConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                    {
                        this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorFastEthernet"] as SolidColorBrush;
                    }
                    else if (this.FromConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                    {
                        this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorGigabitEthernet"] as SolidColorBrush;
                    }
                    else if (this.FromConnector.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
                    {
                        this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnector10GigabitEthernet"] as SolidColorBrush;
                    }
                    else if (this.FromConnector.KapasiteId == (int)KapasiteEnum._40GigabitEthernet)
                    {
                        this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnector40GigabitEthernet"] as SolidColorBrush;
                    }
                }
                else
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorGucArayuzu"] as SolidColorBrush;
                }
            

            //this.Stroke =  Application.Current.Resources[value ? "ColorSelectedElement": "ColorConnect"] as SolidColorBrush;
        }
        private void StartPointUpdate(Point point)
        {
            var offset1 = ((FromConnector.Node.InputList.Count() - 1) * 20);
            var offset2 = FromConnector.Node.GucInputList.Count > 0 ? (FromConnector.Node.GucInputList.Count() * 20) : 0;

            StartPoint = point.Addition(0, (offset1 + offset2) - 5);
        }
        private void EndPointUpdate(Point point)
        {
            EndPoint = point;
        }
        private void UpdateMedium()
        {
            Point different = EndPoint.Subtraction(StartPoint);
            Point1 = new Point(StartPoint.X + 3 * different.X / 8, StartPoint.Y + 1 * different.Y / 8);
            Point2 = new Point(StartPoint.X + 5 * different.X / 8, StartPoint.Y + 7 * different.Y / 8);
        }

        #endregion Setup Subscriptions

    }
}
