using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Helpers
{
    public static class ServiceLocator
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        public static T GetService<T>() where T : notnull
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
