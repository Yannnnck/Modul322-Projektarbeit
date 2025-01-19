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
        private string _pageTitle = "ListPage";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterProducts();
            }
        }

        private DateTime _startDate = DateTime.Today.AddDays(-30);
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                FilterProducts();
            }
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                FilterProducts();
            }
        }

        public ObservableCollection<Product> Products => SharedData.Instance.Products;

        private ObservableCollection<Product> _filteredProducts = new();
        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set => SetProperty(ref _filteredProducts, value);
        }

        private decimal _totalSum;
        public decimal TotalSum
        {
            get => _totalSum;
            set => SetProperty(ref _totalSum, value);
        }

        public ICommand ClearListCommand { get; }
        public ICommand SortByDateCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand DeleteProductWithAnimationCommand { get; }

        public ListPageViewModel()
        {
            FilteredProducts = new ObservableCollection<Product>(Products);
            ClearListCommand = new Command(ClearList);
            SortByDateCommand = new Command(SortByDate);
            DeleteProductCommand = new Command<Product>(DeleteProduct);
            DeleteProductWithAnimationCommand = new Command<Product>(async (product) => await DeleteProductWithAnimation(product));

            Products.CollectionChanged += (s, e) => FilterProducts();
            FilterProducts();
        }

        private void FilterProducts()
        {
            var filtered = Products
                .Where(p =>
                    (string.IsNullOrWhiteSpace(SearchText) || p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) &&
                    p.DateAdded.Date >= StartDate.Date &&
                    p.DateAdded.Date <= EndDate.Date)
                .OrderBy(p => p.DateAdded)
                .ToList();

            FilteredProducts = new ObservableCollection<Product>(filtered);
            CalculateTotalSum();
        }

        private void SortByDate()
        {
            var sorted = FilteredProducts.OrderBy(p => p.DateAdded).ToList();
            FilteredProducts = new ObservableCollection<Product>(sorted);
        }

        private void CalculateTotalSum()
        {
            TotalSum = FilteredProducts.Sum(p => p.Price);
        }

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

        private async Task DeleteProductWithAnimation(Product product)
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;

            if (product != null && Products.Contains(product) && currentPage != null)
            {
                bool confirm = await currentPage.DisplayAlert("Bestätigung", $"Möchten Sie {product.Name} wirklich löschen?", "Ja", "Abbrechen");

                if (confirm)
                {
                    // Animation oder andere visuelle Effekte hier ausführen
                    await ShowMessage($"Produkt {product.Name} wird gelöscht...");

                    // Produkt löschen
                    Products.Remove(product);
                    FilterProducts();
                    await ShowMessage($"{product.Name} wurde erfolgreich gelöscht.");
                }
            }
        }

        private async Task ShowMessage(string message)
        {
            try
            {
                // Verwende Toast auf unterstützten Plattformen
                if (DeviceInfo.Platform != DevicePlatform.WinUI) // Prüfe auf unterstützte Plattformen
                {
                    await Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
                }
                else
                {
                    // Fallback auf DisplayAlert für nicht unterstützte Plattformen
                    var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                    if (currentPage != null)
                    {
                        await currentPage.DisplayAlert("Info", message, "OK");
                    }
                }
            }
            catch
            {
                // Fallback bei Fehlern
                var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (currentPage != null)
                {
                    await currentPage.DisplayAlert("Info", message, "OK");
                }
            }
        }
    }
}