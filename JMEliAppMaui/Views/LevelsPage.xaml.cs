using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class LevelsPage : ContentPage
{
	public LevelsPage(LevelsPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
