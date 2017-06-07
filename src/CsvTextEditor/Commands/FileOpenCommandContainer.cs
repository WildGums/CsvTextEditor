// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOpenCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orc.ProjectManagement;

    public class FileOpenCommandContainer : ProjectCommandContainerBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IFileService _fileService;
        private readonly IOpenFileService _openFileService;
        private readonly IPleaseWaitService _pleaseWaitService;
        #endregion

        #region Constructors
        public FileOpenCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IOpenFileService openFileService,
            IFileService fileService, IPleaseWaitService pleaseWaitService)
            : base(Commands.File.Open, commandManager, projectManager)
        {
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => pleaseWaitService);

            _openFileService = openFileService;
            _fileService = fileService;
            _pleaseWaitService = pleaseWaitService;
        }
        #endregion

        #region Methods
        protected override bool CanExecute(object parameter)
        {
            return true;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var location = parameter as string;

                if (string.IsNullOrWhiteSpace(location) || !_fileService.Exists(location))
                {
                    _openFileService.Filter = "Text Files (*.csv)|*csv";

                    _openFileService.IsMultiSelect = false;
                    if (_openFileService.DetermineFile())
                    {
                        location = _openFileService.FileName;
                    }
                }

                if (!string.IsNullOrWhiteSpace(location))
                {
                    using (_pleaseWaitService.PushInScope())
                    {
                        await _projectManager.LoadAsync(location);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to open file");
            }
        }
        #endregion
    }
}