namespace CsvTextEditor
{
    using System;
    using Catel;
    using Orchestra;

    public class ShellActivatedActionQueue : ApplicationWatcherBase
    {
        public void EnqueueAction(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);

            EnqueueShellActivatedAction(w => action());
        }
    }
}