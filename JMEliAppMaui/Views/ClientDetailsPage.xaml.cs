using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class ClientDetailsPage : ContentPage
{
	public ClientDetailsPage(ClientDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
