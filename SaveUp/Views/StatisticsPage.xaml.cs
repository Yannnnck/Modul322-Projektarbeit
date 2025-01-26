using Microsoft.Maui.Controls;
using SaveUp.ViewModels;
using System.ComponentModel;

namespace SaveUp.Views;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
        BindingContext = new StatisticsPageViewModel();

        ProgressGrid.SizeChanged += (sender, e) =>
        {
            if (BindingContext is StatisticsPageViewModel viewModel)
            {
                viewModel.GridWidth = ProgressGrid.Width; // Dynamische Breite des Grids
            }
        };
    }
}


