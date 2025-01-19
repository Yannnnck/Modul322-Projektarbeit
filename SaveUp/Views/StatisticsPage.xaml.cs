using Microsoft.Maui.Controls;
using SaveUp.ViewModels;

namespace SaveUp.Views;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
        BindingContext = new StatisticsPageViewModel();
    }
}