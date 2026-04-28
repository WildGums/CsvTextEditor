namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditDuplicateLineCommandContainer : QuickFormatCommandContainerBase
    {
        public EditDuplicateLineCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, INotificationService notificationService, 
            IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.DuplicateLine, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<DuplicateLineOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "duplicate line";
        }
    }
}
