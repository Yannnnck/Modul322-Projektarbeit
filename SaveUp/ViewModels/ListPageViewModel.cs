using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace SaveUp.ViewModels
{
    public class ListPageViewModel : INotifyPropertyChanged
    {
        // PropertyChanged-Event als nullable deklariert
        public event PropertyChangedEventHandler? PropertyChanged;

        // Methode zum Auslösen des PropertyChanged-Events
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _pageTitle = "ListPage";
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                if (_pageTitle != value)
                {
                    _pageTitle = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}