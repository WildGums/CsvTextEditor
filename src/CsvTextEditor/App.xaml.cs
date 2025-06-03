namespace CsvTextEditor
{
    using System.Diagnostics;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Orc.Squirrel;
    using Orchestra.Services;
    using Orchestra.Views;
    using Velopack;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Stopwatch _stopwatch;

        public App()
        {
            // Keep here, even though we have it in module initializer. But in case module
            // initializer is not called we still want to initialize velopack.
            VelopackApp.Build().Run();

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

#pragma warning disable CS0618 // Type or member is obsolete
            await SquirrelHelper.HandleSquirrelAutomaticallyAsync();
#pragma warning restore CS0618 // Type or member is obsolete

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            await shellService.CreateAsync<ShellWindow>();

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
        }
    }
}
