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
            RegisterSingletonServices(builder);
           
        }

        private static void RegisterViewModels(MauiAppBuilder builder)
        {
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddScoped<Notes4DevViewModel>();
            builder.Services.AddScoped<Notes4DevPage>();
            builder.Services.AddTransient<ClientsPage>( );
            builder.Services.AddTransient<ClientsViewModel>();
            builder.Services.AddTransient<ClientDetailsPage>();
            builder.Services.AddTransient<ClientDetailsViewModel>();

            builder.Services.AddTransient<LevelsPageViewModel>();
            builder.Services.AddTransient<LevelsPage>();

             builder.Services.AddSingleton<IFibAddGenericService<object>, FibGenericAddService>();

            builder.Services.AddSingleton<IDevNotesService, FibService>();
            builder.Services.AddSingleton<IFibCRUDClients, FibCRUDClientsService>();
            builder.Services.AddSingleton<IFibStorageService, FibStorageService>();
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

