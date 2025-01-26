using SaveUp.Models;
using SaveUp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;

namespace SaveUp.ViewModels
{
    public class ListPageViewModel : BaseViewModel
    {
        // Titel der Seite, der in der Benutzeroberfläche angezeigt wird
        private string _pageTitle = "List Page";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value); // Aktualisiert die UI bei Änderungen
        }

        // Suchtext für die Filterung der Produkte
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value); // Aktualisiert den Wert
                FilterProducts(); // Filtert die Produkte basierend auf dem Suchtext
            }
        }

        // Startdatum für die Filterung
        private DateTime _startDate = DateTime.Today.AddDays(-30);
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value); // Aktualisiert den Wert
                FilterProducts(); // Filtert die Produkte basierend auf dem Datum
            }
        }

        // Enddatum für die Filterung
        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value); // Aktualisiert den Wert
                FilterProducts(); // Filtert die Produkte basierend auf dem Datum
            }
        }

        // Gesamtliste der Produkte
        public ObservableCollection<Product> Products => SharedData.Instance.Products;

        // Gefilterte Liste der Produkte
        private ObservableCollection<Product> _filteredProducts = new();
        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set => SetProperty(ref _filteredProducts, value); // Aktualisiert die UI bei Änderungen
        }

        // Gesamtsumme der gefilterten Produkte
        private decimal _totalSum;
        public decimal TotalSum
        {
            get => _totalSum;
            set => SetProperty(ref _totalSum, value); // Aktualisiert die UI bei Änderungen
        }

        // Befehle für verschiedene Aktionen
        public ICommand ClearListCommand { get; }
        public ICommand SortByDateCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand DeleteProductWithAnimationCommand { get; }

        // Konstruktor zur Initialisierung der ViewModel-Logik
        public ListPageViewModel()
        {
            // Initialisierung der gefilterten Produkte
            FilteredProducts = new ObservableCollection<Product>(Products);

            // Befehle verknüpfen
            ClearListCommand = new Command(ClearList);
            SortByDateCommand = new Command(SortByDate);
            DeleteProductCommand = new Command<Product>(DeleteProduct);
            DeleteProductWithAnimationCommand = new Command<Product>(async (product) => await DeleteProductWithAnimation(product));

            // Überwacht Änderungen in der Produktliste
            Products.CollectionChanged += (s, e) => FilterProducts();

            // Initiale Filterung der Produkte
            FilterProducts();
        }

        // Filtert die Produkte basierend auf Suchtext und Datum
        private void FilterProducts()
        {
            var filtered = Products
                .Where(p =>
                    (string.IsNullOrWhiteSpace(SearchText) || p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) &&
                    p.DateAdded.Date >= StartDate.Date &&
                    p.DateAdded.Date <= EndDate.Date)
                .OrderBy(p => p.DateAdded) // Sortierung nach Datum
                .ToList();

            FilteredProducts = new ObservableCollection<Product>(filtered);
            CalculateTotalSum(); // Berechnet die Gesamtsumme
        }

        // Sortiert die gefilterten Produkte nach Datum
        private void SortByDate()
        {
            var sorted = FilteredProducts.OrderBy(p => p.DateAdded).ToList();
            FilteredProducts = new ObservableCollection<Product>(sorted);
        }

        // Berechnet die Gesamtsumme der gefilterten Produkte
        private void CalculateTotalSum()
        {
            TotalSum = FilteredProducts.Sum(p => p.Price);
        }

        // Löscht die gesamte Liste nach einer Bestätigung
        private async void ClearList()
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;

            if (currentPage != null)
            {
                bool confirm = await currentPage.DisplayAlert("Bestätigung", "Möchten Sie wirklich alle Produkte löschen?", "Ja", "Abbrechen");

                if (confirm)
                {
                    Products.Clear();
                    TotalSum = 0;
                    FilterProducts();
                    await ShowMessage("Alle Produkte wurden gelöscht.");
                }
            }
        }

        // Löscht ein einzelnes Produkt nach einer Bestätigung
        private async void DeleteProduct(Product product)
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;

            if (product != null && Products.Contains(product) && currentPage != null)
            {
                bool confirm = await currentPage.DisplayAlert("Bestätigung", $"Möchten Sie {product.Name} wirklich löschen?", "Ja", "Abbrechen");

                if (confirm)
                {
                    Products.Remove(product);
                    FilterProducts();
                    await ShowMessage($"{product.Name} wurde gelöscht.");
                }
            }
        }

        // Löscht ein Produkt mit Animation nach einer Bestätigung
        private async Task DeleteProductWithAnimation(Product product)
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;

            if (product != null && Products.Contains(product) && currentPage != null)
            {
                bool confirm = await currentPage.DisplayAlert("Bestätigung", $"Möchten Sie {product.Name} wirklich löschen?", "Ja", "Abbrechen");

                if (confirm)
                {
                    // Animation oder visuelle Effekte könnten hier hinzugefügt werden

                    Products.Remove(product);
                    FilterProducts();
                    await ShowMessage($"{product.Name} wurde erfolgreich gelöscht.");
                }
            }
        }

        // Zeigt eine Nachricht an den Benutzer
        private async Task ShowMessage(string message)
        {
            try
            {
                // Zeigt Toast-Nachricht, wenn möglich
                if (DeviceInfo.Platform != DevicePlatform.WinUI)
                {
                    await Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
                }
                else
                {
                    // Fallback auf DisplayAlert
                    var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                    if (currentPage != null)
                    {
                        await currentPage.DisplayAlert("Info", message, "OK");
                    }
                }
            }
            catch
            {
                // Zusätzlicher Fallback
                var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (currentPage != null)
                {
                    await currentPage.DisplayAlert("Info", message, "OK");
                }
            }
        }
    }
}