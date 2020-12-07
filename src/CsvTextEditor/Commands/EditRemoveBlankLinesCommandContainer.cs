// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditRemoveBlankLinesCommandContainer.cs" company="WildGums">
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

    public class EditRemoveBlankLinesCommandContainer : QuickFormatCommandContainerBase
    {
        #region Constructors
        public EditRemoveBlankLinesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider,
            INotificationService notificationService)
            : base(Commands.Edit.RemoveBlankLines, commandManager, projectManager, notificationService, csvTextEditorInstanceProvider)
        {
        }

        #endregion

        #region Methods

        protected override void EcecuteOperation()
        {
            CsvTextEditorInstance.ExecuteOperation<RemoveBlankLinesOperation>();
        }

        protected override string GetOperationDescription()
        {
            return "removing blank lines";
        }
        #endregion
    }
}
