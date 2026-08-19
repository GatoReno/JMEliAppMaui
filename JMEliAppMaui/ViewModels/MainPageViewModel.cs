using System;

namespace JMEliAppMaui.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private string _title;
        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }

        public MainPageViewModel()
        {
            Title = "Joan Miró - Admin";
        }
    }
}

