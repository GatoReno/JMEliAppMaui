using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class ClientsPage : ContentPage
{
	public ClientsPage(ClientsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
