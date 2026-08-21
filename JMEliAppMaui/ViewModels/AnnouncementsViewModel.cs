using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels
{
    public class AnnouncementsViewModel : DataViewModel
    {
        public static readonly NotNullToBoolConverter NotNullConverter = new();

        private bool _isFormVisible;
        private string? _newTitle, _newBody, _newLocation, _newFacebook, _newInstagram, _newTiktok;
        private DateTime _eventDate = DateTime.Today;
        private TimeSpan _startTime = new(9, 0, 0);
        private TimeSpan _endTime = new(10, 0, 0);

        public bool IsFormVisible { get => _isFormVisible; set { _isFormVisible = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsFabVisible)); } }
        public bool IsFabVisible => !IsFormVisible;
        public string? NewTitle { get => _newTitle; set { _newTitle = value; OnPropertyChanged(); } }
        public string? NewBody { get => _newBody; set { _newBody = value; OnPropertyChanged(); OnPropertyChanged(nameof(CharCount)); } }
        public string CharCount => $"{250 - (NewBody?.Length ?? 0)} caracteres restantes";
        public string? NewLocation { get => _newLocation; set { _newLocation = value; OnPropertyChanged(); } }
        public string? NewFacebook { get => _newFacebook; set { _newFacebook = value; OnPropertyChanged(); } }
        public string? NewInstagram { get => _newInstagram; set { _newInstagram = value; OnPropertyChanged(); } }
        public string? NewTiktok { get => _newTiktok; set { _newTiktok = value; OnPropertyChanged(); } }
        public DateTime EventDate { get => _eventDate; set { _eventDate = value; OnPropertyChanged(); } }
        public TimeSpan StartTime { get => _startTime; set { _startTime = value; OnPropertyChanged(); } }
        public TimeSpan EndTime { get => _endTime; set { _endTime = value; OnPropertyChanged(); } }
        public DateTime Today => DateTime.Today;

        public ObservableCollection<AnnouncementModel> Announcements { get; set; } = new();
        public ICommand AddCommand { get; }
        public ICommand CancelFormCommand { get; }
        public ICommand PublishCommand { get; }

        public AnnouncementsViewModel(IFirebaseService firebase) : base(firebase)
        {
            AddCommand = new Command(() => IsFormVisible = true);
            CancelFormCommand = new Command(OnCancel);
            PublishCommand = new Command(OnPublish);
        }

        protected override async Task LoadDataAsync()
        {
            var all = await Firebase.GetAllAsync<AnnouncementModel>("Announcements");
            Announcements.Clear();
            foreach (var a in all.Where(a => a.IsActive).OrderByDescending(a => a.CreatedDate))
                Announcements.Add(a);
        }

        protected override bool ShouldReloadOnAppearing() => true;

        private void OnCancel()
        {
            IsFormVisible = false;
            ClearForm();
        }

        private async void OnPublish()
        {
            if (string.IsNullOrWhiteSpace(NewTitle) || string.IsNullOrWhiteSpace(NewBody))
            {
                await Shell.Current.DisplayAlert("Error", "Título y mensaje son obligatorios", "OK");
                return;
            }

            var announcement = new AnnouncementModel
            {
                Title = NewTitle,
                Body = NewBody,
                EventDate = EventDate != DateTime.Today ? EventDate.ToString("dd/MM/yyyy") : null,
                StartTime = StartTime != new TimeSpan(9, 0, 0) ? StartTime.ToString(@"hh\:mm") : null,
                EndTime = EndTime != new TimeSpan(10, 0, 0) ? EndTime.ToString(@"hh\:mm") : null,
                Location = string.IsNullOrWhiteSpace(NewLocation) ? null : NewLocation,
                FacebookUrl = string.IsNullOrWhiteSpace(NewFacebook) ? null : NewFacebook,
                InstagramUrl = string.IsNullOrWhiteSpace(NewInstagram) ? null : NewInstagram,
                TiktokUrl = string.IsNullOrWhiteSpace(NewTiktok) ? null : NewTiktok,
                CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                IsActive = true
            };

            var id = await Firebase.AddAsync(announcement, "Announcements");
            announcement.Id = id;
            await Firebase.UpdateAsync(announcement, "Announcements", id);

            Announcements.Insert(0, announcement);
            IsFormVisible = false;
            ClearForm();
            await Shell.Current.DisplayAlert("✅", "Anuncio publicado", "OK");
        }

        private void ClearForm()
        {
            NewTitle = NewBody = NewLocation = NewFacebook = NewInstagram = NewTiktok = null;
            EventDate = DateTime.Today;
            StartTime = new TimeSpan(9, 0, 0);
            EndTime = new TimeSpan(10, 0, 0);
        }
    }

    public class NotNullToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type t, object? p, CultureInfo c) => value != null && !string.IsNullOrEmpty(value.ToString());
        public object ConvertBack(object? value, Type t, object? p, CultureInfo c) => throw new NotImplementedException();
    }
}
