// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorInstanceProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Collections.Generic;
    using Catel;
    using Catel.IoC;
    using Models;
    using Orc.CsvTextEditor;

    public class CsvTextEditorInstanceProvider : ICsvTextEditorInstanceProvider
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public CsvTextEditorInstanceProvider(IServiceLocator serviceLocator, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;
            _typeFactory = typeFactory;
        }
        #endregion

        #region Methods
        public ICsvTextEditorInstance GetInstance(Project project)
        {
            if (_serviceLocator.IsTypeRegistered<ICsvTextEditorInstance>(project))
            {
                return _serviceLocator.ResolveType<ICsvTextEditorInstance>(project);
            }

            var csvTextEditorInstance = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorInstance>();
            _serviceLocator.RegisterInstance<ICsvTextEditorInstance>(csvTextEditorInstance, project);

            return csvTextEditorInstance;
        }
        #endregion
    }
}
