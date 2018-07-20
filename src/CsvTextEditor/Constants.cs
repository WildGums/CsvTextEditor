// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Orc.Squirrel;
    using System;
    using System.Collections.Immutable;
    using System.Windows.Input;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public static class Settings
    {
        public static class Application
        {
            public static class AutomaticUpdates
            {
                public const bool CheckForUpdatesDefaultValue = true;

                public static readonly ImmutableArray<UpdateChannel> AvailableChannels = ImmutableArray.Create(
                    new UpdateChannel("Stable", "http://downloads.sesolutions.net.au/csvtexteditor/stable"),
                    new UpdateChannel("Beta", "http://downloads.sesolutions.net.au/csvtexteditor/beta"),
                    new UpdateChannel("Alpha", "http://downloads.sesolutions.net.au/csvtexteditor/alpha")
                );

                public static readonly UpdateChannel DefaultChannel = AvailableChannels[0];
            }
        }
    }

    public static class Configuration
    {
        public const string CustomEditor = "Settings.Application.Editor.CustomEditor";

        public const string AutoSaveEditor = "Settings.Application.Editor.AutoSaveEditor";
        public const bool AutoSaveEditorDefaultValue = false;

        public const string AutoSaveInterval = "Settings.Application.Editor.AutoSaveInterval";
        public static readonly TimeSpan AutoSaveIntervalDefaultValue = TimeSpan.FromSeconds(60);
    }

    public static class Commands
    {
        public static class Help
        {
            public const string About = "Help.About";
            public static readonly InputGesture AboutInputGesture = null;
        }

        public static class File
        {
            public const string Close = "File.Close";
            public static readonly InputGesture CloseInputGesture = new InputGesture(Key.X, ModifierKeys.Alt);

            public const string Open = "File.Open";
            public static readonly InputGesture OpenInputGesture = new InputGesture(Key.O, ModifierKeys.Control);

            public const string OpenInTextEditor = "File.OpenInTextEditor";
            public static readonly InputGesture OpenInTextEditorInputGesture = null;

            public const string OpenInExcel = "File.OpenInExcel";
            public static readonly InputGesture OpenInExcelInputGesture = null;

            public const string Save = "File.Save";
            public static readonly InputGesture SaveInputGesture = new InputGesture(Key.S, ModifierKeys.Control);

            public const string SaveAs = "File.SaveAs";
            public static readonly InputGesture SaveAsInputGesture = null;
        }

        public static class Edit
        {
            public const string Undo = "Edit.Undo";
            //public static readonly InputGesture UndoInputGesture = new InputGesture(Key.Z, ModifierKeys.Control);

            public const string Redo = "Edit.Redo";
            //public static readonly InputGesture RedoInputGesture = new InputGesture(Key.Y, ModifierKeys.Control);

            public const string Copy = "Edit.Copy";
            //public static readonly InputGesture CopyInputGesture = new InputGesture(Key.C, ModifierKeys.Control);

            public const string Cut = "Edit.Cut";
            //public static readonly InputGesture CutInputGesture = new InputGesture(Key.X, ModifierKeys.Control);

            public const string Paste = "Edit.Paste";

            public const string FindReplace = "Edit.FindReplace";
            public static readonly InputGesture FindReplaceInputGesture = new InputGesture(Key.F, ModifierKeys.Control);

            public const string DuplicateLine = "Edit.DuplicateLine";
            public static readonly InputGesture DuplicateLineInputGesture = new InputGesture(Key.D, ModifierKeys.Control);

            public const string DeleteLine = "Edit.DeleteLine";
            public static readonly InputGesture DeleteLineInputGesture = new InputGesture(Key.L, ModifierKeys.Control);
            
            public const string RemoveBlankLines = "Edit.RemoveBlankLines";
            public static readonly InputGesture RemoveBlankLinesInputGesture = null;

            public const string RemoveDuplicateLines = "Edit.RemoveDuplicateLines";
            public static readonly InputGesture RemoveDuplicateLinesInputGesture = null;

            public const string TrimWhitespaces = "Edit.TrimWhitespaces";
            public static readonly InputGesture TrimWhitespacesInputGesture = null;

        }

        public static class Settings
        {
 
            public const string General = "Settings.General";
            public static readonly InputGesture GeneralInputGesture = new InputGesture(Key.S, ModifierKeys.Alt | ModifierKeys.Control);
        }
    }
}
