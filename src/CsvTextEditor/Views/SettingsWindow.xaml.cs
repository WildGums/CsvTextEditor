namespace CsvTextEditor.Views
{

    public partial class SettingsWindow
    {
        partial void OnInitializingComponent()
        {
            Mode = Catel.Windows.DataWindowMode.OkCancel;
        }
    }
}
