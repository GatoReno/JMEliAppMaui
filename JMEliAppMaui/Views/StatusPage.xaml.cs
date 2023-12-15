using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class StatusPage : ContentPage
{
    private StatusPageViewModel ViewModel;
    public StatusPage(StatusPageViewModel vm)
	{
        BindingContext = ViewModel = vm;
		InitializeComponent();
	}

   

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {//Green
        ViewModel.OnSetSelectedColorCommand("Green");
    }

    void Button_Clicked_1(System.Object sender, System.EventArgs e)
    {
        ViewModel.OnSetSelectedColorCommand("IndianRed");

    }

    void Button_Clicked_2(System.Object sender, System.EventArgs e)
    {
        ViewModel.OnSetSelectedColorCommand("Gold");

    }

    void PlusButtonClicled(System.Object sender, System.EventArgs e)
    {
        ViewModel.OnAddStatusCommand(null); 

    }

    void BackButtonClicked(System.Object sender, System.EventArgs e)
    {
        ViewModel.OnBackCommand(null);
    }
}
