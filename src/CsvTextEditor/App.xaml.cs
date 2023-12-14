namespace CsvTextEditor
{
    using System.Diagnostics;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Orchestra.Services;
    using Orchestra.Views;
    using Orc.Squirrel;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Stopwatch _stopwatch;

        public App()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

#pragma warning disable AvoidAsyncVoid // Avoid async void
        protected override async void OnStartup(StartupEventArgs e)
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            await SquirrelHelper.HandleSquirrelAutomaticallyAsync();

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            await shellService.CreateAsync<ShellWindow>();

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
        }
    }
}
