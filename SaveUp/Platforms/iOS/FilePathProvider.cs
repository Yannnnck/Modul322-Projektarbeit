using SaveUp.Services;
using Foundation;
using System.Linq;

namespace SaveUp.Platforms.iOS
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetAppDataDirectory()
        {
            var path = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User)
                .FirstOrDefault()?.Path;

            if (path == null)
                throw new InvalidOperationException("Der AppData-Pfad konnte nicht abgerufen werden.");

            return Path.Combine(path, "SaveUp");
        }
    }
}
