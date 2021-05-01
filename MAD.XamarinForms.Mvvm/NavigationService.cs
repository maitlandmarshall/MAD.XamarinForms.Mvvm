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
            if (navigationData.Any())
            {
                var navigationDataBagId = this.navigationDataBag.Put(navigationData);
                var query = HttpUtility.ParseQueryString("");
                query[nameof(ViewModel.NavigationDataBagId)] = navigationDataBagId;

                route += $"?{query}";
            }

            if (Shell.Current is null)
            {
                if (this.mvvmApp.FormsApplication.MainPage is NavigationPage navigationPage)
                {
                    var routeElement = this.navigationRouteService.GetOrCreateContent(route) as Page;

                    if (routeElement is null)
                        throw new NotSupportedException("Route Element must be a Page.");

                    await navigationPage.PushAsync(routeElement);
                }
                else
                {
                    throw new NotSupportedException($"The FormsApplication.MainPage must be a {nameof(NavigationPage)} or a {nameof(Shell)}.");
                }
            }
            else
            {
                await Shell.Current.GoToAsync(route);
            }
        }

    }
}
