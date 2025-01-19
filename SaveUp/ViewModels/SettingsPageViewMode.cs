using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;
using System.Windows.Input;

namespace SaveUp.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private string _pageTitle = "SettingsPage";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        public ICommand DeleteDataCommand { get; }

        public SettingsPageViewModel()
        {
            DeleteDataCommand = new Command(DeleteSavedProductsFile);
        }

        private void DeleteSavedProductsFile()
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "SavedProducts.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                ShowAlert("Erfolg", "Die gespeicherten Daten wurden zurückgesetzt.");
            }
            else
            {
                ShowAlert("Hinweis", "Es gibt keine gespeicherten Daten.");
            }
        }

        private async void ShowAlert(string title, string message)
        {
            if (Application.Current?.Windows.Count > 0)
            {
                var page = Application.Current.Windows[0].Page;
                if (page != null)
                {
                    await page.DisplayAlert(title, message, "OK");
                }
            }
        }
    }
}
