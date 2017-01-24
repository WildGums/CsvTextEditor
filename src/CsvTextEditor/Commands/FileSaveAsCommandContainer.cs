// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSaveAsCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orc.ProjectManagement;

    public class FileSaveAsCommandContainer : ProjectCommandContainerBase
    {
        #region Fields
        private readonly ISaveFileService _saveFileService;
        #endregion

        #region Constructors
        public FileSaveAsCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ISaveFileService saveFileService)
            : base(Commands.File.SaveAs, commandManager, projectManager)
        {
            Argument.IsNotNull(() => saveFileService);

            _saveFileService = saveFileService;
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(object parameter)
        {
            var project = _projectManager.ActiveProject as Project;
            if (ReferenceEquals(project, null))
            {
                return;
            }

            _saveFileService.Filter = "Text Files (*.csv)|*csv";

            if (_saveFileService.DetermineFile())
            {
                await _projectManager.SaveAsync(project, _saveFileService.FileName);
            }

            await base.ExecuteAsync(parameter);
        }
        #endregion
    }
}