using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public static partial class Mvvm
    {
        public static readonly BindableProperty ViewModelProperty = BindableProperty.CreateAttached("ViewModel", typeof(Type), typeof(ViewModel), null, propertyChanged: OnViewModelChanged);

        public static Type GetViewModel(BindableObject target)
        {
            return target.GetValue(ViewModelProperty) as Type;
        }

        private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is VisualElement visualElement)
            {
                MvvmApp.Current.Services.GetRequiredService<MvvmEngine>().InitializeViewModel(visualElement, newValue as Type);
            }
            else
            {
                throw new NotSupportedException("bindable must be a VisualElement.");
            }
        }
    }
}
