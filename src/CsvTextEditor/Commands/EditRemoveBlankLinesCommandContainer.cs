namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditRemoveBlankLinesCommandContainer : QuickFormatCommandContainerBase
    {
        public EditRemoveBlankLinesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, INotificationService notificationService, 
            IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.RemoveBlankLines, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<RemoveBlankLinesOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "removing blank lines";
        }
    }
}
