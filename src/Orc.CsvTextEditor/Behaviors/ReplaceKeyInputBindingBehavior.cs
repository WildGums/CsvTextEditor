// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceInputBindingBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using Catel.MVVM;
    using ICSharpCode.AvalonEdit;

    public class ReplaceKeyInputBindingBehavior : Behavior<TextEditor>
    {
        public static readonly DependencyProperty GestureProperty = DependencyProperty.Register(
            "Gesture", typeof (KeyGesture), typeof (ReplaceKeyInputBindingBehavior), new PropertyMetadata(default(KeyGesture)));

        public KeyGesture Gesture
        {
            get { return (KeyGesture) GetValue(GestureProperty); }
            set { SetValue(GestureProperty, value); }
        }

        public static readonly DependencyProperty ExecutedProperty = DependencyProperty.Register(
            "Executed", typeof (ExecutedRoutedEventHandler), typeof (ReplaceKeyInputBindingBehavior), new PropertyMetadata(default(ExecutedRoutedEventHandler)));

        public ExecutedRoutedEventHandler Executed
        {
            get { return (ExecutedRoutedEventHandler) GetValue(ExecutedProperty); }
            set { SetValue(ExecutedProperty, value); }
        }

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

                var routedCommand = commandBinding.Command as RoutedCommand;
                var gesture = routedCommand?.InputGestures.OfType<KeyGesture>().FirstOrDefault(x => x.IsKeyAndModifierEquals(Gesture));
                if (gesture == null)
                {
                    continue;
                }

                routedCommand.InputGestures.Remove(gesture);
                break;
            }

            var inputBindings = textArea.InputBindings;
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
                break;
            }

            inputBindings.Add(new InputBinding(new Command(() => Executed(this, null)), Gesture));
        }
    }
}