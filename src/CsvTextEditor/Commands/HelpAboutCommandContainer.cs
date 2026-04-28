namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;

    public class HelpAboutCommandContainer : CommandContainerBase
    {
        public HelpAboutCommandContainer(ICommandManager commandManager, IServiceProvider serviceProvider)
            : base(Commands.Help.About, commandManager, serviceProvider)
        {
        }
    }
}
