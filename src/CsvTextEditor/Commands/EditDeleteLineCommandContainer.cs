namespace CsvTextEditor
{
    using Catel.MVVM;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditDeleteLineCommandContainer : QuickFormatCommandContainerBase
    {
        #region Constructors
        public EditDeleteLineCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider,
            INotificationService notificationService)
            : base(Commands.Edit.DeleteLine, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider)
        {
        }

        #endregion

        #region Methods
        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<RemoveLineOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "removing line";
        }
        #endregion
    }
}
