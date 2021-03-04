﻿#pragma checksum "..\..\..\..\View\NodesCanvas.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A6E393087898DC8D225890EE3E44226727EE6C3D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Bu kod araç tarafından oluşturuldu.
//     Çalışma Zamanı Sürümü:4.0.30319.42000
//
//     Bu dosyada yapılacak değişiklikler yanlış davranışa neden olabilir ve
//     kod yeniden oluşturulursa kaybolur.
// </auto-generated>
//------------------------------------------------------------------------------

using AYP.View;
using AYP.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AYP.View {
    
    
    /// <summary>
    /// NodesCanvas
    /// </summary>
    public partial class NodesCanvas : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BorderElement;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas CanvasElement;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.View.Selector Selector;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.View.Cutter Cutter;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl ItemControlElement;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.TransformGroup TransformGroup;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform Scale;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.TranslateTransform Translate;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Data.CollectionContainer Connects;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Data.CollectionContainer Nodes;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ItemAddNode;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ItemDelete;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ItemCollapsUp;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ItemExpandDown;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.KeyBinding BindingUndo;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.KeyBinding BindingRedo;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.KeyBinding BindingSelectAll;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.KeyBinding BindingAddNode;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.KeyBinding BindingDeleteSelectedElements;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.KeyBinding BindingExportToJPEG;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.MouseBinding BindingSelect;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\..\View\NodesCanvas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.MouseBinding BindingCut;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AYP;component/view/nodescanvas.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\NodesCanvas.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.3.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.BorderElement = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.CanvasElement = ((System.Windows.Controls.Canvas)(target));
            return;
            case 3:
            this.Selector = ((AYP.View.Selector)(target));
            return;
            case 4:
            this.Cutter = ((AYP.View.Cutter)(target));
            return;
            case 5:
            this.ItemControlElement = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 6:
            this.TransformGroup = ((System.Windows.Media.TransformGroup)(target));
            return;
            case 7:
            this.Scale = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 8:
            this.Translate = ((System.Windows.Media.TranslateTransform)(target));
            return;
            case 9:
            this.Connects = ((System.Windows.Data.CollectionContainer)(target));
            return;
            case 10:
            this.Nodes = ((System.Windows.Data.CollectionContainer)(target));
            return;
            case 11:
            this.ItemAddNode = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 12:
            this.ItemDelete = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 13:
            this.ItemCollapsUp = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 14:
            this.ItemExpandDown = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 15:
            this.BindingUndo = ((System.Windows.Input.KeyBinding)(target));
            return;
            case 16:
            this.BindingRedo = ((System.Windows.Input.KeyBinding)(target));
            return;
            case 17:
            this.BindingSelectAll = ((System.Windows.Input.KeyBinding)(target));
            return;
            case 18:
            this.BindingAddNode = ((System.Windows.Input.KeyBinding)(target));
            return;
            case 19:
            this.BindingDeleteSelectedElements = ((System.Windows.Input.KeyBinding)(target));
            return;
            case 20:
            this.BindingExportToJPEG = ((System.Windows.Input.KeyBinding)(target));
            return;
            case 21:
            this.BindingSelect = ((System.Windows.Input.MouseBinding)(target));
            return;
            case 22:
            this.BindingCut = ((System.Windows.Input.MouseBinding)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

