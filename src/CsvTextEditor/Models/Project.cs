namespace CsvTextEditor.Models
{
    using System;
    using Orc.ProjectManagement;

    public sealed class Project : ProjectBase, IProject, IEquatable<Project>
    {
        public Project(string location)
            : base(location)
        {
        }

        public Project(string location, string title)
            : base(location, title)
        {
        }

        public string EditorId { get; set; }

        public string Text { get; set; }

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
            return (Location is not null ? Location.GetHashCode() : 0);
        }

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
