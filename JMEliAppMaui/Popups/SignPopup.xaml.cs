using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace JMEliAppMaui.Popups;

public partial class SignPopup : Popup
{
    public ObservableCollection<IDrawingLine> Lines { get; set; } = new ObservableCollection<IDrawingLine>();
    public SignPopup()
	{
		InitializeComponent();
        BindingContext = this;
    }
    async void CancelButton_Clicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(token: cts.Token);
    }

    async void SaveButton_Clicked(object? sender, EventArgs e)
    {
        if (Lines.Count > 0)
        {
            var sign = await DrawingView.GetImageStream(Lines, new Size(200, 100), Colors.Transparent);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await CloseAsync(sign, cts.Token);
        }
    }

    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        Lines.Clear();
    }
}