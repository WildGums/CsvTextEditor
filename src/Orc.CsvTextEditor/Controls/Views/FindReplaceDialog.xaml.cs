// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceDialog.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel.MVVM.Views;
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for FindReplaceDialog.xaml
    /// </summary>
    public partial class FindReplaceDialog
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