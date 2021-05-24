using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    internal class NavigationService : INavigationService
    {
        private readonly NavigationDataBag navigationDataBag;
        private readonly INavigationRouteService navigationRouteService;
        private readonly MvvmApp mvvmApp;

        public event EventHandler<NavigatingEventArgs> Navigating;

        public NavigationService(NavigationDataBag navigationDataBag, INavigationRouteService navigationRouteService, MvvmApp mvvmApp)
        {
            this.navigationDataBag = navigationDataBag;
            this.navigationRouteService = navigationRouteService;
            this.mvvmApp = mvvmApp;
            this.mvvmApp.FormsApplication.PropertyChanged += FormsApplication_PropertyChanged;

            if (this.mvvmApp.FormsApplication.MainPage != null)
                this.OnMainPageChanged();
        }

        public async Task GoToAsync(string route, params object[] navigationData)
        {
            await this.NavigateAndYieldCurrentPage(route, navigationData);
        }

        public async Task<TResult> GoToAsync<TResult>(string route, params object[] navigationData)
        {
            var currentPage = await this.NavigateAndYieldCurrentPage(route, navigationData);
            var viewModel = currentPage?.BindingContext as ViewModel<TResult>;

            if (viewModel is null)
                return default(TResult);

            return await viewModel?.ResultTaskCompletionSource.Task;
        }

        private async Task<Page> NavigateAndYieldCurrentPage(string route, params object[] navigationData)
        {          
            if (navigationData.Any())
            {
                var navigationDataBagId = this.navigationDataBag.Put(navigationData);
                var query = HttpUtility.ParseQueryString("");
                query[nameof(ViewModel.NavigationDataBagId)] = navigationDataBagId;

                route += $"?{query}";
            }

            Page currentPage;

            if (Shell.Current is null)
            {
                if (this.mvvmApp.FormsApplication.MainPage is NavigationPage navigationPage)
                {
                    var navArgs = this.OnNavigating(new NavigatingEventArgs(route));

                    if (navArgs.Cancel == true)
                        return null;

                    var routeElement = this.navigationRouteService.GetOrCreateContent(route) as Page;

                    if (routeElement is null)
                        throw new NotSupportedException("Route Element must be a Page.");

                    await navigationPage.PushAsync(routeElement);
                    currentPage = routeElement;
                }
                else
                {
                    throw new NotSupportedException($"The FormsApplication.MainPage must be a {nameof(NavigationPage)} or a {nameof(Shell)}.");
                }
            }
            else
            {
                await Shell.Current.GoToAsync(route);
                currentPage = Shell.Current.CurrentPage;
            }

            return currentPage;
        }

        private void FormsApplication_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.mvvmApp.FormsApplication.MainPage):
                    this.OnMainPageChanged();
                    break;
            }
        }

        private void OnMainPageChanged()
        {
            var mainPage = this.mvvmApp.FormsApplication.MainPage;

            if (mainPage is Shell shell)
            {
                shell.Navigating += Shell_Navigating;
            }
            else if (mainPage is NavigationPage navigationPage)
            {
                // TODO: Hook into when the back button is pressed
            }
        }


        private void Shell_Navigating(object sender, ShellNavigatingEventArgs e)
        {            
            var navArgs = this.OnNavigating(new NavigatingEventArgs(e));

            if (navArgs.Cancel)
                e.Cancel();
        }

        private NavigatingEventArgs OnNavigating(NavigatingEventArgs navArgs)
        {
            this.Navigating?.Invoke(this, navArgs);

            return navArgs;
        }
    }
}
