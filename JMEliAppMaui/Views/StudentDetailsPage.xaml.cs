using JMEliAppMaui.ViewModels.StudentsViewModels;

namespace JMEliAppMaui.Views;

public partial class StudentDetailsPage : ContentPage
{
	private StudentDetailsViewModel ViewModel;
	public StudentDetailsPage(StudentDetailsViewModel vm)
	{
		BindingContext = ViewModel = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        ViewModel.AppearingCommand.Execute(null);
        base.OnAppearing();
       
    }
}
