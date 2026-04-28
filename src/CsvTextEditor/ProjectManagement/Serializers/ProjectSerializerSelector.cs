namespace CsvTextEditor.ProjectManagement
{
    using Orc.ProjectManagement;

    internal class ProjectSerializerSelector : IProjectSerializerSelector
    {
        private readonly IProjectReader _projectReader;
        private readonly IProjectWriter _projectWriter;

        public ProjectSerializerSelector(IProjectReader projectReader, IProjectWriter projectWriter)
        {
            _projectReader = projectReader;
            _projectWriter = projectWriter;
        }

        public IProjectReader GetReader(string location)
        {
            //return _typeFactory.CreateInstance<ProjectReader>();
            return _projectReader;
        }

        public IProjectWriter GetWriter(string location)
        {
            //return _typeFactory.CreateInstance<ProjectWriter>();
            return _projectWriter;
        }
    }
}
