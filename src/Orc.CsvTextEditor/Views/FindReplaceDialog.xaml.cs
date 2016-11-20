// Code found at http://www.codeproject.com/Tips/768408/A-Find-and-Replace-tool-for-AvalonEdit

using System.Text.RegularExpressions;
using System.Windows;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;

namespace Orc.CsvTextEditor
{
    using System.Media;

    /// <summary>
    /// Interaction logic for FindReplaceDialog.xaml
    /// </summary>
    public partial class FindReplaceDialog : Window
    {
        private static string textToFind = "";
        private static bool caseSensitive = true;
        private static bool wholeWord = true;
        private static bool useRegex = false;
        private static bool useWildcards = false;
        private static bool searchUp = false;

        private TextEditor editor;

        public FindReplaceDialog(TextEditor editor)
        {            
            InitializeComponent();

            this.editor = editor;

            txtFind.Text = txtFind2.Text = textToFind;
            cbCaseSensitive.IsChecked = caseSensitive;
            cbWholeWord.IsChecked = wholeWord;
            cbRegex.IsChecked = useRegex;
            cbWildcards.IsChecked = useWildcards;
            cbSearchUp.IsChecked = searchUp;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            textToFind = txtFind2.Text;
            caseSensitive = (cbCaseSensitive.IsChecked == true);
            wholeWord = (cbWholeWord.IsChecked == true);
            useRegex = (cbRegex.IsChecked == true);
            useWildcards = (cbWildcards.IsChecked == true);
            searchUp = (cbSearchUp.IsChecked == true);

            theDialog = null;
        }

        private void FindNextClick(object sender, RoutedEventArgs e)
        {
            if (!FindNext(txtFind.Text))
                SystemSounds.Beep.Play();
        }

        private void FindNext2Click(object sender, RoutedEventArgs e)
        {
            if (!FindNext(txtFind2.Text))
                SystemSounds.Beep.Play();
        }

        private void ReplaceClick(object sender, RoutedEventArgs e)
        {
            Regex regex = GetRegEx(txtFind2.Text);
            string input = editor.Text.Substring(editor.SelectionStart, editor.SelectionLength);
            Match match = regex.Match(input);
            bool replaced = false;
            if (match.Success && match.Index == 0 && match.Length == input.Length)
            {
                editor.Document.Replace(editor.SelectionStart, editor.SelectionLength, txtReplace.Text);
                replaced = true;
            }

            if (!FindNext(txtFind2.Text) && !replaced)
                SystemSounds.Beep.Play();
        }

        private void ReplaceAllClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Replace All occurences of \"" + 
            txtFind2.Text + "\" with \"" + txtReplace.Text + "\"?",
                "Replace All", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Regex regex = GetRegEx(txtFind2.Text, true);
                int offset = 0;
                editor.BeginChange();
                foreach (Match match in regex.Matches(editor.Text))
                {
                    editor.Document.Replace(offset + match.Index, match.Length, txtReplace.Text);
                    offset += txtReplace.Text.Length - match.Length;
                }
                editor.EndChange();
            }
        }

        private bool FindNext(string textToFind)
        {
            Regex regex = GetRegEx(textToFind);
            int start = regex.Options.HasFlag(RegexOptions.RightToLeft) ? 
            editor.SelectionStart : editor.SelectionStart + editor.SelectionLength;
            Match match = regex.Match(editor.Text, start);

            if (!match.Success)  // start again from beginning or end
            {
                if (regex.Options.HasFlag(RegexOptions.RightToLeft))
                    match = regex.Match(editor.Text, editor.Text.Length);
                else
                    match = regex.Match(editor.Text, 0);
            }

            if (match.Success)
            {
                editor.Select(match.Index, match.Length);
                TextLocation loc = editor.Document.GetLocation(match.Index);
                editor.ScrollTo(loc.Line, loc.Column);
            }

            return match.Success;
        }

        private Regex GetRegEx(string textToFind, bool leftToRight = false)
        {
            RegexOptions options = RegexOptions.None;
            if (cbSearchUp.IsChecked == true && !leftToRight)
                options |= RegexOptions.RightToLeft;
            if (cbCaseSensitive.IsChecked == false)
                options |= RegexOptions.IgnoreCase;

            if (cbRegex.IsChecked == true)
            {
                return new Regex(textToFind, options);
            }
            else
            {
                string pattern = Regex.Escape(textToFind);
                if (cbWildcards.IsChecked == true)
                    pattern = pattern.Replace("\\*", ".*").Replace("\\?", ".");
                if (cbWholeWord.IsChecked == true)
                    pattern = "\\b" + pattern + "\\b";
                return new Regex(pattern, options);
            }
        }

        private static FindReplaceDialog theDialog = null;

        public static void ShowForReplace(TextEditor editor)
        {
            if (theDialog == null)
            {
                theDialog = new FindReplaceDialog(editor);
                theDialog.tabMain.SelectedIndex = 1;
                theDialog.Show();
                theDialog.Activate();
            }
            else
            {
                theDialog.tabMain.SelectedIndex = 1;
                theDialog.Activate();
            }

            if (!editor.TextArea.Selection.IsMultiline)
            {
                theDialog.txtFind.Text = theDialog.txtFind2.Text = editor.TextArea.Selection.GetText();
                theDialog.txtFind.SelectAll();
                theDialog.txtFind2.SelectAll();
                theDialog.txtFind2.Focus();
            }
        }
    }
} 