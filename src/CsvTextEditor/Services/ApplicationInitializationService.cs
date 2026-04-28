namespace CsvTextEditor.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Windows.Controls;
    using CsvTextEditor.Views;
    using Fluent;
    using MethodTimer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Orc.ProjectManagement;
    using Orc.Squirrel;
    using Orchestra;
    using Settings = CsvTextEditor.Settings;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(ApplicationInitializationService));

        private readonly ICommandManager _commandManager;
        private readonly IBusyIndicatorService _busyIndicatorService;

        public ApplicationInitializationService(IServiceProvider serviceProvider, ICommandManager commandManager, 
            IBusyIndicatorService busyIndicatorService)
            : base(serviceProvider)
        {
            _commandManager = commandManager;
            _busyIndicatorService = busyIndicatorService;
        }
   
        public override async Task InitializeBeforeCreatingShellAsync()
        {
            InitializeFonts();
            InitializeCommands();

            var tasks = new List<Task>() 
            {
                Task.Run(ImprovePerformanceAsync),
                Task.Run(CheckForUpdatesAsync)
            };

            await Task.WhenAll(tasks);
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            var shellWindow = System.Windows.Application.Current.MainWindow as RibbonWindow;

            var windowCommands = new WindowCommands();
            windowCommands.Items.Add(new WindowCommandsView());
            shellWindow.WindowCommands = windowCommands;

            var mainWindowTitleService = ServiceProvider.GetRequiredService<IMainWindowTitleService>();
            mainWindowTitleService.UpdateTitle();

            await base.InitializeAfterCreatingShellAsync();
        }

        public override async Task InitializeAfterShowingShellAsync()
        {
            await base.InitializeAfterShowingShellAsync();

            await LoadProjectAsync();
        }

        private void InitializeFonts()
        {
            Orc.Theming.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/CsvTextEditor;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            Orc.Theming.FontImage.DefaultFontFamily = "FontAwesome";
            Orc.Theming.FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        }

        [Time]
        private async Task ImprovePerformanceAsync()
        {
            Logger.LogInformation("Improving performance");

            UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private void InitializeCommands()
        {
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.File), "Close");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.File), "Open");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.File), "Save");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.File), "SaveAs");

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.File), "OpenInTextEditor");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.File), "OpenInExcel");

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "Undo");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "Redo");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "Copy");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "Paste");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "Cut");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "DeleteLine");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "DuplicateLine");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "FindReplace");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "RemoveBlankLines");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "RemoveDuplicateLines");
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Edit), "TrimWhitespaces");

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Settings), "General");

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Help), "About");
        }

        [Time]
        private async Task CheckForUpdatesAsync()
        {
            Logger.LogInformation("Checking for updates");

            var updateService = ServiceProvider.GetRequiredService<IUpdateService>();
            await updateService.InitializeAsync(Settings.Application.AutomaticUpdates.AvailableChannels, Settings.Application.AutomaticUpdates.DefaultChannel,
                Settings.Application.AutomaticUpdates.CheckForUpdatesDefaultValue);

#pragma warning disable 4014
            // Not dot await, it's a background thread
            updateService.InstallAvailableUpdatesAsync(new SquirrelContext());
#pragma warning restore 4014
        }

        protected async Task LoadProjectAsync()
        {
            using (_busyIndicatorService.PushInScope())
            {
                var projectManager = ServiceProvider.GetRequiredService<IProjectManager>();
                if (projectManager is null)
                {
                    throw Logger.LogErrorAndCreateException<Exception>("Failed to resolve project manager");
                }

                await projectManager.InitializeAsync();
            }
        }
    }
}
