using MAD.XamarinForms.Mvvm.TestApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm.TestApp.ViewModels
{
    public class IndexViewModel : ViewModel
    {
        private readonly INavigationService navigationService;

        public IndexViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            this.NavigateWithResultCommand = new Command(this.OnNavigateWithResult);
            this.CancelNavigatingEventCommand = new Command(this.CancelNavigatingEvent);
        }

        public ICommand NavigateWithResultCommand { get; }
        public ICommand CancelNavigatingEventCommand { get; }


        private void NavigationService_Navigating(object sender, NavigatingEventArgs e)
        {
            e.Cancel = true;
        }

        private async void OnNavigateWithResult()
        {
            var navResult = await this.navigationService.GoToAsync<string>(nameof(Details1Page));
        }


        private async void CancelNavigatingEvent()
        {
            this.navigationService.Navigating += NavigationService_Navigating;
            await this.navigationService.GoToAsync<string>(nameof(Details1Page));
            this.navigationService.Navigating -= NavigationService_Navigating;
        }
    }
}
