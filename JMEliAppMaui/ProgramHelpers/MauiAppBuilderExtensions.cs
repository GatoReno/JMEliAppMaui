using System;
using JMEliAppMaui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace JMEliAppMaui.ProgramHelpers
{
    public interface ISingletonDependency { };
    public static class MauiAppBuilderExtensions
    {
        public static void ConfigureServices(this MauiAppBuilder builder)
        {
            RegisterViewModels(builder);
            RegisterSingletonServices(builder);
        }

        private static void RegisterViewModels(MauiAppBuilder builder)
        {
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            
        }

        private static void RegisterSingletonServices(MauiAppBuilder builder)
        {
            var services = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                            .Where(x => typeof(ISingletonDependency).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

            foreach (var service in services)
                builder.Services.AddSingleton(service);
        }
    }
}

