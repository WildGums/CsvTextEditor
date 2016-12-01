// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextSynchronizationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System;

    public class CsvTextSynchronizationService : ICsvTextSynchronizationService
    {
        #region Properties
        public bool IsSynchronizing { get; set; }
        public IDisposable SynchronizeInScope()
        {
            return new CsvTextSynchronizationScope(this);
        }
        #endregion
    }
}