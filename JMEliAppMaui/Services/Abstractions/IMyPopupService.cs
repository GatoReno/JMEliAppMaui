using CommunityToolkit.Maui.Views;

namespace JMEliAppMaui.Services.Abstractions
{
    public interface IMyPopupService
    {
        void Show(Popup popup);
        void Close(Popup popup);
        Task<object> ShowPopupAsync(Popup popup);
    }
}
