namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditRemoveDuplicateLinesCommandContainer : QuickFormatCommandContainerBase
    {
        public EditRemoveDuplicateLinesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, INotificationService notificationService,
            IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.RemoveDuplicateLines, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<RemoveDuplicateLinesOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "removing duplicate lines";
        }
    }
}
