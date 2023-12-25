using JMEliAppMaui.ViewModels.StudentsViewModels;

namespace JMEliAppMaui.Views;
[XamlCompilation(XamlCompilationOptions.Skip)]
public partial class AddStudentPage : ContentPage
{
	public AddStudentPage(AddStudentPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    
}
