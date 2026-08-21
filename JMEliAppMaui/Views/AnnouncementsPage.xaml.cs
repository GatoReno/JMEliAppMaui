using JMEliAppMaui.ViewModels;
namespace JMEliAppMaui.Views;
public partial class AnnouncementsPage : ContentPage
{
    public AnnouncementsPage(AnnouncementsViewModel vm) { BindingContext = vm; InitializeComponent(); }
    protected override void OnAppearing() { base.OnAppearing(); if (BindingContext is DataViewModel d) d.AppearingCommand.Execute(null); }
}
