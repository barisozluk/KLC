﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using ReactiveUI;

using AYP.Helpers;
using AYP.Helpers.Enums;
using AYP.Helpers.Extensions;
using ReactiveUI.Fody.Helpers;
using AYP.ViewModel;
using AYP.Helpers.Transformations;

namespace AYP.View
{
    /// <summary>
    /// Interaction logic for ViewNodesCanvas.xaml
    /// </summary>
    public partial class NodesCanvas : UserControl, IViewFor<NodesCanvasViewModel>, CanBeMove
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(NodesCanvasViewModel), typeof(NodesCanvas), new PropertyMetadata(null));

        public NodesCanvasViewModel ViewModel
        {
            get { return (NodesCanvasViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (NodesCanvasViewModel)value; }
        }
        #endregion ViewModel
        public Point PositionMove { get; set; }

        private Point SumMove { get; set; }
        private TypeMove Move { get; set; } = TypeMove.None;

        public NodesCanvas()
        {
            InitializeComponent();
            ViewModel = new NodesCanvasViewModel();
            SetupCommands();
            SetupSubscriptions();
            SetupBinding();
            SetupEvents();
           
        }
        #region Setup Binding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {               
                this.OneWayBind(this.ViewModel, x => x.NodesForView, x => x.Nodes.Collection).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Connects, x => x.Connects.Collection).DisposeWith(disposable);

                //this.OneWayBind(this.ViewModel, x => x.Scale.Scales.X, x => x.Scale.ScaleX).DisposeWith(disposable);

                //this.OneWayBind(this.ViewModel, x => x.Scale.Scales.Y, x => x.Scale.ScaleY).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Selector, x => x.Selector.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Cutter, x => x.Cutter.ViewModel).DisposeWith(disposable);
            });
        }
        #endregion Setup Binding

        #region Setup Commands
        private void SetupCommands()
        {
            this.WhenActivated(disposable =>
            {

                this.BindCommand(this.ViewModel, x => x.CommandSelect,              x => x.BindingSelect, x => x.PositionRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAddNodeWithUndoRedo, x => x.BindingAddNode, x => x.PositionRight).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.CommandAddNodeWithUndoRedo, x => x.ItemAddNode, x => x.PositionLeft).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandRedo,                x => x.BindingRedo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandUndo,                x => x.BindingUndo).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandZoomOriginalSize, x => x.BindingZoomOriginalSize).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandZoomIn, x => x.BindingZoomIn).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandZoomOut, x => x.BindingZoomOut).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandSelectAll,           x => x.BindingSelectAll).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandCopy, x => x.BindingCopy).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandPaste, x => x.BindingPaste).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandGroup, x => x.BindingGroup).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandUngroup, x => x.BindingUngroup).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandAlignLeft, x => x.BindingAlignLeft).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAlignRight, x => x.BindingAlignRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAlignCenter, x => x.BindingAlignCenter).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandEditSelected, x => x.BindingEditSelected).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandCopyMultiple, x => x.BindingCopyMultiple).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.CommandUndo,                x => x.ItemCollapsUp).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.CommandSelectAll,           x => x.ItemExpandDown).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandDeleteSelectedElements, x => x.BindingDeleteSelectedElements).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandDeleteSelectedElements, x => x.ItemDelete).DisposeWith(disposable);

                //this.BindCommand(this.ViewModel, x => x.CommandCollapseUpSelected,  x => x.ItemCollapsUp).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.CommandExpandDownSelected,  x => x.ItemExpandDown).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandCopy, x => x.ItemCopy).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandPaste, x => x.ItemPaste).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandAlignLeft, x => x.ItemAlignLeft).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAlignRight, x => x.ItemAlignRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAlignCenter, x => x.ItemAlignCenter).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandGroup, x => x.ItemGroup).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandUngroup, x => x.ItemUngroup).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandZoomOriginalSize, x => x.ItemZoomOriginalSize).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandZoomIn, x => x.ItemZoomIn).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandZoomOut, x => x.ItemZoomOut).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandEditSelected, x => x.ItemEditSelected).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandCopyMultiple, x => x.ItemCopyMultiple).DisposeWith(disposable);
                
                this.BindCommand(this.ViewModel, x => x.CommandRename, x => x.BindingRename).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandRename, x => x.ItemRename).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandShowAgAkis, x => x.ItemShowAgAkis).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandArayuzEkle, x => x.ItemArayuzEkle).DisposeWith(disposable);

            });
        }
        #endregion Setup Commands
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel.Selector.Size).WithoutParameter().InvokeCommand(ViewModel, x => x.CommandSelectorIntersect).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.Cutter.EndPoint).WithoutParameter().InvokeCommand(ViewModel, x => x.CommandCutterIntersect).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.ImagePath).Where(x => !string.IsNullOrEmpty(x)).Subscribe(value => SaveCanvasToImage(value, ImageFormats.JPEG)).DisposeWith(disposable);
                this.WhenAnyValue(x=>x.ViewModel.RenderTransformMatrix).Subscribe(value=>this.CanvasElement.RenderTransform = new MatrixTransform(value)).DisposeWith(disposable);
                //here need use ZoomIn and ZoomOut

                //this.WhenAnyValue(x=>x.ViewModel.Scale.Value).Subscribe(x=> {this.zoomBorder.ZoomDeltaTo })
            });
        }

        #endregion Setup Subscriptions
        #region Setup Events
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseLeftDown(e)).DisposeWith(disposable);
                this.Events().MouseLeftButtonUp.Subscribe(e => OnEventMouseLeftUp(e));
                this.Events().MouseRightButtonDown.Subscribe(e => OnEventMouseRightDown(e)).DisposeWith(disposable);
                this.Events().MouseUp.Subscribe(e => OnEventMouseUp(e)).DisposeWith(disposable);
                this.Events().MouseMove.Subscribe(e => OnEventMouseMove(e)).DisposeWith(disposable);
                this.BorderElement.Events().MouseWheel.Subscribe(e => OnEventMouseWheel(e)).DisposeWith(disposable);
                this.Events().DragOver.Subscribe(e => OnEventDragOver(e)).DisposeWith(disposable);
                this.Cutter.Events().MouseLeftButtonUp.InvokeCommand(this.ViewModel.CommandDeleteSelectedConnects).DisposeWith(disposable);
                this.Events().PreviewMouseLeftButtonDown.Subscribe(e => OnEventPreviewMouseLeftButtonDown(e)).DisposeWith(disposable);
                this.Events().PreviewMouseRightButtonDown.Subscribe(e => OnEventPreviewMouseRightButtonDown(e)).DisposeWith(disposable);
                //this.WhenAnyValue(x => x.ViewModel.Scale.Value).Subscribe(value => { this.Canvas.Height /= value; this.Canvas.Width /= value; }).DisposeWith(disposable);
            });
        }


        private void OnEventMouseLeftDown(MouseButtonEventArgs e)
        {
            PositionMove = Mouse.GetPosition(this.CanvasElement);

            if (Mouse.Captured == null)
            {
                NodeCanvasClickMode clickMode = ViewModel.ClickMode;
                if (clickMode == NodeCanvasClickMode.Default)
                {
                    Keyboard.ClearFocus();
                    this.CaptureMouse();
                    Keyboard.Focus(this);
                    this.ViewModel.CommandUnSelectAll.ExecuteWithSubscribe();
                }
                else if (clickMode == NodeCanvasClickMode.AddNode)
                {                   
                    this.ViewModel.CommandAddNodeWithUndoRedo.Execute(PositionMove);
                }
                else if (clickMode == NodeCanvasClickMode.Select)
                {
                    this.ViewModel.CommandSelect.ExecuteWithSubscribe(PositionMove);
                }
                else if (clickMode == NodeCanvasClickMode.Cut)
                {
                    this.ViewModel.CommandCut.ExecuteWithSubscribe(PositionMove);
                }
            }
        }

        private void OnEventMouseLeftUp(MouseButtonEventArgs e)
        {
            if (Move == TypeMove.None)
                return;

            if (Move == TypeMove.MoveAll)
                this.ViewModel.CommandFullMoveAllNode.Execute(SumMove);
            else if (Move == TypeMove.MoveSelected)
                this.ViewModel.CommandFullMoveAllSelectedNode.Execute(SumMove);

            Move = TypeMove.None;
            SumMove = new Point();
        }
        private void OnEventMouseRightDown(MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

        }
        private void OnEventMouseWheel(MouseWheelEventArgs e)
        {
            Point point = e.GetPosition(this.CanvasElement);
            //Matrix value = this.CanvasElement.RenderTransform.Value;
            //double step = 1.2;
            //double zoom = e.Delta > 0 ? step : 1 / step;
            //value = MatrixExtension.ScaleAtPrepend(value,zoom, zoom, point.X, point.Y);
            //this.CanvasElement.RenderTransform = new MatrixTransform(value);
            this.ViewModel.CommandZoom.ExecuteWithSubscribe((point, e.Delta));
        }
        private void OnEventMouseUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            PositionMove = new Point();
            Keyboard.Focus(this);
        }

        private void OnEventMouseMove(MouseEventArgs e)
        {
            if (!(Mouse.Captured is CanBeMove))
                return;

            Point delta = GetDeltaMove();

            if (delta.IsClear())
                return;

            SumMove = SumMove.Addition(delta);
            if (this.IsMouseCaptured)
            {
                ViewModel.CommandPartMoveAllNode.ExecuteWithSubscribe(delta);
                Move = TypeMove.MoveAll;
            }
            else
            {
                ViewModel.CommandPartMoveAllSelectedNode.ExecuteWithSubscribe(delta);
                Move = TypeMove.MoveSelected;
            }
        }
        private void OnEventDragOver(DragEventArgs e)
        {
            Point point = e.GetPosition(this.CanvasElement);
            if (this.ViewModel.DraggedConnect != null)
            {
                point = point.Subtraction(2);
                //this.ViewModel.DraggedConnect.EndPoint = point.Division(this.ViewModel.Scale.Value);
                this.ViewModel.DraggedConnect.EndPoint = point;
            }
        }
        private void OnEventPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.ViewModel.PositionRight = e.GetPosition(this.CanvasElement);
        }
        private void OnEventPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            this.ViewModel.PositionLeft = e.GetPosition(this.CanvasElement);
            int selectedNodeCount = this.ViewModel.Nodes.Items.Where(x => x.Selected).Count();

            if(selectedNodeCount > 0)
            {
                //ItemCollapsUp.IsEnabled = true;
                //ItemCollapsUp.Opacity = 1;

                //ItemExpandDown.IsEnabled = true;
                //ItemExpandDown.Opacity = 1;

                ItemArayuzEkle.IsEnabled = true;
                ItemArayuzEkle.Opacity = 1;

                ItemShowAgAkis.IsEnabled = true;
                ItemShowAgAkis.Opacity = 1;

                ItemDelete.IsEnabled = true;
                ItemDelete.Opacity = 1;

                ItemRename.IsEnabled = true;
                ItemRename.Opacity = 1;

                ItemCopy.IsEnabled = true;
                ItemCopy.Opacity = 1;

                ItemAlignRight.IsEnabled = true;
                ItemAlignRight.Opacity = 1;

                ItemAlignCenter.IsEnabled = true;
                ItemAlignCenter.Opacity = 1;

                ItemAlignLeft.IsEnabled = true;
                ItemAlignLeft.Opacity = 1;

                ItemGroup.IsEnabled = true;
                ItemGroup.Opacity = 1;

                ItemUngroup.IsEnabled = true;
                ItemUngroup.Opacity = 1;

                ItemEditSelected.IsEnabled = true;
                ItemEditSelected.Opacity = 1;

                ItemCopyMultiple.IsEnabled = true;
                ItemCopyMultiple.Opacity = 1;

            }
            else
            {
                //ItemCollapsUp.IsEnabled = false;
                //ItemCollapsUp.Opacity = 0.25;

                //ItemExpandDown.IsEnabled = false;
                //ItemExpandDown.Opacity = 0.25;

                ItemArayuzEkle.IsEnabled = false;
                ItemArayuzEkle.Opacity = 0.25;

                ItemShowAgAkis.IsEnabled = false;
                ItemShowAgAkis.Opacity = 0.25;

                ItemDelete.IsEnabled = false;
                ItemDelete.Opacity = 0.25;

                ItemRename.IsEnabled = false;
                ItemRename.Opacity = 0.25;

                ItemCopy.IsEnabled = false;
                ItemCopy.Opacity = 0.25;

                ItemAlignRight.IsEnabled = false;
                ItemAlignRight.Opacity = 0.25;

                ItemAlignCenter.IsEnabled = false;
                ItemAlignCenter.Opacity = 0.25;

                ItemAlignLeft.IsEnabled = false;
                ItemAlignLeft.Opacity = 0.25;

                ItemGroup.IsEnabled = false;
                ItemGroup.Opacity = 0.25;

                ItemUngroup.IsEnabled = false;
                ItemUngroup.Opacity = 0.25;

                ItemEditSelected.IsEnabled = false;
                ItemEditSelected.Opacity = 0.25;

                ItemCopyMultiple.IsEnabled = false;
                ItemCopyMultiple.Opacity = 0.25;

            }

            ItemPaste.IsEnabled = this.ViewModel.NodeClipboard.Count() > 0 ? true : false;
            ItemPaste.Opacity = this.ViewModel.NodeClipboard.Count() > 0 ? 1 : 0.25;
        }

        #endregion Setup Events
        private Point GetDeltaMove()
        {
            Point CurrentPosition = Mouse.GetPosition(this.CanvasElement);
            Point result = new Point();

            if (!PositionMove.IsClear())
            {
                result = CurrentPosition.Subtraction(PositionMove);
            }

            PositionMove = CurrentPosition;
            return result;
        }

        private void SaveCanvasToImage(string filename, ImageFormats format)
        {
            //this.zoomBorder.Uniform();
            MyUtils.PanelToImage(this.CanvasElement, filename, format);
            ViewModel.CommandLogDebug.ExecuteWithSubscribe(String.Format("Scheme was exported to \"{0}\"", filename));
        }

    }
}
