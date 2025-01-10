using Microsoft.Maui.Controls;
using SaveUp.ViewModels;

namespace SaveUp.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel();
        }
    }
}