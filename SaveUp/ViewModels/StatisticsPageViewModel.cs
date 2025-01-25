using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SaveUp.Models;
using SaveUp.Services;
using Microsoft.Maui.Storage;

namespace SaveUp.ViewModels
{
    public class StatisticsPageViewModel : BaseViewModel
    {
        private string _pageTitle = "StatisticsPage";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        public ObservableCollection<int> AvailableYears { get; }
        public ObservableCollection<int> AvailableMonths { get; }

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (SetProperty(ref _selectedYear, value))
                {
                    UpdateChartData();
                }
            }
        }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (SetProperty(ref _selectedMonth, value))
                {
                    UpdateChartData();
                }
            }
        }

        public int CurrentYear { get; }
        public int CurrentMonth { get; }

        private decimal _savingGoal;
        public decimal SavingGoal
        {
            get => _savingGoal;
            set
            {
                if (SetProperty(ref _savingGoal, value))
                {
                    // Sparziel speichern
                    Preferences.Set(nameof(SavingGoal), _savingGoal.ToString());
                    UpdateSavingsRelatedProperties();
                }
            }
        }

        public decimal CurrentSavings => SharedData.Instance.Products.Sum(p => p.Price);

        public decimal RemainingAmount => Math.Max(SavingGoal - CurrentSavings, 0);

        public double ProgressPercentage => SavingGoal > 0 ? (double)(CurrentSavings / SavingGoal) : 0;

        private decimal _maxPrice;
        public decimal MaxPrice
        {
            get => _maxPrice;
            set => SetProperty(ref _maxPrice, value);
        }

        public ObservableCollection<ChartData> ChartData { get; set; }

        public ICommand SetSavingGoalCommand { get; }

        public StatisticsPageViewModel()
        {
            CurrentYear = DateTime.Now.Year;
            CurrentMonth = DateTime.Now.Month;

            // Aktuelles Jahr und die letzten 10 Jahre
            AvailableYears = new ObservableCollection<int>(Enumerable.Range(CurrentYear - 10, 11).Reverse());
            SelectedYear = CurrentYear;

            // Monate (1 bis 12)
            AvailableMonths = new ObservableCollection<int>(Enumerable.Range(1, 12));
            SelectedMonth = CurrentMonth;

            // Sparziel laden (String auslesen und in decimal umwandeln)
            var savedGoal = Preferences.Get(nameof(SavingGoal), "0");
            _savingGoal = decimal.TryParse(savedGoal, out var parsedGoal) ? parsedGoal : 0m;
            UpdateSavingsRelatedProperties();

            ChartData = new ObservableCollection<ChartData>();
            SetSavingGoalCommand = new Command(UpdateSavingGoal);

            // Abonniere Änderungen an der Produktliste
            SharedData.Instance.Products.CollectionChanged += (s, e) =>
            {
                UpdateChartData();
                UpdateSavingsRelatedProperties(); // Aktualisiere abhängige Werte wie Fortschritt
            };

            UpdateChartData();
        }

        private void UpdateChartData()
        {
            if (SelectedYear < DateTime.MinValue.Year || SelectedYear > DateTime.MaxValue.Year)
                return;

            if (SelectedMonth < 1 || SelectedMonth > 12)
                return;

            // Alle Tage des ausgewählten Monats generieren
            var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(SelectedYear, SelectedMonth))
                                         .Select(day => new DateTime(SelectedYear, SelectedMonth, day))
                                         .ToList();

            // Produkte gruppieren und fehlende Tage mit Value = 0 auffüllen
            var filteredData = daysInMonth
                .GroupJoin(
                    SharedData.Instance.Products.Where(p => p.DateAdded.Year == SelectedYear && p.DateAdded.Month == SelectedMonth),
                    day => day,
                    product => product.DateAdded.Date,
                    (day, products) => new ChartData
                    {
                        Date = day.ToShortDateString(),
                        Value = products.Sum(p => p.Price),
                        Height = 0 // Höhe wird später berechnet
                    })
                .ToList();

            // Maximalen Preis für die Skalierung berechnen
            MaxPrice = filteredData.Any() ? filteredData.Max(d => d.Value) * 1.2m : 1m;

            // Höhe für jeden Balken berechnen
            foreach (var data in filteredData)
            {
                const double totalHeight = 250; // Maximale Diagrammhöhe
                data.Height = MaxPrice > 0 ? (double)(data.Value / MaxPrice) * totalHeight : 0;
            }

            ChartData = new ObservableCollection<ChartData>(filteredData);
            OnPropertyChanged(nameof(ChartData));
            OnPropertyChanged(nameof(MaxPrice));
        }

        private void UpdateSavingsRelatedProperties()
        {
            OnPropertyChanged(nameof(CurrentSavings));
            OnPropertyChanged(nameof(RemainingAmount));
            OnPropertyChanged(nameof(ProgressPercentage));
        }

        private void UpdateSavingGoal()
        {
            Preferences.Set(nameof(SavingGoal), SavingGoal.ToString()); // Sparziel speichern
            UpdateSavingsRelatedProperties();
        }
    }
}