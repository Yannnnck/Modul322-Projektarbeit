using SaveUp.Services;
using System;
using System.IO;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(SaveUp.Platforms.Windows.FilePathProvider))]
namespace SaveUp.Platforms.Windows
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetAppDataDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SaveUp");
        }
    }
}
