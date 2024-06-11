﻿using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using JMEliAppMaui.Services;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using JMEliAppMaui.ViewModels;
using JMEliAppMaui.ViewModels.LoginViewModels;
using JMEliAppMaui.ViewModels.StudentsViewModels;
using JMEliAppMaui.Views;
using JMEliAppMaui.Views.LoginViews;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
 

namespace JMEliAppMaui.ProgramHelpers
{
    public interface ISingletonDependency { };
    public static class MauiAppBuilderExtensions
    {
        public static void ConfigureServices(this MauiAppBuilder builder)
        {
            RegisterViewModels(builder);
            RegisterSingletonServices(builder);
            RegisterUserDialogsServices(builder);
            RegisterPDFViewerServices(builder);
            //RegisterHandlers(builder) ... still to research
#if ANDROID
            Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {

                handler.PlatformView.Settings.JavaScriptEnabled = true;
                handler.PlatformView.Settings.AllowFileAccess = true;
                handler.PlatformView.Settings.AllowFileAccessFromFileURLs = true;
                handler.PlatformView.Settings.AllowUniversalAccessFromFileURLs = true;

            });
#endif
        }

        private static void RegisterPDFViewerServices(MauiAppBuilder builder)
        {
#if IOS
            builder.Services.AddTransient<IGetAsyncFileService, FileServices>();
#endif
        }

        private static void RegisterUserDialogsServices(MauiAppBuilder builder)
        {
            // OS level settings
            builder.UseUserDialogs(() =>
              {
                  //setup your default styles for dialogs
                  AlertConfig.DefaultBackgroundColor = Colors.Purple;
#if ANDROID
                  AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular.ttf";
#else
                  AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular";
#endif

                  ToastConfig.DefaultCornerRadius = 15;
              });

            //builder.Services.AddSingleton(UserDialogs.Instance);

            builder.Services.AddSingleton(typeof(IUserDialogs),UserDialogs.Instance);
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
            builder.Services.AddTransient<StudentDetailsPage>();
            builder.Services.AddTransient<StudentDetailsViewModel>();
            builder.Services.AddTransient<CyclesPage>();
            builder.Services.AddTransient<CyclesViewModel>();
            builder.Services.AddTransient<ContractViewerPage>();

            builder.Services.AddTransient<ContractViewerViewModel>();
            builder.Services.AddTransient<ContractsViewModel>();
            builder.Services.AddTransient<ContractsPage>();
            builder.Services.AddSingleton(typeof(IFingerprint), Plugin.Fingerprint.CrossFingerprint.Current);
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddTransient<AddStudentPageViewModel>();
            builder.Services.AddTransient<AddStudentPage>();

            builder.Services.AddTransientWithShellRoute<PdfCreatorPage,PdfCreatorViewModel>(nameof(PdfCreatorPage));
            builder.Services.AddTransientWithShellRoute<PdfViewerPage,PdfViewerViewModel>(nameof(PdfViewerPage));

            builder.Services.AddSingleton<IFibAddGenericService, FibGenericAddService>();
            builder.Services.AddSingleton<IFibCyclesService, FibCyclesService>();

            builder.Services.AddSingleton<IFibLevelsService, FibLevelsService>();
            builder.Services.AddSingleton<IFibStatusService, FibStatusService>();
            builder.Services.AddSingleton<IFibContract, FibContractService>();
            //
          
            builder.Services.AddSingleton<IDevNotesService, FibService>();
            builder.Services.AddSingleton<IFibCRUDClients, FibCRUDClientsService>();
            builder.Services.AddSingleton<IFibStorageService, FibStorageService>();

            builder.Services.AddSingleton<IAlertService,AlertService>();
            builder.Services.AddSingleton<IFileService, FileService>();
            builder.Services.AddSingleton<IMyPopupService,MyPopupService>();
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

