using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Views;

namespace JMEliAppMaui.ViewModels
{
    public class SearchStudentsViewModel : DataViewModel
    {
        private string? _searchText;
        private List<StudentModel> _all = new();

        public string? SearchText { get => _searchText; set { _searchText = value; OnPropertyChanged(); ApplyFilter(); } }
        public ObservableCollection<StudentModel> Results { get; set; } = new();
        public ICommand ViewStudentCommand { get; }

        public SearchStudentsViewModel(IFirebaseService firebase) : base(firebase)
        {
            ViewStudentCommand = new Command<StudentModel>(OnView);
        }

        protected override async Task LoadDataAsync()
        {
            _all = (await Firebase.GetAllAsync<StudentModel>("Students")).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            Results.Clear();
            var source = string.IsNullOrWhiteSpace(SearchText)
                ? _all
                : _all.Where(s => s.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true).ToList();
            foreach (var s in source)
                Results.Add(s);
        }

        private async void OnView(StudentModel s)
        {
            if (s == null) return;
            await Shell.Current.GoToAsync(nameof(StudentDetailsPage), true,
                new Dictionary<string, object> { { "Student", s } });
        }
    }
}
