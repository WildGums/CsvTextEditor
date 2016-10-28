// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusBarViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orchestra;

    public class StatusBarViewModel : ViewModelBase
    {
        #region Constructors
        public StatusBarViewModel()
        {
        }
        #endregion

        #region Properties
        public string Version { get; private set; }
        #endregion

        #region Methods
        protected override Task InitializeAsync()
        {
            Version = VersionHelper.GetCurrentVersion();

            return base.InitializeAsync();
        }
        #endregion
    }
}