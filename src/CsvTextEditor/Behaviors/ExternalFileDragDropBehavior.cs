namespace CsvTextEditor.Behaviors
{
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.Windows.Interactivity;
    using Orc.ProjectManagement;

    public partial class ExternalFileDragDropBehavior : BehaviorBase<FrameworkElement>
    {
        private readonly IProjectManager _projectManager;

        public ExternalFileDragDropBehavior(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            AssociatedObject.SetCurrentValue(UIElement.AllowDropProperty, true);
            AssociatedObject.PreviewDragEnter += OnPreviewDragEnter;
            AssociatedObject.DragOver += OnPreviewDragEnter;
            AssociatedObject.PreviewDrop += OnPreviewDrop;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            AssociatedObject.PreviewDragEnter -= OnPreviewDragEnter;
            AssociatedObject.DragOver -= OnPreviewDragEnter;
            AssociatedObject.PreviewDrop -= OnPreviewDrop;
        }

        private static void OnPreviewDragEnter(object sender, DragEventArgs e)
        {
            var isCorrect = true;

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                if (!(e.Data.GetData(DataFormats.FileDrop, true) is string[] fileNames) || fileNames.Length != 1)
                {
                    isCorrect = false;
                }
                else
                {
                    isCorrect = fileNames
                        .Select(x => new FileInfo(x))
                        .All(x => x.Exists && x.Extension.EqualsIgnoreCase(".csv"));
                }
            }

            e.Effects = isCorrect ? DragDropEffects.All : DragDropEffects.None;

            e.Handled = true;
        }

        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetData(DataFormats.FileDrop, true) is string[] fileNames))
            {
                return;
            }

            foreach (var fileName in fileNames)
            {
                _projectManager.LoadAsync(fileName);
            }

            e.Handled = true;
        }
    }
}
