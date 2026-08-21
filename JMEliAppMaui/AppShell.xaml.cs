using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Views;

namespace JMEliAppMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ClientDetailsPage), typeof(ClientDetailsPage));
        Routing.RegisterRoute(nameof(AddStudentPage), typeof(AddStudentPage));
        Routing.RegisterRoute(nameof(StudentDetailsPage), typeof(StudentDetailsPage));
        Routing.RegisterRoute(nameof(ContractViewerPage), typeof(ContractViewerPage));
        Routing.RegisterRoute(nameof(SelectClientPage), typeof(SelectClientPage));
        Routing.RegisterRoute(nameof(ReviewRequestDetailPage), typeof(ReviewRequestDetailPage));

        // Check pending requests badge on appearing
        this.Navigated += OnShellNavigated;
    }

    private async void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        await UpdateRequestsBadge();
    }

    private async Task UpdateRequestsBadge()
    {
        try
        {
            var firebase = App.Current?.Handler?.MauiContext?.Services.GetService<IFirebaseService>();
            if (firebase == null) return;

            var pending = await firebase.GetWhereAsync<EnrollmentRequest>(
                "EnrollmentRequests", r => r.Status == RequestStatus.Pendiente || r.Status == RequestStatus.EnRevision);

            var count = pending.Count;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SolicitudesFlyout.Title = count > 0
                    ? $"📨 Solicitudes ({count})"
                    : "📨 Solicitudes";
            });
        }
        catch { }
    }
}
