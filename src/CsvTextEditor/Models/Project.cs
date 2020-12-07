// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Project.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Models
{
    using System;
    using Orc.ProjectManagement;

    public sealed class Project : ProjectBase, IProject, IEquatable<Project>
    {
        #region Constructors
        public Project(string location)
            : base(location)
        {
        }

        public Project(string location, string title)
            : base(location, title)
        {
        }
        #endregion

        #region Properties
        public string Text { get; set; }
        #endregion

        #region Methods
        public bool Equals(Project other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Location, other.Location);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Project) obj);
        }

        public override int GetHashCode()
        {
            return (Location != null ? Location.GetHashCode() : 0);
        }
        #endregion

        public void SetIsDirty(bool isDirty)
        {
            if (isDirty)
            {
                MarkAsDirty();
            }
            else
            {
                ClearIsDirty();
            }
        }
    }
}
