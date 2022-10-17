// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectSerializerSelector.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement
{
    using System;
    using Catel;
    using Catel.IoC;
    using Orc.ProjectManagement;

    internal class ProjectSerializerSelector : IProjectSerializerSelector
    {
        #region Fields
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public ProjectSerializerSelector(ITypeFactory typeFactory)
        {
            ArgumentNullException.ThrowIfNull(typeFactory);

            _typeFactory = typeFactory;
        }
        #endregion

        #region Methods
        public IProjectReader GetReader(string location)
        {
            return _typeFactory.CreateInstance<ProjectReader>();
        }

        public IProjectWriter GetWriter(string location)
        {
            return _typeFactory.CreateInstance<ProjectWriter>();
        }
        #endregion
    }
}