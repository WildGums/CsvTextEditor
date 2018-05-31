// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineContext.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor
{
    using Orc.CommandLine;

    public class CommandLineContext : ContextBase
    {
        [Option("", "", DisplayName = "initialFile", HelpText = "The initial file to open in CsvTextEditor")]
        public string InitialFile { get; set; }
    }
}
