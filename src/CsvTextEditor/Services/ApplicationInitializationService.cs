namespace CsvTextEditor.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Windows.Controls;
    using Orc.ProjectManagement;
    using Orchestra.Services;
    using ProjectManagement;
    using Orc.Squirrel;
    using MethodTimer;
    using Settings = CsvTextEditor.Settings;
    using Fluent;
    using CsvTextEditor.Views;
    using Orc.Automation.Controls;
    using System.Collections.Generic;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly ICommandManager _commandManager;
        private readonly IBusyIndicatorService _busyIndicatorService;

        private readonly IServiceLocator _serviceLocator;

        public ApplicationInitializationService(IServiceLocator serviceLocator, ICommandManager commandManager, IBusyIndicatorService busyIndicatorService)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);
            ArgumentNullException.ThrowIfNull(commandManager);
            ArgumentNullException.ThrowIfNull(busyIndicatorService);

            _serviceLocator = serviceLocator;
            _commandManager = commandManager;
            _busyIndicatorService = busyIndicatorService;
        }
   
        public override async Task InitializeBeforeCreatingShellAsync()
        {
            RegisterTypes();
            InitializeFonts();
            InitializeCommands();
            InitializeWatchers();

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

            var mainWindowTitleService = _serviceLocator.ResolveType<IMainWindowTitleService>();
            mainWindowTitleService.UpdateTitle();

            await base.InitializeAfterCreatingShellAsync();
        }

        public override async Task InitializeAfterShowingShellAsync()
        {
            await base.InitializeAfterShowingShellAsync();

            await LoadProjectAsync();
        }

        private void RegisterTypes()
        {
            _serviceLocator.RegisterType<IProjectSerializerSelector, ProjectSerializerSelector>();
            _serviceLocator.RegisterType<IMainWindowTitleService, MainWindowTitleService>();
            _serviceLocator.RegisterType<ISaveProjectChangesService, SaveProjectChangesService>();
            _serviceLocator.RegisterType<IFileExtensionService, FileExtensionService>();
            _serviceLocator.RegisterType<IInitialProjectLocationService, InitialProjectLocationService>();

            _serviceLocator.RegisterType<IProjectInitializer, FileProjectInitializer>();
            
            _serviceLocator.RegisterType<ICsvTextEditorInstanceProvider, CsvTextEditorInstanceProvider>();

            _serviceLocator.RegisterTypeAndInstantiate<ProjectManagementCloseApplicationWatcher>();
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
            Log.Info("Improving performance");

            UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private void InitializeCommands()
        {
            _commandManager.CreateCommandWithGesture(typeof(Commands.File), "Close");
            _commandManager.CreateCommandWithGesture(typeof(Commands.File), "Open");
            _commandManager.CreateCommandWithGesture(typeof(Commands.File), "Save");
            _commandManager.CreateCommandWithGesture(typeof(Commands.File), "SaveAs");

            _commandManager.CreateCommandWithGesture(typeof(Commands.File), "OpenInTextEditor");
            _commandManager.CreateCommandWithGesture(typeof(Commands.File), "OpenInExcel");

            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "Undo");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "Redo");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "Copy");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "Paste");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "Cut");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "DeleteLine");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "DuplicateLine");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "FindReplace");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "RemoveBlankLines");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "RemoveDuplicateLines");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Edit), "TrimWhitespaces");

            _commandManager.CreateCommandWithGesture(typeof(Commands.Settings), "General");

            _commandManager.CreateCommandWithGesture(typeof(Commands.Help), "About");
        }

        private void InitializeWatchers()
        {
            _serviceLocator.RegisterTypeAndInstantiate<CsvTextEditorAutoCompleteProjectWatcher>();
            _serviceLocator.RegisterTypeAndInstantiate<RecentlyUsedItemsProjectWatcher>();
            _serviceLocator.RegisterTypeAndInstantiate<MainWindowTitleProjectWatcher>();
            _serviceLocator.RegisterTypeAndInstantiate<CsvTextEditorIsDirtyProjectWatcher>();
        }

        [Time]
        private async Task CheckForUpdatesAsync()
        {
            Log.Info("Checking for updates");

            var updateService = _serviceLocator.ResolveRequiredType<IUpdateService>();
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
                var projectManager = _serviceLocator.ResolveType<IProjectManager>();
                if (projectManager is null)
                {
                    const string error = "Failed to resolve project manager";
                    Log.Error(error);
                    throw new Exception(error);
                }

                await projectManager.InitializeAsync();
            }
        }
    }
}
