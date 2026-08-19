using JMEliAppMaui.Views;

namespace JMEliAppMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Only register routes for PUSH navigation pages (not Shell tab/flyout pages)
        // Shell pages (CycleDashboardPage, ClientsPage, LevelsPage, CyclesPage, ContractsPage, AuthorizedPickupPage)
        // are already routed via their ShellContent Route="..." in XAML.
        Routing.RegisterRoute(nameof(ClientDetailsPage), typeof(ClientDetailsPage));
        Routing.RegisterRoute(nameof(AddStudentPage), typeof(AddStudentPage));
        Routing.RegisterRoute(nameof(StudentDetailsPage), typeof(StudentDetailsPage));
        Routing.RegisterRoute(nameof(ContractViewerPage), typeof(ContractViewerPage));
        Routing.RegisterRoute(nameof(SelectClientPage), typeof(SelectClientPage));
    }
}
