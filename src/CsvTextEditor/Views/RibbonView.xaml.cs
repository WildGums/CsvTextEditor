namespace CsvTextEditor.Views
{
    using Orchestra;

    public partial class RibbonView
    {
        #region Constructors
        public RibbonView()
        {
            InitializeComponent();

            ribbon.AddAboutButton();
        }
        #endregion
    }
}