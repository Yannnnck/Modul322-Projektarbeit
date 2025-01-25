using SaveUp.Services;
using System;
using System.IO;

namespace SaveUp.Platforms.Windows
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetAppDataDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SaveUp");
        }
    }
}
