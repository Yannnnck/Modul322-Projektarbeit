using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveUp.ViewModels
{
    public class StatisticsPageViewModel : BaseViewModel
    {
        private string _pageTitle = "StatisticsPage";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }
    }
}
