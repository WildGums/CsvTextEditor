// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSaveCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Models;
    using Orc.ProjectManagement;

    public class FileSaveCommandContainer : ProjectCommandContainerBase
    {
        #region Constructors
        public FileSaveCommandContainer(ICommandManager commandManager, IProjectManager projectManager)
            : base(Commands.File.Save, commandManager, projectManager)
        {
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(object parameter)
        {
            await base.ExecuteAsync(parameter);

            var project = _projectManager.ActiveProject as Project;
            if (!ReferenceEquals(project, null))
            {
                await _projectManager.SaveAsync(project);
            }
        }
        #endregion
    }
}