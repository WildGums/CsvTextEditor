// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditCopyCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditCopyCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditCopyCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.Copy, commandManager, projectManager, serviceLocator)
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

            return CsvTextEditorService?.HasSelection ?? false;
        }

        protected override void Execute(object parameter)
        {
            CsvTextEditorService.Copy();

            base.Execute(parameter);
        }
        #endregion
    }
}