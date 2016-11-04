// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditPasteCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Windows;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditPasteCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditPasteCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.Paste, commandManager, projectManager, serviceLocator)
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


            return true;
        }

        protected override void Execute(object parameter)
        {
            if (Clipboard.ContainsText())
            {
                CsvTextEditorService.Paste();
            }

            base.Execute(parameter);
        }
        #endregion
    }
}