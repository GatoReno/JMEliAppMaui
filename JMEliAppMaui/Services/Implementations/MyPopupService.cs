using CommunityToolkit.Maui.Views;
using JMEliAppMaui.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMEliAppMaui.Services.Implementations
{
    public class MyPopupService : IMyPopupService
    {
        public void Close(Popup popup)
        {
            popup.Close();
        }

        public void Show(Popup popup)
        {
            Page page = Application.Current?.MainPage ?? throw new InvalidOperationException("Application.Current.MainPage cannot be null.");
            page.ShowPopup(popup);
        }

        public async Task<object> ShowPopupAsync(Popup popup)
        {
            Page page = Application.Current?.MainPage ?? throw new InvalidOperationException("Application.Current.MainPage cannot be null.");
            return await page.ShowPopupAsync(popup, CancellationToken.None);
        }
    }
}
