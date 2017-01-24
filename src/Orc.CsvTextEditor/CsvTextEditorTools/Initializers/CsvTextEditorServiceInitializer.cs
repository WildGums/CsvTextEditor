// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorServiceInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel;
    using Catel.IoC;
    using ICSharpCode.AvalonEdit;
    using Services;

    public class CsvTextEditorServiceInitializer : ICsvTextEditorServiceInitializer
    {
        #region Fields
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public CsvTextEditorServiceInitializer(ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            _typeFactory = typeFactory;
        }
        #endregion

        #region Methods
        public virtual void Initialize(TextEditor textEditor, ICsvTextEditorService csvTextEditorService)
        {
            Argument.IsNotNull(() => textEditor);
            Argument.IsNotNull(() => csvTextEditorService);

            var findReplaceTool = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<FindReplaceTextEditorTool>(textEditor, csvTextEditorService);

            csvTextEditorService.AddTool(findReplaceTool);
        }
        #endregion
    }
}