using Microsoft.Extensions.DependencyInjection;
using MAD.XamarinForms.Mvvm.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    internal static class MvvmServiceCollectionExtensions
    {
        public static IServiceCollection AddMvvmServices(this IServiceCollection serviceDescriptors, MvvmApp mvvmApp)
        {
            serviceDescriptors.AddSingleton(mvvmApp);
            serviceDescriptors.AddSingleton<MvvmEngine>();

            serviceDescriptors.AddSingleton<INavigationService, NavigationService>();
            serviceDescriptors.AddSingleton<INavigationRouteService, NavigationRouteService>();

            serviceDescriptors.AddSingleton<NavigationDataBag>();
            serviceDescriptors.AddSingleton<IMessagingCenter>(MessagingCenter.Instance);

            // Register event service as all its public interfaces
            serviceDescriptors.AddSingleton<EventService>();
            serviceDescriptors.AddSingleton<IEventConsumer>(svc => svc.GetRequiredService<EventService>());
            serviceDescriptors.AddSingleton<IEventProducer>(svc => svc.GetRequiredService<EventService>());

            return serviceDescriptors;
        }
    }
}
