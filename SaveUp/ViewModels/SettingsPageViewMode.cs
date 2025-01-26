using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;
using SaveUp.Services;
using System.Windows.Input;

namespace SaveUp.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        // Titel der Seite, der in der Benutzeroberfläche angezeigt wird
        private string _pageTitle = "Settings Page";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value); // Benachrichtigt die UI bei Änderungen
        }

        // Befehl, um gespeicherte Daten zu löschen
        public ICommand DeleteDataCommand { get; }

        // Konstruktor, um den Befehl zu initialisieren
        public SettingsPageViewModel()
        {
            DeleteDataCommand = new Command(DeleteSavedProductsFile); // Verknüpft den Löschbefehl mit der entsprechenden Methode
        }

        // Methode, um die gespeicherten Produkte zu löschen
        private void DeleteSavedProductsFile()
        {
            // Pfad zur Datei, in der die Produkte gespeichert sind
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "SavedProducts.json");

            // Überprüft, ob die Datei existiert
            if (File.Exists(filePath))
            {
                // Löscht die Datei
                File.Delete(filePath);

                // Leert die Sammlung der Produkte
                SharedData.Instance.Products.Clear();

                // Zeigt eine Erfolgsmeldung an
                ShowAlert("Erfolg", "Die gespeicherten Daten wurden zurückgesetzt.");
            }
            else
            {
                // Zeigt eine Hinweisnachricht an, wenn die Datei nicht existiert
                ShowAlert("Hinweis", "Es gibt keine gespeicherten Daten.");
            }
        }

        // Methode, um eine Benachrichtigung (Alert) anzuzeigen
        private async void ShowAlert(string title, string message)
        {
            // Überprüft, ob es mindestens ein geöffnetes Fenster gibt
            if (Application.Current?.Windows.Count > 0)
            {
                // Holt die Hauptseite des aktuellen Fensters
                var page = Application.Current.Windows[0].Page;

                // Zeigt die Benachrichtigung an, falls die Seite verfügbar ist
                if (page != null)
                {
                    await page.DisplayAlert(title, message, "OK");
                }
            }
        }
    }
}