using MAD.XamarinForms.Mvvm.TestApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
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
        }

        public ICommand NavigateWithResultCommand { get; }

        private async void OnNavigateWithResult()
        {
            var navResult = await this.navigationService.GoToAsync<string>(nameof(Details1Page));
        }

    }
}
