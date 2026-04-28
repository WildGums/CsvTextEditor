namespace CsvTextEditor
{
    using System;
    using Catel.Services;
    using Orchestra;

    public class ShellActivatedActionQueue : ApplicationWatcherBase
    {
        public ShellActivatedActionQueue(IDispatcherService dispatcherService, IMainWindowService mainWindowService)
            : base(dispatcherService, mainWindowService)
        {
            
        }

        public void EnqueueAction(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);

            EnqueueShellActivatedAction(w => action());
        }
    }
}
