using SaveUp.Services;
using System;
using System.IO;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(SaveUp.Platforms.Android.FilePathProvider))]
namespace SaveUp.Platforms.Android
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetAppDataDirectory()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        }
    }
}
