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
        public EditUndoCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.Undo, commandManager, projectManager, csvTextEditorInstanceProvider)
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

            return CsvTextEditorInstance?.CanUndo ?? false;
        }

        protected override void Execute(object parameter)
        {
            CsvTextEditorInstance.Undo();

            base.Execute(parameter);
        }
        #endregion
    }
}
