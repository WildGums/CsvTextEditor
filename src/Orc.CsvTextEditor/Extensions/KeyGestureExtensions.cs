// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyGestureExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Windows.Input;
    using Catel;

    public static class KeyGestureExtensions
    {
        public static bool IsKeyAndModifierEquals(this KeyGesture left, KeyGesture right)
        {
            Argument.IsNotNull(() => left);
            Argument.IsNotNull(() => right);

            return left.Key == right.Key && left.Modifiers == right.Modifiers;
        }
    }
}