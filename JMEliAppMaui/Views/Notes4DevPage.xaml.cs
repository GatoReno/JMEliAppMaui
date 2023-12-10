using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class Notes4DevPage : ContentPage
{
	public Notes4DevPage(Notes4DevViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
