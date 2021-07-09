namespace CsvTextEditor
{
    using Catel.MVVM;
    using Orc.CsvTextEditor.Operations;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class EditTrimWhitespacesCommandContainer : QuickFormatCommandContainerBase
    {
        #region Constructors
        public EditTrimWhitespacesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider,
            INotificationService notificationService)
            : base(Commands.Edit.TrimWhitespaces, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider)
        {
        }

        #endregion

        #region Methods
        protected override void ExecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<TrimWhitespacesOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "trimming white-spaces";
        }
        #endregion
    }
}
