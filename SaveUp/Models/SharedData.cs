using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using SaveUp.Models;
using SaveUp.Services;

namespace SaveUp.Services
{
    /// <summary>
    /// Singleton-Klasse, die die zentralisierte Verwaltung und Persistenz von Produktdaten für die "SaveUp"-App bereitstellt.
    /// </summary>
    public class SharedData
    {
        // Singleton-Instanz
        private static SharedData? _instance;

        /// <summary>
        /// Zugriff auf die Singleton-Instanz.
        /// </summary>
        public static SharedData Instance => _instance ??= new SharedData();

        /// <summary>
        /// Sammlung aller Produkte, die von der App verwaltet werden.
        /// Änderungen an dieser Sammlung werden automatisch in die Persistenz geschrieben.
        /// </summary>
        public ObservableCollection<Product> Products { get; } = new();

        /// <summary>
        /// Ereignis, das ausgelöst wird, wenn sich die Produkte ändern.
        /// </summary>
        public event EventHandler ProductsChanged = delegate { };

        // Pfad zur Datei, in der die Produktdaten gespeichert werden.
        private readonly string FilePath;

        /// <summary>
        /// Privater Konstruktor für die Singleton-Implementierung.
        /// Initialisiert die Dateiablage und lädt vorhandene Produktdaten.
        /// </summary>
        private SharedData()
        {
            // Initialisiere den Pfad zur Datenablage mithilfe von Dependency Injection.
            var services = IPlatformApplication.Current?.Services;
            if (services == null)
            {
                throw new InvalidOperationException("Die Dienste von IPlatformApplication sind nicht verfügbar.");
            }

            var filePathProvider = services.GetService<IFilePathProvider>();
            if (filePathProvider == null)
            {
                throw new InvalidOperationException("IFilePathProvider wurde nicht registriert.");
            }

            FilePath = Path.Combine(filePathProvider.GetAppDataDirectory(), "SavedProducts.json");

            // Sicherstellen, dass das Verzeichnis existiert
            var directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Produkte aus der Datei laden
            LoadProducts();

            // Überwache Änderungen an der Produktsammlung
            Products.CollectionChanged += (s, e) => OnProductsChanged();
        }

        /// <summary>
        /// Lädt gespeicherte Produkte aus der JSON-Datei in die Sammlung.
        /// </summary>
        private void LoadProducts()
        {
            // Prüfen, ob die Datei existiert
            if (!File.Exists(FilePath)) return;

            try
            {
                // JSON-Inhalt der Datei lesen
                var json = File.ReadAllText(FilePath);

                // Deserialisieren in die ObservableCollection<Product>
                var products = JsonSerializer.Deserialize<ObservableCollection<Product>>(json);
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON-Fehler beim Laden der Produkte: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unerwarteter Fehler beim Laden der Produkte: {ex.Message}");
            }
        }

        /// <summary>
        /// Speichert die aktuelle Produktsammlung in der JSON-Datei.
        /// </summary>
        private void SaveProducts()
        {
            try
            {
                // Serialize die Produktsammlung in JSON-Format
                var json = JsonSerializer.Serialize(Products, new JsonSerializerOptions
                {
                    WriteIndented = true // Macht die JSON-Datei lesbarer
                });

                // JSON-Daten in die Datei schreiben
                File.WriteAllText(FilePath, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Keine Berechtigung zum Speichern der Produkte: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unerwarteter Fehler beim Speichern der Produkte: {ex.Message}");
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Produktsammlung geändert wurde.
        /// Triggert das ProductsChanged-Ereignis und speichert die Änderungen.
        /// </summary>
        private void OnProductsChanged()
        {
            // Ereignis auslösen, um andere Teile der App über Änderungen zu informieren
            ProductsChanged.Invoke(this, EventArgs.Empty);

            // Änderungen in der Datei speichern
            SaveProducts();
        }
    }
}