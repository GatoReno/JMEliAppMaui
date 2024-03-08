using System;
using System.Windows.Input;

namespace JMEliAppMaui.ViewModels.LoginViewModels
{
	public class LoginViewModel : BindableObject
	{
        #region commands and implementeations
        public ICommand StayLoggedCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand NavigationCommand { get; set; }

        #endregion
        private bool _Loading, _InfoEntries;
        public bool Loading
        {
            get => _Loading;
            set { _Loading = value; OnPropertyChanged(); }
        }
        public bool InfoEntries
        {
            get => _InfoEntries;
            set { _InfoEntries = value; OnPropertyChanged(); }
        }


        public LoginViewModel()
		{
            LoginCommand = new Command(OnLoginCommand);
            Loading = false;
            InfoEntries = true;
            StayLoggedCommand = new Command(OnStayLoggedCommand);
            NavigationCommand = new Command(OnNavigationCommand);
        }

        private void OnNavigationCommand()
        {
            App.Current.MainPage = new AppShell();

        }
 

        Task LoadingProcess()
        {
            InfoEntries = false;
            Loading = true;
            
            return Task.Delay(3000);

        }

       
        async void OnStayLoggedCommand(object obj)
        {
            Preferences.Set("IsLogged", true);
            await LoadingProcess();
            
        } 
        async void OnLoginCommand(object obj)
        {
            //throw new NotImplementedException();
            await LoadingProcess();
             
        }
    }
}

