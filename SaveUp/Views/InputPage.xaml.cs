using Microsoft.Maui.Controls;
using SaveUp.ViewModels;

namespace SaveUp.Views
{
    public partial class InputPage : ContentPage
    {
        public InputPage()
        {
            InitializeComponent();
            BindingContext = new InputPageViewModel();
        }
    }
}