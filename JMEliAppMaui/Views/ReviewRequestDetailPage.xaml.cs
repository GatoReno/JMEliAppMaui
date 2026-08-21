using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class ReviewRequestDetailPage : ContentPage
{
    public ReviewRequestDetailPage(ReviewRequestDetailViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}
