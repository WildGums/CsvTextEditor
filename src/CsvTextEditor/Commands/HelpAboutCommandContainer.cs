namespace CsvTextEditor
{
    using Catel.MVVM;

    public class HelpAboutCommandContainer : CommandContainerBase
    {
        #region Constructors
        public HelpAboutCommandContainer(ICommandManager commandManager)
            : base(Commands.Help.About, commandManager)
        {
        }
        #endregion
    }
}
