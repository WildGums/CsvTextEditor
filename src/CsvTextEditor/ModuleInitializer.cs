using Catel.IoC;
using CsvTextEditor.Services;
using Orchestra.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IRibbonService, RibbonService>();
        serviceLocator.RegisterType<IApplicationInitializationService, ApplicationInitializationService>();

        // ***** IMPORTANT NOTE *****
        //
        // Only register the shell services in the ModuleInitializer. All other types must be registered 
        // in the ApplicationInitializationService
    }
}