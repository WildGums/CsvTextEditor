namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orchestra;

    public class RecentlyUsedItemsViewModel : ViewModelBase
    {
        private readonly IRecentlyUsedItemsService _recentlyUsedItemsService;
        private readonly IFileService _fileService;
        private readonly IMessageService _messageService;
        private readonly IProcessService _processService;

        public RecentlyUsedItemsViewModel(IRecentlyUsedItemsService recentlyUsedItemsService, 
            IFileService fileService, IMessageService messageService, 
            IProcessService processService, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _recentlyUsedItemsService = recentlyUsedItemsService;
            _fileService = fileService;
            _messageService = messageService;
            _processService = processService;

            PinItem = new Command<string>(serviceProvider, OnPinItemExecute);
            UnpinItem = new Command<string>(serviceProvider, OnUnpinItemExecute);
            OpenInExplorer = new TaskCommand<string>(serviceProvider, OnOpenInExplorerExecuteAsync);
        }

        public List<RecentlyUsedItem> RecentlyUsedItems { get; private set; }
        public List<RecentlyUsedItem> PinnedItems { get; private set; }

        public Command<string> PinItem { get; private set; }

        private void OnPinItemExecute(string parameter)
        {
            Argument.IsNotNullOrWhitespace(() => parameter);

            _recentlyUsedItemsService.PinItem(parameter);
        }

        public Command<string> UnpinItem { get; private set; }

        private void OnUnpinItemExecute(string parameter)
        {
            Argument.IsNotNullOrWhitespace(() => parameter);

            _recentlyUsedItemsService.UnpinItem(parameter);
        }

        public TaskCommand<string> OpenInExplorer { get; private set; }

        private async Task OnOpenInExplorerExecuteAsync(string parameter)
        {
            if (!_fileService.Exists(parameter))
            {
                await _messageService.ShowWarningAsync("The file doesn't seem to exist. Cannot open it in explorer.");
                return;
            }
          
            _processService.StartProcess("explorer.exe", $"/select, \"{parameter}\"");
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _recentlyUsedItemsService.Updated += OnRecentlyUsedItemsServiceUpdated;

            UpdateRecentlyUsedItems();
            UpdatePinnedItem();
        }

        protected override Task CloseAsync()
        {
            _recentlyUsedItemsService.Updated -= OnRecentlyUsedItemsServiceUpdated;

            return base.CloseAsync();
        }

        private void OnRecentlyUsedItemsServiceUpdated(object sender, EventArgs e)
        {
            UpdateRecentlyUsedItems();
            UpdatePinnedItem();
        }

        private void UpdateRecentlyUsedItems()
        {
            RecentlyUsedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.Items);            
        }

        private void UpdatePinnedItem()
        {
            PinnedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.PinnedItems);
        }
    }
}
