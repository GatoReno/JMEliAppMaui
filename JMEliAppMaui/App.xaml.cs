using JMEliAppMaui.Views.LoginViews;

namespace JMEliAppMaui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();


        bool isLogged = Preferences.Get("IsLogged", false);
        if (isLogged)
        {
            MainPage = new AppShell();
        }
        else
        {
            MainPage = new LoginPage(new ViewModels.LoginViewModels.LoginViewModel());
        }
             
        
	}

    protected override void OnResume()
    {
        base.OnResume();
    }
}

