// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
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
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;
    using Orchestra.Markup;
    using Orchestra.Services;
    using ProjectManagement;

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
        public override Task InitializeBeforeCreatingShellAsync()
        {
            RegisterTypes();
            InitializeFonts();
            InitializeCommands();
            ImprovePerformance();

            return TaskHelper.Completed;
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            await base.InitializeAfterCreatingShellAsync();
        }

        private void RegisterTypes()
        {
            _serviceLocator.RegisterType<IProjectSerializerSelector, ProjectSerializerSelector>();
        }

        private void InitializeFonts()
        {
            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/CsvTextEditor;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultFontFamily = "FontAwesome";

            FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        }

        private static void ImprovePerformance()
        {
            Log.Info("Improving performance");

            ModelBase.DefaultSuspendValidationValue = true;
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

            _commandManager.CreateCommandWithGesture(typeof(Commands.Help), "About");
        }
        #endregion
    }
}