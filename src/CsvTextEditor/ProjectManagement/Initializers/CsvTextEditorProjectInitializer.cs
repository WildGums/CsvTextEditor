// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorProjectInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor.ProjectManagement
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Catel;
    using Catel.Services;
    using Orc.CommandLine;
    using Orc.ProjectManagement;

    public class CsvTextEditorProjectInitializer : FileProjectInitializer
    {
        private readonly IStartUpInfoProvider _startUpInfoProvider;
        private readonly ICommandLineParser _commandLineParser;

        public CsvTextEditorProjectInitializer(IStartUpInfoProvider startUpInfoProvider, ICommandLineParser commandLineParser) 
            : base(startUpInfoProvider)
        {
            Argument.IsNotNull(() => startUpInfoProvider);
            Argument.IsNotNull(() => commandLineParser);

            _startUpInfoProvider = startUpInfoProvider;
            _commandLineParser = commandLineParser;
        }

        public override IEnumerable<string> GetInitialLocations()
        {
            var commandLineContext = new CommandLineContext();
            _commandLineParser.Parse(_startUpInfoProvider.Arguments, commandLineContext);

            var initialFile = commandLineContext.InitialFile;
            if (string.IsNullOrWhiteSpace(initialFile) || !File.Exists(initialFile))
            {
                return Enumerable.Empty<string>();
            }

            return new[] { initialFile };
        }
    }
}