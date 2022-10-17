namespace CsvTextEditor.ProjectManagement
{
    using System;
    using System.Threading.Tasks;
    using Orc.ProjectManagement;
    using Orchestra;
    using Orchestra.Services;

    public class RecentlyUsedItemsProjectWatcher : ProjectWatcherBase
    {
        private readonly IRecentlyUsedItemsService _recentlyUsedItemsService;

        public RecentlyUsedItemsProjectWatcher(IProjectManager projectManager, IRecentlyUsedItemsService recentlyUsedItemsService)
            : base(projectManager)
        {
            ArgumentNullException.ThrowIfNull(recentlyUsedItemsService);

            _recentlyUsedItemsService = recentlyUsedItemsService;
        }
        protected override Task OnLoadedAsync(IProject project)
        {
            _recentlyUsedItemsService.AddItem(new RecentlyUsedItem(project.Location, DateTime.Now));

            return base.OnLoadedAsync(project);
        }
    }
}
