using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class CyclesPage : ContentPage
{
	public CyclesPage(CyclesViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
