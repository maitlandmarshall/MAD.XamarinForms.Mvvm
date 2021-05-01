using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    internal class MvvmEngine
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly NavigationDataBag navigationDataBag;

        public MvvmEngine(IServiceScopeFactory serviceScopeFactory, NavigationDataBag navigationDataBag)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.navigationDataBag = navigationDataBag;
        }

        public void InitializeViewModel (VisualElement visualElement, Type viewModelType)
        {
            if (typeof(ViewModel).IsAssignableFrom(viewModelType) == false) throw new NotSupportedException("viewModelType must inherit from ViewModel.");

            var viewModelScope = this.serviceScopeFactory.CreateScope();
            var viewModelInstance = viewModelScope.ServiceProvider.GetRequiredService(viewModelType) as ViewModel;
            var viewModelLifecycleBehavior = new ViewModelLifecycleBehavior(viewModelScope, viewModelInstance);

            visualElement.Behaviors.Add(viewModelLifecycleBehavior);
        }

        public void OnNavigationDataBagIdChanged(ViewModel viewModel, string navigationDataBagId)
        {
            var navigationData = this.navigationDataBag.Get(navigationDataBagId);
            this.InvokePrepareMethod(viewModel, navigationData);
        }

        private void InvokePrepareMethod(ViewModel viewModel, object[] navigationData)
        {
            // Does a Prepare method signature match the navigation bag data input types?
            var prepareMethodInfo = viewModel.GetType().GetMethod("Prepare", navigationData.Select(y => y.GetType()).ToArray());

            if (prepareMethodInfo is null
                && navigationData.Length > 0)
            {
                throw new PrepareMethodNotFoundException("Unable to find Prepare method in viewModel that matches input navigation data", navigationData);
            }
            else if (prepareMethodInfo is null)
            {
                return;
            }

            prepareMethodInfo.Invoke(viewModel, navigationData);
        }
    }
}
