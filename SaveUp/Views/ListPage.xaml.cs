using Microsoft.Maui.Controls;
using SaveUp.ViewModels;

namespace SaveUp.Views
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
            BindingContext = new ListPageViewModel();
        }
    }
}