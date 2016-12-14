// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Orc.CsvTextEditor;
using Orc.CsvTextEditor.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    #region Methods
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<ICsvTextEditorService, CsvTextEditorService>();
        serviceLocator.RegisterType<ICsvTextSynchronizationService, CsvTextSynchronizationService>();

        var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
        viewModelLocator.Register<FindReplaceDialog, FindReplaceDialogViewModel>();
        viewModelLocator.Register<CsvTextEditorControl, CsvTextEditorControlViewModel>();

        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
        uiVisualizerService.Register(typeof(FindReplaceDialogViewModel), typeof(FindReplaceDialog));
    }
    #endregion
}