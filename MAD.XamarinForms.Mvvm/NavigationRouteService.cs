using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    internal class NavigationRouteService : INavigationRouteService
    {
        public void RegisterRoute(string route, Type type)
        {
            Routing.RegisterRoute(route, type);
        }

        public Element GetOrCreateContent(string route)
        {
            return Routing.GetOrCreateContent(route);
        }
    }
}
