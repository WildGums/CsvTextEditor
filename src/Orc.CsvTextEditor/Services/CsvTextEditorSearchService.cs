// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorSearchService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using Catel;
    using Controls;
    using ICSharpCode.AvalonEdit;

    public class CsvTextEditorSearchService : ICsvTextEditorSearchService
    {
        #region Fields
        private readonly TextEditor _textEditor;
        #endregion

        #region Constructors
        public CsvTextEditorSearchService(TextEditor textEditor)
        {
            Argument.IsNotNull(() => textEditor);

            _textEditor = textEditor;
        }
        #endregion

        #region Methods
        public bool FindNext(string textToFind, FindReplaceSettings settings)
        {
            var regex = GetRegEx(textToFind, settings);
            var start = regex.Options.HasFlag(RegexOptions.RightToLeft) ?
                _textEditor.SelectionStart : _textEditor.SelectionStart + _textEditor.SelectionLength;
            var match = regex.Match(_textEditor.Text, start);

            if (!match.Success) // start again from beginning or end
            {
                match = regex.Match(_textEditor.Text, regex.Options.HasFlag(RegexOptions.RightToLeft) ? _textEditor.Text.Length : 0);
            }

            if (!match.Success)
            {
                return false;
            }

            _textEditor.Select(match.Index, match.Length);
            var loc = _textEditor.Document.GetLocation(match.Index);
            _textEditor.ScrollTo(loc.Line, loc.Column);

            return match.Success;
        }

        public bool Replace(string textToFind, string textToReplace, FindReplaceSettings settings)
        {
            var regex = GetRegEx(textToFind, settings);
            var input = _textEditor.Text.Substring(_textEditor.SelectionStart, _textEditor.SelectionLength);
            var match = regex.Match(input);

            if (!match.Success || match.Index != 0 || match.Length != input.Length)
            {
                return FindNext(textToFind, settings);
            }

            _textEditor.Document.Replace(_textEditor.SelectionStart, _textEditor.SelectionLength, textToReplace);

            return true;
        }

        public void ReplaceAll(string textToFind, string textToReplace, FindReplaceSettings settings)
        {
            if (MessageBox.Show($"Are you sure you want to Replace All occurences of \"{textToFind}\" with \"{textToReplace}\"?",
                "Replace All", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var regex = GetRegEx(textToFind, settings, true);
            var offset = 0;

            _textEditor.BeginChange();
            foreach (Match match in regex.Matches(_textEditor.Text))
            {
                _textEditor.Document.Replace(offset + match.Index, match.Length, textToReplace);
                offset += textToReplace.Length - match.Length;
            }
            _textEditor.EndChange();
        }
        #endregion

        private Regex GetRegEx(string textToFind, FindReplaceSettings settings, bool isLeftToRight = false)
        {
            var options = RegexOptions.None;
            if (settings.IsSearchUp && !isLeftToRight)
            {
                options |= RegexOptions.RightToLeft;
            }

            if (!settings.CaseSensitive)
            {
                options |= RegexOptions.IgnoreCase;
            }

            if (settings.UseRegex)
            {
                return new Regex(textToFind, options);
            }

            var pattern = Regex.Escape(textToFind);
            if (settings.UseWildcards)
            {
                pattern = pattern.Replace("\\*", ".*").Replace("\\?", ".");
            }

            if (settings.WholeWord)
            {
                pattern = "\\b" + pattern + "\\b";
            }

            return new Regex(pattern, options);
        }
    }
}