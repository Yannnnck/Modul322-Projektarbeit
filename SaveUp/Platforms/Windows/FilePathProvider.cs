using SaveUp.Services;
using System;
using System.IO;

namespace SaveUp.Platforms.Windows
{
    /// <summary>
    /// Plattform-spezifische Implementierung des <see cref="IFilePathProvider"/>-Interfaces für Windows.
    /// Diese Klasse stellt den Pfad zum lokalen Anwendungsdatenverzeichnis bereit, das speziell für die "SaveUp"-App verwendet wird.
    /// </summary>
    public class FilePathProvider : IFilePathProvider
    {
        /// <summary>
        /// Gibt das Verzeichnis für lokale Anwendungsdaten zurück.
        /// Fügt außerdem den spezifischen Unterordner "SaveUp" hinzu, um die Dateien der App zu speichern.
        /// </summary>
        /// <returns>Der vollständige Pfad zum Verzeichnis der Anwendungsdaten der "SaveUp"-App.</returns>
        public string GetAppDataDirectory()
        {
            // Holt den Pfad zum lokalen Anwendungsdatenverzeichnis des aktuellen Benutzers
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Fügt den spezifischen Ordner "SaveUp" hinzu
            var appDataDirectory = Path.Combine(localAppData, "SaveUp");

            // Stellt sicher, dass das Verzeichnis existiert, falls es noch nicht erstellt wurde
            if (!Directory.Exists(appDataDirectory))
            {
                Directory.CreateDirectory(appDataDirectory);
            }

            // Gibt den vollständigen Pfad zurück
            return appDataDirectory;
        }
    }
}
