using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public static partial class Mvvm
    {
        public static void Init<TStartup>(TStartup instance = null)
            where TStartup : class, new()
        {
            if (instance is null) instance = new TStartup();

            var mvvmApp = MvvmApp.Current = new MvvmApp(Application.Current);
            var serviceDescriptors = new ServiceCollection()
                .AddMvvmServices(mvvmApp);

            ConfigureStartupServices(instance, serviceDescriptors);

            var services = serviceDescriptors.BuildServiceProvider();
            mvvmApp.Services = services;

            ConfigureStartup(instance, services);
        }

        private static void ConfigureStartupServices<TStartup>(TStartup instance, IServiceCollection serviceDescriptors)
        {
            var configureServices = typeof(TStartup).GetMethod("ConfigureServices");
            if (configureServices is null) return;

            configureServices.Invoke(instance, new[] { serviceDescriptors });
        }

        private static void ConfigureStartup<TStartup>(TStartup instance, IServiceProvider services)
        {
            var configure = typeof(TStartup).GetMethod("Configure");
            if (configure is null) return;

            var configureParams = configure.GetParameters();
            var servicesToInject = new List<object>();

            foreach(var cp in configureParams)
            {
                servicesToInject.Add(services.GetRequiredService(cp.ParameterType));
            }

            var configureResult = configure.Invoke(instance, servicesToInject.ToArray());

            if (configureResult is Task t)
            {
                t.Wait();
            }
        }
    }
}
