﻿#pragma checksum "..\..\..\..\View\Node.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8FEF0A9FE0F20719BCA88775800C88A663F8727A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AYP.Helpers.Enums;
using AYP.Styles.Node;
using AYP.View;
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
    /// Node
    /// </summary>
    public partial class Node : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BorderElement;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.Styles.Node.ElementNodeHeader NodeHeaderElement;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridElement;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.View.LeftConnector Input;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.View.RightConnector Output;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl ItemsControlTransitions;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.TransformGroup TransformGroup;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform ScaleTransformElement;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.TranslateTransform TranslateTransformElement;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\View\Node.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Input.MouseBinding BindingSelect;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AYP;component/view/node.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Node.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
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
            this.NodeHeaderElement = ((AYP.Styles.Node.ElementNodeHeader)(target));
            return;
            case 3:
            this.GridElement = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.Input = ((AYP.View.LeftConnector)(target));
            return;
            case 5:
            this.Output = ((AYP.View.RightConnector)(target));
            return;
            case 6:
            this.ItemsControlTransitions = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 7:
            this.TransformGroup = ((System.Windows.Media.TransformGroup)(target));
            return;
            case 8:
            this.ScaleTransformElement = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 9:
            this.TranslateTransformElement = ((System.Windows.Media.TranslateTransform)(target));
            return;
            case 10:
            this.BindingSelect = ((System.Windows.Input.MouseBinding)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
