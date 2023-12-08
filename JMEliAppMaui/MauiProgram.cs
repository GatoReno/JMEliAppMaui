using JMEliAppMaui.ProgramHelpers;
using JMEliAppMaui.ViewModels;
 using Microsoft.Extensions.Logging;

namespace JMEliAppMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.ConfigureServices();
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
        static TimeSpan SleepDurationProvider(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));

    }


     
}

