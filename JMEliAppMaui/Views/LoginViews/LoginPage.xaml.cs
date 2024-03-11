using JMEliAppMaui.ViewModels.LoginViewModels;

namespace JMEliAppMaui.Views.LoginViews;

public partial class LoginPage : ContentPage
{
    LoginViewModel viewModel;
    public LoginPage(LoginViewModel vm)
	{
        BindingContext = viewModel = vm;
		InitializeComponent();
	}

    async void Switch_Toggled(System.Object sender, Microsoft.Maui.Controls.ToggledEventArgs e)
    {
        if (e.Value)
        {
            viewModel.StayLoggedCommand.Execute(null);
            await splashImage.FadeTo(1, 150, null);
            await splashImage.ScaleTo(1, 1000); //Time-consuming processes such as initialization
            await splashImage.ScaleTo(0.6, 1500, Easing.BounceOut);
            await splashImage.FadeTo(0, 270, null);
            viewModel.NavigationCommand.Execute(null);
        }
        else
        {
            Preferences.Clear(); 
        }
        //StayLoggedCommand
    }

   async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        viewModel.LoginCommand.Execute(null);
        await splashImage.FadeTo(1, 150, null);
        await splashImage.ScaleTo(1, 1000); //Time-consuming processes such as initialization
        await splashImage.ScaleTo(0.6, 1500, Easing.BounceOut);
        await splashImage.FadeTo(0, 270, null);
        viewModel.NavigationCommand.Execute(null);
    }

    protected override void OnAppearing()
    {
        viewModel.AppearingCommand.Execute(null);
        base.OnAppearing();
    }



}

