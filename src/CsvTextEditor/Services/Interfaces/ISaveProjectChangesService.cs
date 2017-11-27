// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISaveProjectChangesService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor.Services
{
    using System.Threading.Tasks;
    using Models;
    using Orc.ProjectManagement;

    public interface ISaveProjectChangesService
    {
        Task<bool> EnsureChangesSavedAsync(Project project, SaveChangesReason reason);
    }
}