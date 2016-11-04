// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Catel.IoC;
using Orc.CsvTextEditor;
using Orc.CsvTextEditor.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    #region Methods
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IPostprocessorProvider, PostprocessorProvider>();
        serviceLocator.RegisterType<ICsvTextEditorService, CsvTextEditorService>();
    }
    #endregion
}