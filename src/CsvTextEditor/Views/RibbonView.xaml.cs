// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Views
{
    using Orchestra;

    public partial class RibbonView
    {
        #region Constructors
        public RibbonView()
        {
            InitializeComponent();

            ribbon.AddAboutButton();
        }
        #endregion
    }
}