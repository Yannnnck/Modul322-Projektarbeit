using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SaveUp.Models;
using System.Text.Json;
using SaveUp.Services;

namespace SaveUp.Services
{
    public class SharedData
    {
        private static SharedData? _instance;
        public static SharedData Instance => _instance ??= new SharedData();

        public ObservableCollection<Product> Products { get; } = new();

        public event EventHandler ProductsChanged = delegate { };

        private static readonly string FilePath;

        static SharedData()
        {
            var filePathProvider = Microsoft.Maui.Controls.DependencyService.Get<IFilePathProvider>();
            FilePath = Path.Combine(filePathProvider.GetAppDataDirectory(), "SavedProducts.json");
        }

        private SharedData()
        {
            LoadProducts();
            Products.CollectionChanged += (s, e) => OnProductsChanged();
        }

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
                    Console.WriteLine($"Fehler beim Laden der Produkte: {ex.Message}");
                }
            }
        }

        private void SaveProducts()
        {
            try
            {
                var json = JsonSerializer.Serialize(Products);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Produkte: {ex.Message}");
            }
        }

        private void OnProductsChanged()
        {
            ProductsChanged.Invoke(this, EventArgs.Empty);
            SaveProducts();
        }
    }
}
