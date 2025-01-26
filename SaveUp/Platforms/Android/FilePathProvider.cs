using SaveUp.Services;
using System.IO;

namespace SaveUp.Platforms.Android
{
    /// <summary>
    /// Plattform-spezifische Implementierung des <see cref="IFilePathProvider"/>-Interfaces für Android.
    /// Diese Klasse stellt den Pfad zum Verzeichnis der Anwendungsdaten bereit, das speziell für die "SaveUp"-App verwendet wird.
    /// </summary>
    public class FilePathProvider : IFilePathProvider
    {
        /// <summary>
        /// Gibt das Verzeichnis für Anwendungsdaten zurück.
        /// Fügt außerdem den spezifischen Unterordner "SaveUp" hinzu, um App-spezifische Dateien zu speichern.
        /// </summary>
        /// <returns>Der vollständige Pfad zum Verzeichnis der Anwendungsdaten der "SaveUp"-App.</returns>
        public string GetAppDataDirectory()
        {
            // Abrufen des Standardpfads für lokale Anwendungsdaten
            var baseDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

            // Kombinieren des Basisverzeichnisses mit dem spezifischen Ordner "SaveUp"
            var appDataDirectory = Path.Combine(baseDirectory, "SaveUp");

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