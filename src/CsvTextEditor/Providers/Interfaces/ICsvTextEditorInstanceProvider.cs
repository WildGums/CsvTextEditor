// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorInstanceProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using Models;
    using Orc.CsvTextEditor;

    public interface ICsvTextEditorInstanceProvider
    {
        #region Methods
        ICsvTextEditorInstance GetInstance(Project project);
        #endregion
    }
}
