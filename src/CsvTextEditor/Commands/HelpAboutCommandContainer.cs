// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpAboutCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class HelpAboutCommandContainer : CommandContainerBase
    {
        private readonly IProjectManager _projectManager;

        #region Constructors
        public HelpAboutCommandContainer(ICommandManager commandManager)
            : base(Commands.Help.About, commandManager)
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