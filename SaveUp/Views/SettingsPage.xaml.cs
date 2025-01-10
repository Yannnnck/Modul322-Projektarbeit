using Microsoft.Maui.Controls;
using SaveUp.ViewModels;

namespace SaveUp.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
        BindingContext = new SettingsPageViewModel();
    }
}