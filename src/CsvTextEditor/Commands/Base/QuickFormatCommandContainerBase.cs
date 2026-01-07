namespace CsvTextEditor
{
    using System;
    using System.Diagnostics;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public abstract class QuickFormatCommandContainerBase : EditProjectCommandContainerBase
    {
        private readonly Notification _notification;
        private readonly INotificationService _notificationService;
        private readonly Stopwatch _stopwatch;

        protected QuickFormatCommandContainerBase(string commandName, ICommandManager commandManager, 
            IProjectManager projectManager, INotificationService notificationService, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider,
            IDispatcherService dispatcherService)
            : base(commandName, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
            ArgumentNullException.ThrowIfNull(notificationService);

            _notificationService = notificationService;

            _notification = new Notification
            {
                Title = "Quick format",
                ShowTime = TimeSpan.FromSeconds(3)
            };

            _stopwatch = new Stopwatch();
        }

        public sealed override void Execute(object parameter)
        {
            _stopwatch.Restart();

            ExecuteOperation();

            _stopwatch.Stop();

            var operationDescription = GetOperationDescription();

            ShowNotification($"Finished {operationDescription} in {_stopwatch.ElapsedMilliseconds} ms");

            base.Execute(parameter);
        }

        protected abstract void ExecuteOperation();
        protected abstract string GetOperationDescription();

        private void ShowNotification(string message)
        {
            _notification.Message = message;

            _notificationService.ShowNotification(_notification);
        }
    }
}
