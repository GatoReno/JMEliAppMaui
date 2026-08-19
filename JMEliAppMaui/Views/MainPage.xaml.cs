using JMEliAppMaui.ViewModels;
using JMEliAppMaui.Views;

namespace JMEliAppMaui;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    async void OnClientsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(ClientsPage)}");
    }

    async void OnLevelsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LevelsPage)}");
    }

    async void OnCyclesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(CyclesPage)}");
    }

    async void OnContractsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(ContractsPage)}");
    }

    async void OnAuthorizedClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(AuthorizedPickupPage)}");
    }

    async void OnCycleDashboardClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(CycleDashboardPage)}");
    }

    void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        App.Instance.LoginPageNavigation();
    }
}

