namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditTrimWhitespacesCommandContainer : QuickFormatCommandContainerBase
    {
        public EditTrimWhitespacesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, INotificationService notificationService, 
            IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.TrimWhitespaces, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<TrimWhitespacesOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "trimming white-spaces";
        }
    }
}
