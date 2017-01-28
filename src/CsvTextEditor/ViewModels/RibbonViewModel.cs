// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Orchestra.ViewModels;
    using System.Threading.Tasks;

    public class RibbonViewModel : ViewModelBase
    {
        #region Fields
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public RibbonViewModel(IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => uiVisualizerService);

            _uiVisualizerService = uiVisualizerService;

            ShowKeyboardMappings = new TaskCommand(OnShowKeyboardMappingsExecuteAsync);

            Title = AssemblyHelper.GetEntryAssembly().Title();
        }
        #endregion

        #region Commands
        public TaskCommand ShowKeyboardMappings { get; private set; }

        private async Task OnShowKeyboardMappingsExecuteAsync()
        {
            _uiVisualizerService.ShowDialog<KeyboardMappingsCustomizationViewModel>();
        }
        #endregion
    }
}