using MAD.XamarinForms.Mvvm.TestApp.Pages;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm.TestApp
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddViewModels();
        }

        public void Configure(MvvmApp mvvmApp, INavigationRouteService navigationRouteService)
        {
            navigationRouteService.RegisterRoute(nameof(Details1Page), typeof(Details1Page));

            mvvmApp.FormsApplication.MainPage = new AppShell();
        }
    }
}