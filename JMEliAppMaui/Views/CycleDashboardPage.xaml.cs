using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class CycleDashboardPage : ContentPage
{
    public CycleDashboardPage(CycleDashboardViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CycleDashboardViewModel dvm)
            dvm.AppearingCommand.Execute(null);
    }
}
