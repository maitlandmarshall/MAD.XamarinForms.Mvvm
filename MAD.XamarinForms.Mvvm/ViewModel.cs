using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    [QueryProperty(nameof(NavigationDataBagId), nameof(NavigationDataBagId))]
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NavigationDataBagId
        {
            set
            {
                MvvmApp.Current.Services.GetRequiredService<MvvmEngine>().OnNavigationDataBagIdChanged(this, value);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Task Initialize() => Task.CompletedTask;
        public virtual void ViewCreated() { }
        public virtual void ViewAppearing() { }
        public virtual void ViewAppeared() { }
        public virtual void ViewDisappearing() { }
        public virtual void ViewDisappeared() { }
        public virtual void ViewDestroy() { }
    }

    public abstract class ViewModel<TResult> : ViewModel
    {
        public TaskCompletionSource<TResult> ResultTaskCompletionSource { get; } = new TaskCompletionSource<TResult>();
    }
}
