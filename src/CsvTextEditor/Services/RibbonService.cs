// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Services
{
    using System.Windows;
    using Orchestra.Services;
    using Views;

    public class RibbonService : IRibbonService
    {
        #region IRibbonService Members
        public FrameworkElement GetRibbon()
        {
            return new RibbonView();
        }

        public FrameworkElement GetMainView()
        {
            return new MainView();
        }

        public FrameworkElement GetStatusBar()
        {
            return new StatusBarView();
        }
        #endregion
    }
}