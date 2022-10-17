namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.MVVM;
    using Orc.Theming;

    public class WindowCommandsViewModel : ViewModelBase
    {
        private readonly IBaseColorSchemeService _baseColorSchemeService;
        private readonly IConfigurationService _configurationService;

        public WindowCommandsViewModel(IBaseColorSchemeService baseColorSchemeService, IConfigurationService configurationService)
        {
            ArgumentNullException.ThrowIfNull(baseColorSchemeService);
            ArgumentNullException.ThrowIfNull(configurationService);

            _baseColorSchemeService = baseColorSchemeService;
            _configurationService = configurationService;

            SwitchTheme = new Command(OnSwitchThemeExecute);
        }

        public bool IsInDarkMode { get; private set; }

        public Command SwitchTheme { get; private set; }

        private void OnSwitchThemeExecute()
        {
            var availableSchemes = _baseColorSchemeService.GetAvailableBaseColorSchemes();
            if (availableSchemes.Count <= 1)
            {
                return;
            }

            var currentScheme = _baseColorSchemeService.GetBaseColorScheme();
            var index = (availableSchemes[0] == currentScheme) ? 1 : 0;

            _baseColorSchemeService.SetBaseColorScheme(availableSchemes[index]);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _baseColorSchemeService.BaseColorSchemeChanged += OnBaseColorSchemeServiceBaseColorSchemeChanged;

            var value = _configurationService.GetRoamingValue(Settings.Application.General.ThemeBaseColor, Settings.Application.General.ThemeBaseColorDefaultValue);

            var availableValues = _baseColorSchemeService.GetAvailableBaseColorSchemes().ToList();
            if (availableValues.Contains(value))
            {
                _baseColorSchemeService.SetBaseColorScheme(value);
            }

            UpdateState();
        }

        protected override async Task CloseAsync()
        {
            _baseColorSchemeService.BaseColorSchemeChanged -= OnBaseColorSchemeServiceBaseColorSchemeChanged;

            await base.CloseAsync();
        }

        private void OnBaseColorSchemeServiceBaseColorSchemeChanged(object sender, EventArgs e)
        {
            var baseColorScheme = _baseColorSchemeService.GetBaseColorScheme();

            _configurationService.SetRoamingValue(Settings.Application.General.ThemeBaseColor, baseColorScheme);

            UpdateState();
        }

        private void UpdateState()
        {
            var baseColorScheme = _baseColorSchemeService.GetBaseColorScheme();

            IsInDarkMode = baseColorScheme == "Dark";
        }
    }
}
