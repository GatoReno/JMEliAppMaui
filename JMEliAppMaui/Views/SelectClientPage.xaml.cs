using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class SelectClientPage : ContentPage
{
    public SelectClientPage(SelectClientViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DataViewModel dvm)
            dvm.AppearingCommand.Execute(null);
    }
}
