namespace CsvTextEditor.Services
{
    using System;
    using System.IO;
    using Catel;
    using Catel.Logging;
    using Catel.Reflection;
    using Microsoft.Extensions.Logging;
    using Orc.FileSystem;

    public class FileExtensionService : IFileExtensionService
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(FileExtensionService));

        private readonly IFileService _fileService;
        private readonly IDirectoryService _directoryService;

        public FileExtensionService(IFileService fileService, IDirectoryService directoryService)
        {
            _fileService = fileService;
            _directoryService = directoryService;
        }

        public string GetRegisteredTool(string extension)
        {
            Logger.LogDebug($"Searching for external tool for '{extension}' files");

            var tool = string.Empty;
            var tempDirectory = string.Empty;

            try
            {
                tempDirectory = CreateTemporaryDirectory();

                var fullPath = string.Empty;
                if (!string.IsNullOrWhiteSpace(tempDirectory))
                {
                    fullPath = CreateDummyFile(tempDirectory, extension);
                }

                if (!string.IsNullOrWhiteSpace(fullPath))
                {
                    tool = Shell32.FindExecutable(new FileInfo(fullPath));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to find external editor for '{extension}' files");
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(tempDirectory))
                {
                    DeleteDirectory(tempDirectory);
                }
            }

            return tool;
        }


        private bool DeleteDirectory(string fullName)
        {
            Argument.IsNotNullOrWhitespace(() => fullName);

            Logger.LogDebug($"Deleting directory '{fullName}'");

            try
            {
                if (_directoryService.Exists(fullName))
                {
                    _directoryService.Delete(fullName, true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, $"Failed to delete directory '{fullName}'");
            }

            return !_directoryService.Exists(fullName);
        }

        private string CreateDummyFile(string directory, string extension)
        {
            Logger.LogDebug($"Creating dummy file at '{directory}'");

            var fullPath = string.Empty;

            try
            {
                var dummyFile = $"dummy.{extension}";

                fullPath = Path.Combine(directory, dummyFile);
                _fileService.Create(fullPath).Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to create dummy file at '{directory}'");
            }

            return fullPath;
        }

        private string CreateTemporaryDirectory()
        {
            Logger.LogDebug("Creating temporary directory");

            var tempDirectory = string.Empty;

            try
            {
                var assembly = AssemblyHelper.GetEntryAssembly();

                tempDirectory = Path.Combine(Path.GetTempPath(), assembly.Company(), assembly.Title(),
                    "ExternalEditor", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                _directoryService.Create(tempDirectory);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create temporary directory");
            }

            return tempDirectory;
        }
    }
}
