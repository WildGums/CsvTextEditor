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

    public class HelpAboutCommandContainer : CommandContainerBase
    {
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