using System;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public interface INavigationRouteService
    {
        Element GetOrCreateContent(string route);
        void RegisterRoute(string route, Type type);
    }
}