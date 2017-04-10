// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellActivatedActionQueue.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System;
    using Catel;
    using Orchestra;

    public class ShellActivatedActionQueue : ApplicationWatcherBase
    {
        public void EnqueueAction(Action action)
        {
            Argument.IsNotNull(() => action);

            EnqueueShellActivatedAction(w => action());
        }
    }
}