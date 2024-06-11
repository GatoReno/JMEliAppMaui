using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class PdfCreatorPage : ContentPage
{
	public PdfCreatorPage(PdfCreatorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}