using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm.TestApp.ViewModels
{
    public class Details1ViewModel : ViewModel<string>
    {

        private ICommand sendResultCommand;

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

        private void SendResult()
        {
            this.ResultTaskCompletionSource.SetResult("hello");
        }
    }
}
