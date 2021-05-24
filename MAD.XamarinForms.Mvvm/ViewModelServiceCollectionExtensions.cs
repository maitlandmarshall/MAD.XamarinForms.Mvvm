using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MAD.XamarinForms.Mvvm
{
    public static class ViewModelServiceCollectionExtensions
    {
        public static IServiceCollection AddViewModels(this IServiceCollection serviceDescriptors, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetCallingAssembly();

            var viewModelTypes = assembly.GetTypes()
               .Where(y => typeof(ViewModel).IsAssignableFrom(y))
               .Where(y => y.IsAbstract == false);

            foreach (var vmt in viewModelTypes)
            {
                serviceDescriptors.AddScoped(vmt);
            }

            return serviceDescriptors;
        }
    }
}
