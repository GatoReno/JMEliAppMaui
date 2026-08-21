using JMEliAppMaui.Models;
using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui.Views;

public partial class SearchStudentsPage : ContentPage
{
    public SearchStudentsPage(SearchStudentsViewModel vm) { BindingContext = vm; InitializeComponent(); }

    protected override void OnAppearing() { base.OnAppearing(); if (BindingContext is DataViewModel d) d.AppearingCommand.Execute(null); }

    private async void OnStudentSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is StudentModel student)
        {
            // Clear selection so tap works again next time
            ((CollectionView)sender).SelectedItem = null;

            await Shell.Current.GoToAsync(nameof(StudentDetailsPage), true,
                new Dictionary<string, object> { { "Student", student } });
        }
    }
}
