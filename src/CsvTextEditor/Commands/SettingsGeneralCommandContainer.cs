namespace CsvTextEditor
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Microsoft.Extensions.Logging;

    public class SettingsGeneralCommandContainer : CommandContainerBase
    {
        private const string ViewModelType = "SettingsViewModel";

        private static readonly ILogger Logger = LogManager.GetLogger(typeof(SettingsGeneralCommandContainer));

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;

        public SettingsGeneralCommandContainer(ICommandManager commandManager, IUIVisualizerService uiVisualizerService, 
            IViewModelFactory viewModelFactory, IServiceProvider serviceProvider)
            : base(Commands.Settings.General, commandManager, serviceProvider)
        {
            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            var settingsViewModelType = TypeCache.GetTypes(x => string.Equals(x.Name, ViewModelType)).FirstOrDefault();
            if (settingsViewModelType is null)
            {
                throw Logger.LogErrorAndCreateException<InvalidOperationException>("Cannot find type '{0}'", ViewModelType);
            }

            var viewModel = _viewModelFactory.CreateViewModel(settingsViewModelType, null, null);

            await _uiVisualizerService.ShowDialogAsync(viewModel);
        }
    }
}
