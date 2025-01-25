using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using SaveUp.Models;
using SaveUp.Services;

namespace SaveUp.Services
{
    public class SharedData
    {
        private static SharedData? _instance;
        public static SharedData Instance => _instance ??= new SharedData();

        public ObservableCollection<Product> Products { get; } = new();

        public event EventHandler ProductsChanged = delegate { };

        private readonly string FilePath;

        private SharedData()
        {
            // Initialisiere FilePath über Dependency Injection
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

            // Stelle sicher, dass das Verzeichnis existiert
            var directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            LoadProducts();
            Products.CollectionChanged += (s, e) => OnProductsChanged();
        }

        private void LoadProducts()
        {
            if (!File.Exists(FilePath)) return;

            try
            {
                var json = File.ReadAllText(FilePath);
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

        private void SaveProducts()
        {
            try
            {
                var json = JsonSerializer.Serialize(Products, new JsonSerializerOptions
                {
                    WriteIndented = true // Macht die JSON-Datei lesbarer
                });
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

        private void OnProductsChanged()
        {
            ProductsChanged.Invoke(this, EventArgs.Empty);
            SaveProducts();
        }
    }
}