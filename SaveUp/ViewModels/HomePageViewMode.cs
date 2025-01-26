using System;
using System.Linq;
using SaveUp.Models;
using SaveUp.Services;

namespace SaveUp.ViewModels
{
    /// <summary>
    /// ViewModel für die HomePage. Es verwaltet den Titel der Seite und die Berechnung der Gesamteinsparungen der letzten 30 Tage.
    /// </summary>
    public class HomePageViewModel : BaseViewModel
    {
        // Der Titel der Seite, der in der Benutzeroberfläche angezeigt wird
        private string _pageTitle = "Home Page";

        /// <summary>
        /// Titel der HomePage. Diese Eigenschaft wird verwendet, um den Seitentitel dynamisch zu aktualisieren.
        /// </summary>
        public string PageTitle
        {
            get => _pageTitle; // Gibt den aktuellen Titel zurück
            set => SetProperty(ref _pageTitle, value); // Aktualisiert den Titel und benachrichtigt die UI
        }

        /// <summary>
        /// Berechnet die Gesamteinsparungen der letzten 30 Tage basierend auf der gespeicherten Produktliste.
        /// </summary>
        public decimal TotalSavingsLast30Days
        {
            get
            {
                // Bestimmt das Startdatum der letzten 30 Tage
                var fromDate = DateTime.Today.AddDays(-30);

                // Filtert die Produkte, die in den letzten 30 Tagen hinzugefügt wurden, und summiert die Preise
                return SharedData.Instance.Products
                    .Where(p => p.DateAdded >= fromDate) // Produkte mit Datum innerhalb der letzten 30 Tage
                    .Sum(p => p.Price); // Summiert die Preise der gefilterten Produkte
            }
        }

        /// <summary>
        /// Konstruktor, der die notwendige Logik initialisiert.
        /// </summary>
        public HomePageViewModel()
        {
            // Aktualisiert die Anzeige, wenn sich die Produktliste ändert
            SharedData.Instance.Products.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalSavingsLast30Days));
        }
    }
}