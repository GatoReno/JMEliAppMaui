using Controls.UserDialogs.Maui;
using JMEliAppMaui.ViewModels.LoginViewModels;
using JMEliAppMaui.Views.LoginViews;
using Plugin.Fingerprint;

namespace JMEliAppMaui;

public partial class App : Application
{
    private static App instance;
    public static App Instance { get { return instance; } }
    public App()
	{
		InitializeComponent();
        instance = this;

        bool isLogged = Preferences.Get("IsLogged", false);
        if (isLogged)
        {
            MainPage = new AppShell();
        }
        else
        {
            LoginPageNavigation();
        }
             
        
	}

    public void LoginPageNavigation()
    {
        MainPage = new LoginPage(new LoginViewModel
                        (CrossFingerprint.Current,UserDialogs.Instance));
    } 

    protected override void OnResume()
    {
        base.OnResume();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);
    }
}

