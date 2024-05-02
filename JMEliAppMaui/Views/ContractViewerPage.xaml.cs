using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class ContractViewerPage : ContentPage
{
	public ContractViewerPage(ContractViewerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}