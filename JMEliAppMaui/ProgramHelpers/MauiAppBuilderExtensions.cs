using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using JMEliAppMaui.ViewModels;
using JMEliAppMaui.ViewModels.StudentsViewModels;
using JMEliAppMaui.Views; 
using static FibStatusService;

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

            builder.Services.AddTransient<StatusPageViewModel>();
            builder.Services.AddTransient<StatusPage>();


            builder.Services.AddTransient<CyclesPage>();
            builder.Services.AddTransient<CyclesViewModel>();

            builder.Services.AddTransient<ContractsViewModel>();
            builder.Services.AddTransient<ContractsPage>();
            
                builder.Services.AddSingleton<AddStudentPageViewModel>();
            builder.Services.AddSingleton<AddStudentPage>();

            builder.Services.AddSingleton<IFibAddGenericService<object>, FibGenericAddService>();
            builder.Services.AddSingleton<IFibCyclesService, FibCyclesService>();

            builder.Services.AddSingleton<IFibLevelsService, FibLevelsService>();
            builder.Services.AddSingleton<IFibStatusService, FibStatusService>();
            builder.Services.AddSingleton<IFibContract, FibContractService>();
            //

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

