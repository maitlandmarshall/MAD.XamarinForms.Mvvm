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

        public NavigationService(NavigationDataBag navigationDataBag, INavigationRouteService navigationRouteService, MvvmApp mvvmApp)
        {
            this.navigationDataBag = navigationDataBag;
            this.navigationRouteService = navigationRouteService;
            this.mvvmApp = mvvmApp;
        }

        public async Task GoToAsync(string route, params object[] navigationData)
        {
            await this.NavigateAndYieldCurrentPage(route, navigationData);
        }

        public async Task<TResult> GoToAsync<TResult>(string route, params object[] navigationData)
        {
            var currentPage = await this.NavigateAndYieldCurrentPage(route, navigationData);
            var viewModel = currentPage.BindingContext as ViewModel<TResult>;

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

    }
}
