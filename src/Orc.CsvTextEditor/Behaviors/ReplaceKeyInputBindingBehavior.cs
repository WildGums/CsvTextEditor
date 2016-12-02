// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceKeyInputBindingBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using ICSharpCode.AvalonEdit;

    internal class ReplaceKeyInputBindingBehavior : Behavior<TextEditor>
    {
        private RoutedCommand _removedRoutedCommand;
        private KeyGesture _removedKeyGesture;
        private InputBinding _removedInputBinding;

        public KeyGesture Gesture
        {
            get { return (KeyGesture) GetValue(GestureProperty); }
            set { SetValue(GestureProperty, value); }
        }

        public static readonly DependencyProperty GestureProperty = DependencyProperty.Register(
            "Gesture", typeof(KeyGesture), typeof(ReplaceKeyInputBindingBehavior), new PropertyMetadata(default(KeyGesture)));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(ReplaceKeyInputBindingBehavior), new PropertyMetadata(default(ICommand), (o, args) => ((ReplaceKeyInputBindingBehavior)o).OnCommandPropertyChanged(args)));


        private void OnCommandPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            var textArea = AssociatedObject?.TextArea;
            if (textArea == null)
            {
                return;
            }

            var command = args.NewValue as ICommand;
            if (command == null)
            {
                return;
            }

            var commandBindings = textArea.CommandBindings;
            _removedRoutedCommand?.InputGestures.Add(_removedKeyGesture);

            for (var i = 0; i < commandBindings.Count; i++)
            {
                var commandBinding = commandBindings[i];

                var routedCommand = commandBinding.Command as RoutedCommand;
                var gesture = routedCommand?.InputGestures.OfType<KeyGesture>().FirstOrDefault(x => x.IsKeyAndModifierEquals(Gesture));
                if (gesture == null)
                {
                    continue;
                }

                routedCommand.InputGestures.Remove(gesture);

                _removedKeyGesture = gesture;
                _removedRoutedCommand = routedCommand;
                break;
            }

            var inputBindings = textArea.InputBindings;
            if (_removedInputBinding != null)
            {
                inputBindings.Add(_removedInputBinding);
            }

            for (var i = 0; i < inputBindings.Count; i++)
            {
                var inputBinding = inputBindings[i];
                var keyGesture = inputBinding.Gesture as KeyGesture;
                if (keyGesture == null)
                {
                    continue;
                }

                if (!keyGesture.IsKeyAndModifierEquals(Gesture))
                {
                    continue;
                }

                inputBindings.Remove(inputBinding);

                _removedInputBinding = inputBinding;
                break;
            }

            inputBindings.Add(new InputBinding(command, Gesture));
        }
    }
}