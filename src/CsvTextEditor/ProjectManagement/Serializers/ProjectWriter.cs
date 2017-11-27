// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectWriter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Threading;
    using Models;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class ProjectWriter : ProjectWriterBase<Project>
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        #endregion

        #region Constructors
        public ProjectWriter(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;
        }
        #endregion

        #region Methods
        protected override Task<bool> WriteToLocationAsync(Project project, string location)
        {
            if (!_serviceLocator.IsTypeRegistered<ICsvTextEditorInstance>(project))
            {
                return TaskHelper<bool>.FromResult(false);
            }

            var csvTextEditorInstance = _serviceLocator.ResolveType<ICsvTextEditorInstance>(project);
            csvTextEditorInstance.Save(location);

            return TaskHelper<bool>.FromResult(true);
        }
        #endregion
    }
}