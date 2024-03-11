using System;
using System.Windows.Input;
using Controls.UserDialogs.Maui;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions; 

namespace JMEliAppMaui.ViewModels.LoginViewModels
{
	public class LoginViewModel : BindableObject
	{
        private readonly IFingerprint _fingerprint;
        readonly IUserDialogs _userDialogs;
        #region commands and implementeations
        public ICommand StayLoggedCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand NavigationCommand { get; set; }
        public ICommand BiometricLogCommand { get; set; }
        public ICommand AppearingCommand { get; set; }

        #endregion
        private bool _Loading, _InfoEntries;

        private string _BiometricText, _biometricType;
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
      
        public string BiometricText
        {
            get => _BiometricText;
            set { _BiometricText = value; OnPropertyChanged(); }
        }

        public string BiometricType
        {
            get => _biometricType;
            set { _biometricType = value; OnPropertyChanged(); }
        }
        

        public LoginViewModel(IFingerprint fingerprint,IUserDialogs userDialogs)
		{
            _fingerprint = fingerprint;
            _userDialogs = userDialogs;
            LoginCommand = new Command(OnLoginCommand);
            Loading = false;
            InfoEntries = true;
            StayLoggedCommand = new Command(OnStayLoggedCommand);
            NavigationCommand = new Command(OnNavigationCommand);
            AppearingCommand = new Command(OnAppearingCommand);
            BiometricLogCommand = new Command(OnBiometricLogCommand);
        }

        async void OnBiometricLogCommand()
        {
            InfoEntries = false;
            Loading = true;
            var request = new AuthenticationRequestConfiguration("Biometic Auth",$"Use {BiometricType} , grant access?");
            var result = await _fingerprint.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                Preferences.Set("BiometricAuthenticated",true);
                 
                await LoadingProcess();
                OnNavigationCommand();
            }
            else {
                Vibration.Vibrate();
                _userDialogs.ShowToast("Please try again");
                InfoEntries = true;
                Loading = false;
            }
        }

        async void OnAppearingCommand()
        {
            var hasBiometric = await _fingerprint.IsAvailableAsync();
            var _biometricType = await _fingerprint.GetAuthenticationTypeAsync();
            BiometricText = $"Use {_biometricType.ToString()}";
            BiometricType = _biometricType.ToString();

            bool loginBiometric = Preferences.Get("BiometricAuthenticated", false);
            if (loginBiometric)
            {
                OnBiometricLogCommand();
            }
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

