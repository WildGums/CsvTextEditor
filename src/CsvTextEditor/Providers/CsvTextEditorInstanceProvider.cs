namespace CsvTextEditor
{
    using System;
    using Catel.IoC;
    using Models;
    using Orc.CsvTextEditor;

    public class CsvTextEditorInstanceProvider : ICsvTextEditorInstanceProvider
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;

        public CsvTextEditorInstanceProvider(IServiceLocator serviceLocator, ITypeFactory typeFactory)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);

            _serviceLocator = serviceLocator;
            _typeFactory = typeFactory;
        }

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
    }
}
