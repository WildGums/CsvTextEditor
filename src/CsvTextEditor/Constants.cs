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
    }
}