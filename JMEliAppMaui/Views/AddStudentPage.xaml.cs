using JMEliAppMaui.ViewModels.StudentsViewModels;

namespace JMEliAppMaui.Views;

public partial class AddStudentPage : ContentPage
{
	public AddStudentPage(AddStudentPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
