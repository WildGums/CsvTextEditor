// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditUndoCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class EditFindReplaceCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditFindReplaceCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.FindReplace, commandManager, projectManager, serviceLocator)
        {
        }
        #endregion

        #region Methods
        protected override void Execute(object parameter)
        {
            CsvTextEditorService.ShowTool<FindReplaceTextEditorTool>();
        }
        #endregion
    }
}