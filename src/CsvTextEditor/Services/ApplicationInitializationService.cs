// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Threading;
    using Catel.Windows.Controls;
    using Orc.ProjectManagement;
    using Orchestra.Markup;
    using Orchestra.Services;
    using ProjectManagement;
    using Orc.Squirrel;
    using MethodTimer;
    using Settings = CsvTextEditor.Settings;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly ICommandManager _commandManager;

        private readonly IServiceLocator _serviceLocator;
        #endregion

        #region Constructors
        public ApplicationInitializationService(IServiceLocator serviceLocator, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => commandManager);

            _serviceLocator = serviceLocator;
            _commandManager = commandManager;
        }
        #endregion

        #region Methods
        public override async Task InitializeBeforeCreatingShellAsync()
        {
            RegisterTypes();
            InitializeFonts();
            InitializeCommands();
            InitializeWatchers();

            await TaskHelper.RunAndWaitAsync(new Func<Task>[] {
                ImprovePerformanceAsync,
                CheckForUpdatesAsync
            });
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            var mainWindowTitleService = _serviceLocator.ResolveType<IMainWindowTitleService>();

            mainWindowTitleService.UpdateTitle();

            await base.InitializeAfterCreatingShellAsync();
        }

        private void RegisterTypes()
        {
            _serviceLocator.RegisterType<IManageUserDataService, ManageUserDataService>();
            _serviceLocator.RegisterType<IProjectSerializerSelector, ProjectSerializerSelector>();
            _serviceLocator.RegisterType<IMainWindowTitleService, MainWindowTitleService>();
        }

        private void InitializeFonts()
        {
            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/CsvTextEditor;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultFontFamily = "FontAwesome";

            FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
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
        }

        [Time]
        private async Task CheckForUpdatesAsync()
        {
            Log.Info("Checking for updates");

            var maximumReleaseDate = DateTime.MaxValue;

            var updateService = _serviceLocator.ResolveType<IUpdateService>();
            updateService.Initialize(Settings.Application.AutomaticUpdates.AvailableChannels, Settings.Application.AutomaticUpdates.DefaultChannel,
                Settings.Application.AutomaticUpdates.CheckForUpdatesDefaultValue);

#pragma warning disable 4014
            // Not dot await, it's a background thread
            updateService.HandleUpdatesAsync(maximumReleaseDate);
#pragma warning restore 4014
        }
        #endregion
    }
}