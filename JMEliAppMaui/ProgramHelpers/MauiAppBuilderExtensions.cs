using System;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using JMEliAppMaui.ViewModels;
using JMEliAppMaui.Views;
using Microsoft.Extensions.DependencyInjection;

namespace JMEliAppMaui.ProgramHelpers
{
    public interface ISingletonDependency { };
    public static class MauiAppBuilderExtensions
    {
        public static void ConfigureServices(this MauiAppBuilder builder)
        {
            RegisterViewModels(builder);
            //RegisterSingletonServices(builder);
        }

        private static void RegisterViewModels(MauiAppBuilder builder)
        {
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<Notes4DevViewModel>();
            builder.Services.AddTransient<Notes4DevPage>();
            builder.Services.AddSingleton<IDevNotesService, FibService>();
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

