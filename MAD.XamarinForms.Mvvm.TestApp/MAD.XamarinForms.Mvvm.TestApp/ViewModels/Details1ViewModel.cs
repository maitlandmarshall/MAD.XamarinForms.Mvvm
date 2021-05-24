using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm.TestApp.ViewModels
{
    public class Details1ViewModel : ViewModel<string>
    {
        public Details1ViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        private ICommand sendResultCommand;
        private readonly INavigationService navigationService;

        public ICommand SendResultCommand
        {
            get
            {
                if (sendResultCommand == null)
                {
                    sendResultCommand = new Command(SendResult);
                }

                return sendResultCommand;
            }
        }

        public override async Task Initialize()
        {
            this.navigationService.Navigating += NavigationService_Navigating;
        }

        public override void ViewDestroy()
        {
            this.navigationService.Navigating -= NavigationService_Navigating;
        }

        private void NavigationService_Navigating(object sender, NavigatingEventArgs e)
        {
            if (this.ResultTaskCompletionSource.Task.IsCompleted == false)
                e.Cancel = true;
        }

        private void SendResult()
        {
            this.ResultTaskCompletionSource.SetResult("hello");
        }
    }
}
