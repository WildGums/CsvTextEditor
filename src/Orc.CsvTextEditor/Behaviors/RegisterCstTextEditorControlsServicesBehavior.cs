// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterCstTextEditorControlsServicesBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.ComponentModel;
    using Catel.IoC;
    using Catel.Windows.Interactivity;
    using Services;

    public class RegisterCstTextEditorControlsServicesBehavior : BehaviorBase<CsvTextEditorControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var textEditorControl = AssociatedObject;

            textEditorControl.Initialized += OnInitialized;
            textEditorControl.PropertyChanged += OnTextEditorControlPropertyChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var textEditorControl = AssociatedObject;

            textEditorControl.Initialized -= OnInitialized;
            textEditorControl.PropertyChanged -= OnTextEditorControlPropertyChanged;
        }

        private void OnInitialized(object sender, EventArgs eventArgs)
        {
            UpdateServiceRegistration();
        }

        private void OnTextEditorControlPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(AssociatedObject.Scope))
            {
                return;
            }

            UpdateServiceRegistration();
        }

        private void UpdateServiceRegistration()
        {
            var textEditorControl = AssociatedObject;
            var scope = textEditorControl.Scope;
            var serviceLocator = this.GetServiceLocator();
            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();

            var textEditor = textEditorControl.TextEditor;
            if (textEditor == null)
            {
                return;
            }

            if (!serviceLocator.IsTypeRegistered<ICsvTextEditorService>(scope))
            {
                var csvTextEditorService = (ICsvTextEditorService) typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorService>(scope, textEditor);
                serviceLocator.RegisterInstance(csvTextEditorService, scope);
            }

            if (!serviceLocator.IsTypeRegistered<ICsvTextEditorSearchService>(scope))
            {
                var csvTextEditorSearchService = (ICsvTextEditorSearchService) typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorSearchService>(textEditor);
                serviceLocator.RegisterInstance(csvTextEditorSearchService, scope);
            }

            if (!serviceLocator.IsTypeRegistered<ICsvTextSynchronizationService>(scope))
            {
                var csvTextSynchronizationService = (ICsvTextSynchronizationService) typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextSynchronizationService>();
                serviceLocator.RegisterInstance(csvTextSynchronizationService, scope);
            }
        }
    }
}