using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SaveUp.Models;
using System.Text.Json;

namespace SaveUp.Services
{
    public class SharedData
    {
        private static SharedData? _instance;
        public static SharedData Instance => _instance ??= new SharedData();

        public ObservableCollection<Product> Products { get; } = new();

        private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "SavedProducts.json");

        // Privater Konstruktor für Singleton-Pattern
        private SharedData()
        {
            LoadProducts();
            Products.CollectionChanged += (s, e) => SaveProducts();
        }

        // Lädt gespeicherte Produkte aus der JSON-Datei
        private void LoadProducts()
        {
            if (File.Exists(FilePath))
            {
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
                catch (Exception ex)
                {
                    // Fehler beim Lesen oder Deserialisieren abfangen
                    Console.WriteLine($"Fehler beim Laden der Produkte: {ex.Message}");
                }
            }
        }

        // Speichert die Produkte in der JSON-Datei
        private void SaveProducts()
        {
            try
            {
                var json = JsonSerializer.Serialize(Products);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                // Fehler beim Schreiben der Datei abfangen
                Console.WriteLine($"Fehler beim Speichern der Produkte: {ex.Message}");
            }
        }
    }
}