// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Project.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor.Models
{
    using System.Collections.Generic;
    using Orc.ProjectManagement;

    public class Project : ProjectBase, IProject
    {
        public Project(string location) 
            : base(location)
        {
        }

        public Project(string location, string title) 
            : base(location, title)
        {
        }

        public string Text { get; set; }
    }
}