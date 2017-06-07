// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using Catel.ApiCop;
    using Catel.ApiCop.Listeners;
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
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Stopwatch _stopwatch;
        #endregion

        #region Constructors
        public App()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
        #endregion

        #region Methods
#pragma warning disable AvoidAsyncVoid // Avoid async void
        protected override async void OnStartup(StartupEventArgs e)
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            SquirrelHelper.HandleSquirrelAutomatically();

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            await shellService.CreateWithSplashAsync<ShellWindow>();

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
        }

        protected override void OnExit(ExitEventArgs e)
        {
#if DEBUG
            var apiCopListener = new ConsoleApiCopListener();
            ApiCopManager.AddListener(apiCopListener);
            ApiCopManager.WriteResults();
#endif
            base.OnExit(e);
        }
        #endregion
    }
}