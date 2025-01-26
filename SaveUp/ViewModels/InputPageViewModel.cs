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
        // Der Titel der Seite, der in der Benutzeroberfläche angezeigt wird
        private string _pageTitle = "Input Page";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value); // Aktualisiert die UI bei Änderungen
        }

        // Produktname, den der Benutzer eingibt
        private string _productName = string.Empty;
        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value); // Aktualisiert die Eingabe
        }

        // Preis, den der Benutzer eingibt
        private string _price = string.Empty;
        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value); // Aktualisiert die Eingabe
        }

        // Kommentar, den der Benutzer eingibt
        private string _comment = string.Empty;
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value); // Aktualisiert die Eingabe
        }

        // Befehle für die Buttons "Speichern" und "Abbrechen"
        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }

        // Konstruktor zur Initialisierung der ViewModel-Logik
        public InputPageViewModel()
        {
            SaveCommand = new Command(SaveProduct); // Verknüpfung des Speichern-Befehls
            ClearCommand = new Command(ClearInputs); // Verknüpfung des Löschen-Befehls

            // Aktualisiert die Anzeige, wenn sich die Produktliste ändert
            SharedData.Instance.Products.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalSavingsLast30Days));
        }

        // Methode zum Speichern eines Produkts
        private async void SaveProduct()
        {
            // Überprüft, ob der Produktname oder der Preis leer ist
            if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Price))
            {
                await ShowMessage("Fehler", "Produktname und Preis dürfen nicht leer sein.");
                return;
            }

            // Überprüft, ob der eingegebene Preis eine gültige Zahl ist
            if (!decimal.TryParse(Price, out var priceValue))
            {
                await ShowMessage("Fehler", "Preis muss eine gültige Zahl sein.");
                return;
            }

            // Erstellt ein neues Produkt mit den eingegebenen Werten
            var product = new Product
            {
                Name = ProductName,
                Price = priceValue,
                Comment = Comment,
                DateAdded = DateTime.Now // Aktuelles Datum und Uhrzeit
            };

            // Fügt das Produkt zur Produktliste hinzu
            SharedData.Instance.Products.Add(product);

            // Leert die Eingabefelder
            ClearInputs();

            // Zeigt eine Erfolgsmeldung an
            await ShowMessage("Erfolg", "Produkt erfolgreich gespeichert!");
        }

        // Methode zum Leeren der Eingabefelder
        private void ClearInputs()
        {
            ProductName = string.Empty;
            Price = string.Empty;
            Comment = string.Empty;
        }

        // Zeigt eine Nachricht an den Benutzer
        private async Task ShowMessage(string title, string message)
        {
            try
            {
                // Versucht, eine Toast-Nachricht anzuzeigen
                await Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            }
            catch
            {
                // Fallback auf DisplayAlert, falls Toast nicht verfügbar ist
                var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (currentPage != null)
                {
                    await currentPage.DisplayAlert(title, message, "OK");
                }
            }
        }

        // Berechnet die Gesamteinsparungen der letzten 30 Tage
        public decimal TotalSavingsLast30Days
        {
            get
            {
                var fromDate = DateTime.Today.AddDays(-30); // Zeitraum der letzten 30 Tage
                return SharedData.Instance.Products
                    .Where(p => p.DateAdded >= fromDate) // Filtert Produkte, die innerhalb der letzten 30 Tage hinzugefügt wurden
                    .Sum(p => p.Price); // Summiert die Preise der gefilterten Produkte
            }
        }
    }
}