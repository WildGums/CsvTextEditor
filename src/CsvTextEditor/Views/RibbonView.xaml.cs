namespace CsvTextEditor.Views
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orchestra;

    public partial class RibbonView
    {
        public RibbonView(IServiceProvider serviceProvider, IViewModelWrapperService viewModelWrapperService, 
            IDataContextSubscriptionService dataContextSubscriptionService, IAboutService aboutService)
            : base(serviceProvider, viewModelWrapperService, dataContextSubscriptionService)
        {
            InitializeComponent();

            ribbon.AddAboutButton(aboutService);
        }
    }
}
