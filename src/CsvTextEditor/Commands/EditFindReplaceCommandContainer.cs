// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditUndoCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Models;
    using Orc.CsvTextEditor.CsvTextEditorToolManagement;
    using Orc.ProjectManagement;

    public class EditFindReplaceCommandContainer : EditProjectCommandContainerBase
    {
        private readonly IServiceLocator _serviceLocator;
        private ICsvTextEditorToolManager _csvEditorToolManager;

        #region Constructors
        public EditFindReplaceCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.FindReplace, commandManager, projectManager, serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;
        }
        #endregion

        #region Methods
        protected override void Execute(object parameter)
        {
            var activeProject = _projectManager.ActiveProject;

            _csvEditorToolManager = _serviceLocator.ResolveType<ICsvTextEditorToolManager>(activeProject);

            _csvEditorToolManager.AddTool<FindReplaceTextEditorTool>();
        }
        #endregion
    }
}