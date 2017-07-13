// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditTrimWhitespacesCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditTrimWhitespacesCommandContainer : QuickFormatCommandContainerBase
    {
        #region Constructors
        public EditTrimWhitespacesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator,
            INotificationService notificationService)
            : base(Commands.Edit.TrimWhitespaces, commandManager, projectManager, serviceLocator, notificationService)
        {
        }

        #endregion

        #region Methods
        protected override void EcecuteOperation()
        {
            CsvTextEditorService.TrimWhitespaces();
        }

        protected override string GetOperationDescription()
        {
            return "trimming white-spaces";
        }
        #endregion
    }
}