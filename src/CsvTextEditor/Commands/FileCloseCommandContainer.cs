// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileCloseCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Base;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class FileCloseCommandContainer : ProjectCommandContainerBase
    {
        #region Constructors
        public FileCloseCommandContainer(ICommandManager commandManager, IProjectManager projectManager)
            : base(Commands.File.Close, commandManager, projectManager)
        {
        }
        #endregion

        #region Methods
        protected override Task ExecuteAsync(object parameter)
        {
            return base.ExecuteAsync(parameter);
        }
        #endregion
    }
}