﻿#pragma checksum "..\..\..\..\View\TableOfTransitionsItem.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "12B59EEFA68535DCC4330297ACA3D7EAF65443F0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AYP.Styles;
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
    /// TableOfTransitionsItem
    /// </summary>
    public partial class TableOfTransitionsItem : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\View\TableOfTransitionsItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridElement;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\View\TableOfTransitionsItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition ColumnStateFrom;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\View\TableOfTransitionsItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition TransitionName;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\View\TableOfTransitionsItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition StateTo;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\View\TableOfTransitionsItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.Styles.MyTextBox TextBoxElementStateFrom;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\View\TableOfTransitionsItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.Styles.MyTextBox TextBoxElementTransitionName;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\View\TableOfTransitionsItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AYP.Styles.MyTextBox TextBoxElementStateTo;
        
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
            System.Uri resourceLocater = new System.Uri("/AYP;component/view/tableoftransitionsitem.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\TableOfTransitionsItem.xaml"
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
            this.GridElement = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.ColumnStateFrom = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 3:
            this.TransitionName = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 4:
            this.StateTo = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 5:
            this.TextBoxElementStateFrom = ((AYP.Styles.MyTextBox)(target));
            return;
            case 6:
            this.TextBoxElementTransitionName = ((AYP.Styles.MyTextBox)(target));
            return;
            case 7:
            this.TextBoxElementStateTo = ((AYP.Styles.MyTextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
