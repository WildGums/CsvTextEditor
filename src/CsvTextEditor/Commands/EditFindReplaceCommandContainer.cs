﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditUndoCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
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
            CsvTextEditorService.ShowFindReplaceDialog();

            base.Execute(parameter);
        }
        #endregion
    }
}