// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditCutCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditCutCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditCutCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.Cut, commandManager, projectManager, serviceLocator)
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
            CsvTextEditorService.Cut();

            base.Execute(parameter);
        }
        #endregion
    }
}