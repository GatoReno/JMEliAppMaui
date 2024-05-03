using JMEliAppMaui.Services.Abstractions;


namespace JMEliAppMaui.Services.Implementations
{
    public class AlertService : IAlertService
    {
        public AlertService() { }
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            Page page = Shell.Current?.CurrentPage ?? throw new InvalidOperationException("Application.Current.MainPage cannot be null.");
            return page.DisplayAlert(title, message, cancel);
        }

        public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            Page page = Shell.Current?.CurrentPage ?? throw new InvalidOperationException("Application.Current.MainPage cannot be null.");
            return page.DisplayAlert(title, message, accept, cancel);
        }
    }
}
