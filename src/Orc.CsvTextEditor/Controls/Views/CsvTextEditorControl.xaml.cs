// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Windows;
    using Catel.Logging;
    using Catel.MVVM.Views;

    public partial class CsvTextEditorControl
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        static CsvTextEditorControl()
        {
            typeof (CsvTextEditorControl).AutoDetectViewPropertiesToSubscribe();
        }

        public CsvTextEditorControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Dependency properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (CsvTextEditorControl), new PropertyMetadata(default(string)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public object Scope
        {
            get { return (object) GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register(
            "Scope", typeof (object), typeof (CsvTextEditorControl), new PropertyMetadata(default(object)));
        #endregion
    }
}