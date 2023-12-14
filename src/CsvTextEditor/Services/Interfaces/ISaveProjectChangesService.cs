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