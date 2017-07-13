// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditRemoveDuplicateLinesCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditRemoveDuplicateLinesCommandContainer : QuickFormatCommandContainerBase
    {
        #region Constructors
        public EditRemoveDuplicateLinesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator,
            INotificationService notificationService)
            : base(Commands.Edit.RemoveDuplicateLines, commandManager, projectManager, serviceLocator, notificationService)
        {
        }

        #endregion

        #region Methods
        protected override void EcecuteOperation()
        {
            CsvTextEditorService.RemoveDuplicateLines();
        }

        protected override string GetOperationDescription()
        {
            return "removing duplicate lines";
        }
        #endregion
    }
}