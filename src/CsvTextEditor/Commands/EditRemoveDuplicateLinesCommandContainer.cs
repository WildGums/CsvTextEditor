// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditRemoveDuplicateLinesCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditRemoveDuplicateLinesCommandContainer : QuickFormatCommandContainerBase
    {
        #region Constructors
        public EditRemoveDuplicateLinesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider,
            INotificationService notificationService)
            : base(Commands.Edit.RemoveDuplicateLines, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider)
        {
        }

        #endregion

        #region Methods
        protected override void EcecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<RemoveDuplicateLinesOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "removing duplicate lines";
        }
        #endregion
    }
}
