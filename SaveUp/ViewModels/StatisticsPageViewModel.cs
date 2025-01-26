using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SaveUp.Models;
using SaveUp.Services;
using Microsoft.Maui.Storage;
using CommunityToolkit.Maui.Alerts;

namespace SaveUp.ViewModels
{
    public class StatisticsPageViewModel : BaseViewModel
    {
        // Der Titel der Seite
        private string _pageTitle = "Statistics Page";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value); // Bindet Änderungen des Titels an die UI
        }

        // Breite des Grids für den Fortschrittsbalken
        private double _gridWidth;
        public double GridWidth
        {
            get => _gridWidth;
            set
            {
                if (SetProperty(ref _gridWidth, value))
                {
                    OnPropertyChanged(nameof(ProgressBarWidth)); // Aktualisiert die Breite des Fortschrittsbalkens
                }
            }
        }

        // Berechnet die Breite des Fortschrittsbalkens basierend auf dem Prozentsatz
        public double ProgressBarWidth => ProgressPercentage * GridWidth;

        // Prozentsatz des Fortschritts
        public double ProgressPercentage
        {
            get
            {
                if (SavingGoal <= 0) return 0.0; // Verhindert Division durch 0
                var progress = (double)CurrentSavings / (double)SavingGoal;
                return Math.Clamp(progress, 0.0, 1.0); // Begrenze den Wert auf 0 bis 1 (0% bis 100%)
            }
        }

        // Liste verfügbarer Jahre für die Statistik
        public ObservableCollection<int> AvailableYears { get; }

        // Liste verfügbarer Monate für die Statistik
        public ObservableCollection<int> AvailableMonths { get; }

        // Das aktuell ausgewählte Jahr
        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (SetProperty(ref _selectedYear, value))
                {
                    UpdateChartData(); // Aktualisiert die Diagrammdaten, wenn das Jahr geändert wird
                }
            }
        }

        // Der aktuell ausgewählte Monat
        private int _selectedMonth;
        public int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (SetProperty(ref _selectedMonth, value))
                {
                    UpdateChartData(); // Aktualisiert die Diagrammdaten, wenn der Monat geändert wird
                }
            }
        }

        // Das aktuelle Jahr
        public int CurrentYear { get; }

        // Der aktuelle Monat
        public int CurrentMonth { get; }

        // Das Sparziel des Nutzers
        private decimal _savingGoal;
        public decimal SavingGoal
        {
            get => _savingGoal;
            set
            {
                if (SetProperty(ref _savingGoal, value))
                {
                    Preferences.Set(nameof(SavingGoal), _savingGoal.ToString()); // Speichert das Sparziel
                    UpdateSavingsRelatedProperties(); // Aktualisiert abhängige Werte
                }
            }
        }

        // Gesamtbetrag der aktuellen Einsparungen
        public decimal CurrentSavings => SharedData.Instance.Products.Sum(p => p.Price);

        // Verbleibender Betrag, um das Sparziel zu erreichen
        public decimal RemainingAmount => Math.Max(SavingGoal - CurrentSavings, 0);

        // Der maximale Preis, der für die Diagramm-Skalierung verwendet wird
        private decimal _maxPrice;
        public decimal MaxPrice
        {
            get => _maxPrice;
            set => SetProperty(ref _maxPrice, value);
        }

        // Daten für das Balkendiagramm
        public ObservableCollection<ChartData> ChartData { get; set; }

        // Befehl zum Speichern des Sparziels
        public ICommand SetSavingGoalCommand { get; }

        // Konstruktor der ViewModel-Klasse
        public StatisticsPageViewModel()
        {
            // Initialisiert das aktuelle Jahr und den Monat
            CurrentYear = DateTime.Now.Year;
            CurrentMonth = DateTime.Now.Month;

            // Erstellt eine Liste der letzten 10 Jahre
            AvailableYears = new ObservableCollection<int>(Enumerable.Range(CurrentYear - 10, 11).Reverse());
            SelectedYear = CurrentYear;

            // Erstellt eine Liste der Monate (1 bis 12)
            AvailableMonths = new ObservableCollection<int>(Enumerable.Range(1, 12));
            SelectedMonth = CurrentMonth;

            // Lädt das gespeicherte Sparziel
            var savedGoal = Preferences.Get(nameof(SavingGoal), "0");
            _savingGoal = decimal.TryParse(savedGoal, out var parsedGoal) ? parsedGoal : 0m;
            UpdateSavingsRelatedProperties();

            // Initialisiert das Diagramm und den Sparziel-Befehl
            ChartData = new ObservableCollection<ChartData>();
            SetSavingGoalCommand = new Command(UpdateSavingGoal);

            // Abonniert Änderungen in der Produktliste
            SharedData.Instance.Products.CollectionChanged += (s, e) =>
            {
                UpdateChartData();
                UpdateSavingsRelatedProperties();
            };

            // Initialisiert die Diagrammdaten
            UpdateChartData();
        }

        // Aktualisiert die Diagrammdaten basierend auf dem ausgewählten Jahr und Monat
        private void UpdateChartData()
        {
            if (SelectedYear < DateTime.MinValue.Year || SelectedYear > DateTime.MaxValue.Year)
                return;

            if (SelectedMonth < 1 || SelectedMonth > 12)
                return;

            // Generiert alle Tage des ausgewählten Monats
            var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(SelectedYear, SelectedMonth))
                                         .Select(day => new DateTime(SelectedYear, SelectedMonth, day))
                                         .ToList();

            // Gruppiert Produkte nach Datum und füllt fehlende Tage mit 0
            var filteredData = daysInMonth
                .GroupJoin(
                    SharedData.Instance.Products.Where(p => p.DateAdded.Year == SelectedYear && p.DateAdded.Month == SelectedMonth),
                    day => day,
                    product => product.DateAdded.Date,
                    (day, products) => new ChartData
                    {
                        Date = day.ToShortDateString(),
                        Value = products.Sum(p => p.Price),
                        Height = 0
                    })
                .ToList();

            // Berechnet den maximalen Preis für die Skalierung
            MaxPrice = filteredData.Any() ? filteredData.Max(d => d.Value) * 1.2m : 1m;

            // Berechnet die Höhe jedes Balkens im Diagramm
            foreach (var data in filteredData)
            {
                const double totalHeight = 250;
                data.Height = MaxPrice > 0 ? Math.Min((double)(data.Value / MaxPrice) * totalHeight, totalHeight) : 0;
            }

            // Aktualisiert die Diagrammdaten
            ChartData = new ObservableCollection<ChartData>(filteredData);
            OnPropertyChanged(nameof(ChartData));
            OnPropertyChanged(nameof(MaxPrice));
        }

        // Aktualisiert alle abhängigen Werte wie Fortschritt und verbleibenden Betrag
        private void UpdateSavingsRelatedProperties()
        {
            OnPropertyChanged(nameof(CurrentSavings));
            OnPropertyChanged(nameof(RemainingAmount));
            OnPropertyChanged(nameof(ProgressPercentage));
            OnPropertyChanged(nameof(ProgressBarWidth));
        }

        // Speichert das Sparziel und aktualisiert abhängige Werte
        private void UpdateSavingGoal()
        {
            Preferences.Set(nameof(SavingGoal), SavingGoal.ToString());
            UpdateSavingsRelatedProperties();
        }
    }
}