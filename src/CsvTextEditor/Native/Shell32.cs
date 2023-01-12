namespace CsvTextEditor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using Catel;
    using Catel.Logging;

    public class Shell32
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<long, string> ErrorCodes = new Dictionary<long, string>
        {
            {2, "The specified file was not found."},
            {3, "The specified path is invalid."},
            {5, "The specified file cannot be accessed."},
            {8, "The system is out of memory or resources."},
            {31, "There is no association for the specified file type with an executable file."}
        };
        #endregion

        #region Methods
        [DllImport("shell32.dll")]
        private static extern int FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);

        public static string FindExecutable(FileInfo fileInfo)
        {
            ArgumentNullException.ThrowIfNull(fileInfo);

            var fullName = fileInfo.FullName;

            Log.Debug("Searching executable to open file '{0}'", fullName);

            var result = string.Empty;

            try
            {
                var stringBuilder = new StringBuilder(1024);

                long errorCode = FindExecutable(fileInfo.Name, fileInfo.DirectoryName, stringBuilder);

                if (errorCode < 32)
                {
                    HandleError(fileInfo, errorCode);
                    result = string.Empty;
                }
                else
                {
                    result = stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to find executable to open file '{0}'", fullName);
            }

            Log.Debug(string.IsNullOrWhiteSpace(result)
                ? $"Failed to find executable to open file '{fullName}'"
                : $"Executable for open file '{fullName}' has been successfully found: '{result}'");

            return result;
        }

        private static void HandleError(FileInfo fileInfo, long errorCode)
        {
            ArgumentNullException.ThrowIfNull(fileInfo);

            if (!ErrorCodes.TryGetValue(errorCode, out var errorMessage))
            {
                errorMessage = $"Error: ({errorCode})";
            }

            Log.Error("Failed to find executable to open file {0}. {1}", fileInfo.FullName, errorMessage);
        }
        #endregion
    }
}