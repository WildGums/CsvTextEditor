// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditUndoCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditUndoCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditUndoCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.Undo, commandManager, projectManager, serviceLocator)
        {
        }
        #endregion

        #region Methods
        protected override bool CanExecute(object parameter)
        {
            if (!base.CanExecute(parameter))
            {
                return false;
            }

            return CsvTextEditorService?.CanUndo ?? false;
        }

        protected override void Execute(object parameter)
        {
            CsvTextEditorService.Undo();

            base.Execute(parameter);
        }
        #endregion
    }
}