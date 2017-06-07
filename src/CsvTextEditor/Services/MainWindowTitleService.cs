// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowTitleService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Services
{
    using Catel;
    using Catel.Reflection;
    using Orc.ProjectManagement;

    public class MainWindowTitleService : IMainWindowTitleService
    {
        #region Fields
        private readonly string _defaulTitle;
        private readonly IProjectManager _projectManager;
        private readonly ShellActivatedActionQueue _shellActivatedActionQueue;
        #endregion

        #region Constructors
        public MainWindowTitleService(IProjectManager projectManager)
        {
            Argument.IsNotNull(() => projectManager);

            _projectManager = projectManager;

            _shellActivatedActionQueue = new ShellActivatedActionQueue();

            _defaulTitle = AssemblyHelper.GetEntryAssembly().Title();
        }
        #endregion

        #region Methods
        public void UpdateTitle()
        {
            _shellActivatedActionQueue.EnqueueAction(() =>
            {
                var project = _projectManager.ActiveProject;
                var app = System.Windows.Application.Current;
                var title = _defaulTitle;

                if (project != null)
                {
                    title += $" - {project.Title}";

                    if (project.IsDirty)
                    {
                        title += " *";
                    }
                }

                app.MainWindow.SetCurrentValue(System.Windows.Window.TitleProperty, title);
            });
        }
        #endregion
    }
}