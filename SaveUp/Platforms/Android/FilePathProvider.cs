using SaveUp.Services;
using System.IO;

namespace SaveUp.Platforms.Android
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetAppDataDirectory()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "SaveUp");
        }
    }
}