﻿#pragma checksum "..\..\..\Pages\PageFavoriteRecipes.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DEA5FDE463261417F7667ED4A383F9E4932CB165207E9CDC45E4D77AFDB02F12"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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
using WpfApp1.Pages;


namespace WpfApp1.Pages {
    
    
    /// <summary>
    /// PageFavoriteRecipes
    /// </summary>
    public partial class PageFavoriteRecipes : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\Pages\PageFavoriteRecipes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboSort;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Pages\PageFavoriteRecipes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextSearch;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Pages\PageFavoriteRecipes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListFavoriteRecipes;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Pages\PageFavoriteRecipes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CountRecords;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\Pages\PageFavoriteRecipes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bEditRecipe;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\Pages\PageFavoriteRecipes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btExportToWord;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp1;component/pages/pagefavoriterecipes.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\PageFavoriteRecipes.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ComboSort = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\..\Pages\PageFavoriteRecipes.xaml"
            this.ComboSort.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboSort_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\..\Pages\PageFavoriteRecipes.xaml"
            this.TextSearch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextSearch_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ListFavoriteRecipes = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            this.CountRecords = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.bEditRecipe = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\..\Pages\PageFavoriteRecipes.xaml"
            this.bEditRecipe.Click += new System.Windows.RoutedEventHandler(this.bEditRecipe_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btExportToWord = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\Pages\PageFavoriteRecipes.xaml"
            this.btExportToWord.Click += new System.Windows.RoutedEventHandler(this.btExportToWord_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

