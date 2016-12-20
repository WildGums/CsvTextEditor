// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Linq;
    using Catel;
    using Services;

    public static class ICsvTextEditorServiceExtensions
    {
        public static void ShowTool<T>(this ICsvTextEditorService csvTextEditorService)
            where T : ICsvTextEditorTool
        {
            Argument.IsNotNull(() => csvTextEditorService);

            var tool = csvTextEditorService.Tools.OfType<T>().FirstOrDefault();
            tool?.Open();
        }

        public static void ShowTool(this ICsvTextEditorService csvTextEditorService, string toolName)
        {
            Argument.IsNotNull(() => csvTextEditorService);

            var tool = csvTextEditorService.GetToolByName(toolName);

            tool?.Open();
        }

        public static ICsvTextEditorTool GetToolByName(this ICsvTextEditorService csvTextEditorService, string toolName)
        {
            Argument.IsNotNull(() => csvTextEditorService);

            var tools = csvTextEditorService.Tools;
            return tools.FirstOrDefault(x => x.Name == toolName);
        }
    }
}