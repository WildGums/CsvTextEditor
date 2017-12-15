// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileExtensionService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CsvTextEditor.Services
{
    public interface IFileExtensionService
    {
        string GetRegisteredTool(string extension);
    }
}