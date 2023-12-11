using JMEliAppMaui.Views;

namespace JMEliAppMaui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(Notes4DevPage), typeof(Notes4DevPage));
        Routing.RegisterRoute(nameof(ClientDetailsPage), typeof(ClientDetailsPage));

        Routing.RegisterRoute(nameof(ClientsPage), typeof(ClientsPage));
    }
}

