// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceCommandBindingBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Runtime.InteropServices.ComTypes;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using ICSharpCode.AvalonEdit;

    public class ReplaceCommandBindingBehavior : Behavior<TextEditor>
    {
        #region Fields
        public static readonly DependencyProperty ReplacementCommandProperty = DependencyProperty.Register(
            "ReplacementCommand", typeof (RoutedCommand), typeof (ReplaceCommandBindingBehavior), new PropertyMetadata(default(RoutedCommand)));

        public static readonly DependencyProperty ExecutedProperty = DependencyProperty.Register(
            "Executed", typeof (ExecutedRoutedEventHandler), typeof (ReplaceCommandBindingBehavior), new PropertyMetadata(default(ExecutedRoutedEventHandler)));
        #endregion

        #region Properties
        public RoutedCommand ReplacementCommand
        {
            get { return (RoutedCommand) GetValue(ReplacementCommandProperty); }
            set { SetValue(ReplacementCommandProperty, value); }
        }

        public ExecutedRoutedEventHandler Executed
        {
            get { return (ExecutedRoutedEventHandler) GetValue(ExecutedProperty); }
            set { SetValue(ExecutedProperty, value); }
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            var textArea = AssociatedObject?.TextArea;
            if (textArea == null)
            {
                return;
            }

            var commandBindings = textArea.CommandBindings;

            for (var i = 0; i < commandBindings.Count; i++)
            {
                var commandBinding = commandBindings[i];
                if (commandBinding.Command == ReplacementCommand)
                {
                    textArea.CommandBindings.Remove(commandBinding);
                    textArea.CommandBindings.Add(new CommandBinding(ReplacementCommand, Executed));
                    return;
                }
            }

            var inputBindings = textArea.InputBindings;
            for (var i = 0; i < commandBindings.Count; i++)
            {
                var commandBinding = commandBindings[i];
                var routedCommand = commandBinding.Command as RoutedCommand;
                if (routedCommand == null)
                {
                    continue;
                }
                
                if (routedCommand.Name == "DeleteLine")
                {
                    var routedCommandKeyGestures = routedCommand.InputGestures;
                    if (routedCommand.InputGestures.Contains(ReplacementCommand.InputGestures[0]))
                    {
                        commandBindings.Remove(commandBinding);
                        textArea.CommandBindings.Add(new CommandBinding(ReplacementCommand, Executed));
                        return;
                    }

                }
            }
            
            //for (var i = 0; i < inputBindings.Count; i++)
            //{
            //    var inputBinding = inputBindings[i];
            //    if ((ReplacementCommand as RoutedCommand).InputGestures.Contains(inputBinding.Gesture))
            //    {
            //        textArea.InputBindings.Remove(inputBinding);
            //        textArea.InputBindings.Add(new InputBinding(ReplacementCommand, inputBinding.Gesture));
            //        return;
            //    }
            //}
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}