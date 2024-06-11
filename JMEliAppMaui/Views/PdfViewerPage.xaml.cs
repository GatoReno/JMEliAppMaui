using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class PdfViewerPage : ContentPage
{
	public PdfViewerPage(PdfViewerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}