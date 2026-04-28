namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditDeleteLineCommandContainer : QuickFormatCommandContainerBase
    {
        public EditDeleteLineCommandContainer(ICommandManager commandManager, IProjectManager projectManager,
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, INotificationService notificationService,
            IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.DeleteLine, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<RemoveLineOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "removing line";
        }
    }
}
