using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class AuthorizedPickupPage : ContentPage
{
    public AuthorizedPickupPage(AuthorizedPickupViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AuthorizedPickupViewModel vm)
            vm.AppearingCommand.Execute(null);
    }
}
