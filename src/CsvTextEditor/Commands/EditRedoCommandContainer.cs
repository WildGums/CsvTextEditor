// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditRedoCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditRedoCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditRedoCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.Redo, commandManager, projectManager, serviceLocator)
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

            return CsvTextEditorService?.CanRedo ?? false;
        }

        protected override void Execute(object parameter)
        {
            CsvTextEditorService.Redo();

            base.Execute(parameter);
        }
        #endregion
    }
}