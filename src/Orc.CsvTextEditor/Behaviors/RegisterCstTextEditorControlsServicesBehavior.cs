// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterCstTextEditorControlsServicesBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.ComponentModel;
    using Catel.IoC;
    using Catel.Windows.Interactivity;
    using Services;

    internal class RegisterCstTextEditorControlsServicesBehavior : BehaviorBase<CsvTextEditorControl>
    {
        private readonly ITypeFactory _typeFactory;
        private readonly IServiceLocator _serviceLocator;

        public RegisterCstTextEditorControlsServicesBehavior()
        {
            _serviceLocator = this.GetServiceLocator();
            _typeFactory = _serviceLocator.ResolveType<ITypeFactory>();
        }

        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            UpdateServiceRegistration();

            var textEditorControl = AssociatedObject;
            textEditorControl.PropertyChanged += OnTextEditorControlPropertyChanged;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            var textEditorControl = AssociatedObject;
            textEditorControl.PropertyChanged -= OnTextEditorControlPropertyChanged;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnTextEditorControlPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.HasPropertyChanged(nameof(AssociatedObject.Scope)))
            {
                UpdateServiceRegistration();
            }
        }

        private void UpdateServiceRegistration()
        {
            var textEditorControl = AssociatedObject;
            var textEditor = textEditorControl.TextEditor;
            if (textEditor == null)
            {
                return;
            }

            var scope = textEditorControl.Scope;
            if (!_serviceLocator.IsTypeRegistered<ICsvTextEditorService>(scope))
            {
                var csvTextEditorService = (ICsvTextEditorService)_typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorService>(scope, textEditor);
                _serviceLocator.RegisterInstance(csvTextEditorService, scope);
            }

            if (!_serviceLocator.IsTypeRegistered<ICsvTextEditorSearchService>(scope))
            {
                var csvTextEditorSearchService = (ICsvTextEditorSearchService) _typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorSearchService>(textEditor);
                _serviceLocator.RegisterInstance(csvTextEditorSearchService, scope);
            }

            if (!_serviceLocator.IsTypeRegistered<ICsvTextSynchronizationService>(scope))
            {
                var csvTextSynchronizationService = (ICsvTextSynchronizationService)_typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextSynchronizationService>();
                _serviceLocator.RegisterInstance(csvTextSynchronizationService, scope);
            }
        }
    }
}