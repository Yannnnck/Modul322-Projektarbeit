using SaveUp.Models;
using SaveUp.Services;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;

namespace SaveUp.ViewModels
{
    public class InputPageViewModel : BaseViewModel
    {
        private string _pageTitle = "InputPage";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        private string _productName = string.Empty;
        private string _price = string.Empty;
        private string _comment = string.Empty;

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }

        public InputPageViewModel()
        {
            SaveCommand = new Command(SaveProduct);
            ClearCommand = new Command(ClearInputs);
        }

        private async void SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Price))
            {
                await ShowMessage("Fehler", "Produktname und Preis dürfen nicht leer sein.");
                return;
            }

            if (!decimal.TryParse(Price, out var priceValue))
            {
                await ShowMessage("Fehler", "Preis muss eine gültige Zahl sein.");
                return;
            }

            var product = new Product
            {
                Name = ProductName,
                Price = priceValue,
                Comment = Comment,
                DateAdded = DateTime.Now
            };

            SharedData.Instance.Products.Add(product);

            ClearInputs();

            await ShowMessage("Erfolg", "Produkt erfolgreich gespeichert!");
        }

        private void ClearInputs()
        {
            ProductName = string.Empty;
            Price = string.Empty;
            Comment = string.Empty;
        }

        private async Task ShowMessage(string title, string message)
        {
            try
            {
                await Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            }
            catch
            {
                var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (currentPage != null)
                {
                    await currentPage.DisplayAlert(title, message, "OK");
                }
            }
        }
    }
}