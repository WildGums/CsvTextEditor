namespace CsvTextEditor.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Orc.ProjectManagement;

    public class SaveProjectChangesService : ISaveProjectChangesService
    {
        private readonly IMessageService _messageService;
        private readonly IBusyIndicatorService _busyIndicatorService;
        private readonly IProjectManager _projectManager;

        public SaveProjectChangesService(IProjectManager projectManager, IBusyIndicatorService busyIndicatorService,
            IMessageService messageService)
        {
            ArgumentNullException.ThrowIfNull(projectManager);
            ArgumentNullException.ThrowIfNull(busyIndicatorService);
            ArgumentNullException.ThrowIfNull(messageService);

            _projectManager = projectManager;
            _busyIndicatorService = busyIndicatorService;
            _messageService = messageService;
        }

        public Task<bool> EnsureChangesSavedAsync(Project project, SaveChangesReason reason)
        {
            ArgumentNullException.ThrowIfNull(project);

            var message = GetPromptText(project, reason);

            return EnsureChangesSavedAsync(project, message);
        }

        protected virtual string GetPromptText(Project project, SaveChangesReason reason)
        {
            ArgumentNullException.ThrowIfNull(project);

            var location = project.Location;

            string message;
            switch (reason)
            {
                case SaveChangesReason.Closing:
                    message = $"The file '{location}' has to be closed, but is was changed\n\nDo you want to save changes?";
                    break;

                case SaveChangesReason.Refreshing:
                    message = $"The file '{location}' has to be refreshed, but is was changed\n\nDo you want to save changes?";
                    break;

                default:
                    message = $"The file '{location}' has been changed\n\nDo you want to save changes?";
                    break;
            }

            return message;
        }

        private async Task<bool> EnsureChangesSavedAsync(Project project, string message)
        {
            ArgumentNullException.ThrowIfNull(project);
            Argument.IsNotNullOrEmpty(() => message);

            if (project is null || !project.IsDirty)
                return true;

            var caption = AssemblyHelper.GetEntryAssembly().Title();

            MessageResult messageBoxResult;

            using (_busyIndicatorService.HideTemporarily())
            {
                messageBoxResult = await _messageService.ShowAsync(message, caption, MessageButton.YesNoCancel).ConfigureAwait(false);
            }

            switch (messageBoxResult)
            {
                case MessageResult.Cancel:
                    return false;

                case MessageResult.No:
                    return true;

                case MessageResult.Yes:
                    return await _projectManager.SaveActiveProjectAsync(project.Location).ConfigureAwait(false);
            }

            return false;
        }
    }
}
