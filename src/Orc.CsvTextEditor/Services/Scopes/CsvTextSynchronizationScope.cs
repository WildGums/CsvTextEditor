// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextSynchronizationScope.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System;
    using Catel;

    public class CsvTextSynchronizationScope : IDisposable
    {
        #region Fields
        private readonly CsvTextSynchronizationService _csvTextSynchronizationService;
        #endregion

        #region Constructors
        public CsvTextSynchronizationScope(CsvTextSynchronizationService csvTextSynchronizationService)
        {
            Argument.IsNotNull(() => csvTextSynchronizationService);

            _csvTextSynchronizationService = csvTextSynchronizationService;
            _csvTextSynchronizationService.IsSynchronizing = true;
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            _csvTextSynchronizationService.IsSynchronizing = false;
        }
        #endregion
    }
}