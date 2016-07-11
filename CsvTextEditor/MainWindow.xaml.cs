using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CsvTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _fileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateInBackgroundClick(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            textEditor.UpdateAllLinesInBackground = checkbox.IsChecked ?? false;
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".csv",
                Filter = "Text Files (*.csv)|*csv"
            };

            if (dialog.ShowDialog() == true)
            {
                Load(dialog.FileName);
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_fileName))
                Save(_fileName);
            else
                SaveAsClick(sender, e);
        }

        private void SaveAsClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = ".csv",
                Filter = "Text Files (*.csv)|*csv"
            };

            if (dialog.ShowDialog() == true)
            {
                Save(dialog.FileName);
            }
        }

        private void Load(string fileName)
        {
            _fileName = fileName;
            InitializeText(File.ReadAllLines(fileName));
        }

        private void Save(string fileName)
        {
            _fileName = fileName;
            File.WriteAllText(fileName, textEditor.Text);
        }

        private void InitializeText(string[] lines)
        {
            textEditor.Initialize(lines);
        }
    }
}
