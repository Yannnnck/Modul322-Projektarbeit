using Microsoft.Maui.Controls;
using SaveUp.ViewModels;
using SaveUp.Models;

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