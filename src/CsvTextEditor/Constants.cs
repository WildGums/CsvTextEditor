// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Windows.Input;
    using InputGesture = Catel.Windows.Input.InputGesture;

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
            //public static readonly InputGesture FindReplaceInputGesture = new InputGesture(Key.F, ModifierKeys.Control);

            public const string DuplicateLine = "Edit.DuplicateLine";
            public static readonly InputGesture DuplicateLineInputGesture = new InputGesture(Key.D, ModifierKeys.Control);

            public const string DeleteLine = "Edit.DeleteLine";
            public static readonly InputGesture DeleteLineInputGesture = new InputGesture(Key.L, ModifierKeys.Control);
        }
    }
}