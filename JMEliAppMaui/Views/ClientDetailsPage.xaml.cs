using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class ClientDetailsPage : ContentPage
{
    private ClientDetailsViewModel ViewModel;

    public ClientDetailsPage(ClientDetailsViewModel vm)
	{
		BindingContext = ViewModel = vm;
		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnAppearing()
    {
       
        ViewModel.OnAppearingCommand.Execute(null);
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        ViewModel.Contracts = null;
        ViewModel.StudentList = null;
        base.OnDisappearing();
    }
}
