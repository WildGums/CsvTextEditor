namespace CsvTextEditor
{
    using Catel.MVVM;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditDuplicateLineCommandContainer : QuickFormatCommandContainerBase
    {
        #region Constructors
        public EditDuplicateLineCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider,
            INotificationService notificationService)
            : base(Commands.Edit.DuplicateLine, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider)
        {
        }

        #endregion

        #region Methods
        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<DuplicateLineOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "duplicate line";
        }
        #endregion
    }
}
