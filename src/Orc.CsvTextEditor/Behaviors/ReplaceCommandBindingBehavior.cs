// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceCommandBindingBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using ICSharpCode.AvalonEdit;

    public class ReplaceCommandBindingBehavior : Behavior<TextEditor>
    {

        public static readonly DependencyProperty ReplacementCommandProperty = DependencyProperty.Register(
            "ReplacementCommand", typeof (RoutedCommand), typeof (ReplaceCommandBindingBehavior), new PropertyMetadata(default(RoutedCommand)));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof (ICommand), typeof (ReplaceCommandBindingBehavior), new PropertyMetadata(default(ICommand), (o, args) => ((ReplaceCommandBindingBehavior) o).OnCommandPropertyChanged(args)));

        public RoutedCommand ReplacementCommand
        {
            get { return (RoutedCommand) GetValue(ReplacementCommandProperty); }
            set { SetValue(ReplacementCommandProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        
        private void OnCommandPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            var textArea = AssociatedObject?.TextArea;
            if (textArea == null)
            {
                return;
            }

            var commandBindings = textArea.CommandBindings;

            for (var i = 0; i < commandBindings.Count; i++)
            {
                var commandBinding = commandBindings[i];
                if (commandBinding.Command != ReplacementCommand)
                {
                    continue;
                }

                textArea.CommandBindings.Remove(commandBinding);
                textArea.CommandBindings.Add(new CommandBinding(ReplacementCommand, (sender, e) => Command?.Execute(null)));
                return;
            }
        }
    }
}