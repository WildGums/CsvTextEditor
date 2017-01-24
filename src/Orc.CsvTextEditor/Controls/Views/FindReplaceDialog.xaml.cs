// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceDialog.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel.MVVM.Views;
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for FindReplaceDialog.xaml
    /// </summary>
    internal partial class FindReplaceDialog
    {
        #region Constructors
        static FindReplaceDialog()
        {
            typeof (FindReplaceDialog).AutoDetectViewPropertiesToSubscribe();
        }

        public FindReplaceDialog()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();
        }
        #endregion
    }
}