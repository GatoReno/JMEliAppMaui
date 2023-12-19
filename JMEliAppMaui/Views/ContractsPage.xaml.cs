using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class ContractsPage : ContentPage
{
	public ContractsPage(ContractsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
