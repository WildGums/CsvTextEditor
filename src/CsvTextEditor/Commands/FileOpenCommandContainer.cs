// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOpenCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Base;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Orc.ProjectManagement;

    public class FileOpenCommandContainer : ProjectCommandContainerBase
    {
        private readonly IOpenFileService _openFileService;

        #region Constructors
        public FileOpenCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IOpenFileService openFileService)
            : base(Commands.File.Open, commandManager, projectManager)
        {
            Argument.IsNotNull(() => projectManager);
            Argument.IsNotNull(() => openFileService);

            _openFileService = openFileService;
        }
        #endregion

        protected override bool CanExecute(object parameter)
        {
            return true;
        }

        protected override Task ExecuteAsync(object parameter)
        {
            _openFileService.Filter = "Text Files (*.csv)|*csv";

            _openFileService.IsMultiSelect = false;
            if (_openFileService.DetermineFile())
            {
                return _projectManager.LoadAsync(_openFileService.FileName);
            }

            return TaskHelper.Completed;
        }
    }
}