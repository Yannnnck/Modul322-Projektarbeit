using SaveUp.Services;
using System.Linq;
using Foundation;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(SaveUp.Platforms.iOS.FilePathProvider))]
namespace SaveUp.Platforms.iOS
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetAppDataDirectory()
        {
            return NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User).FirstOrDefault()?.Path ?? "";
        }
    }
}
