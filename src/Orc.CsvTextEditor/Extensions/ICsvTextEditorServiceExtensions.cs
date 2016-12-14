// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using Services;

    public static class ICsvTextEditorServiceExtensions
    {
        public static void AddTool<T>(this ICsvTextEditorService csvTextEditorService)
            where T : ICsvTextEditorTool
        {
            Argument.IsNotNull(() => csvTextEditorService);

            var typeFactory = ServiceLocator.Default.ResolveType<ITypeFactory>();
            var tool = typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>();
            csvTextEditorService.AddTool(tool);
        }

        public static void RemoveToolByName(this ICsvTextEditorService csvTextEditorService, string toolName)
        {
            Argument.IsNotNull(() => csvTextEditorService);
            
            var tools = csvTextEditorService.Tools;

            var tool = tools.FirstOrDefault(x => x.Name == toolName);
            if (tool == null)
            {
                return;
            }

            csvTextEditorService.RemoveTool(tool);
        }
    }
}