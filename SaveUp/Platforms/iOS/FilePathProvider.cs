using SaveUp.Services;
using Foundation;
using System.Linq;

namespace SaveUp.Platforms.iOS
{
    /// <summary>
    /// Plattform-spezifische Implementierung des <see cref="IFilePathProvider"/>-Interfaces für iOS.
    /// Diese Klasse stellt den Pfad zum Anwendungsunterstützungsverzeichnis bereit, das speziell für die "SaveUp"-App verwendet wird.
    /// </summary>
    public class FilePathProvider : IFilePathProvider
    {
        /// <summary>
        /// Gibt das Verzeichnis für Anwendungsunterstützungsdaten zurück.
        /// Fügt außerdem den spezifischen Unterordner "SaveUp" hinzu, um App-spezifische Dateien zu speichern.
        /// </summary>
        /// <returns>Der vollständige Pfad zum Verzeichnis der Anwendungsunterstützungsdaten der "SaveUp"-App.</returns>
        public string GetAppDataDirectory()
        {
            // Abrufen des Pfads zum Verzeichnis "ApplicationSupportDirectory" für den Benutzer
            var path = NSFileManager.DefaultManager
                .GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User)
                .FirstOrDefault()?.Path;

            // Falls der Pfad nicht abgerufen werden konnte, eine Ausnahme werfen
            if (path == null)
                throw new InvalidOperationException("Der AppData-Pfad konnte nicht abgerufen werden.");

            // Fügt den spezifischen Ordner "SaveUp" hinzu
            var appDataDirectory = Path.Combine(path, "SaveUp");

            // Überprüft, ob das Verzeichnis existiert, und erstellt es, falls es nicht existiert
            if (!Directory.Exists(appDataDirectory))
            {
                Directory.CreateDirectory(appDataDirectory);
            }

            // Gibt den vollständigen Pfad zurück
            return appDataDirectory;
        }
    }
}