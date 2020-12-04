// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Views
{
    using Catel.IoC;

    public partial class MainView
    {
        #region Constructors
        public MainView()
        {
            InitializeComponent();

            var serviceLocator = this.GetServiceLocator();
            var typeFactory = this.GetTypeFactory();

            var csvTextEditorInstanceProvider
                = typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorInstanceProvider>();

            serviceLocator.RegisterInstance<ICsvTextEditorInstanceProvider>(csvTextEditorInstanceProvider);
        }
        #endregion
    }
}
