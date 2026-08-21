using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Views;

namespace JMEliAppMaui.ViewModels
{
    public class ReviewRequestsViewModel : DataViewModel
    {
        public ObservableCollection<EnrollmentRequest> Requests { get; set; } = new();
        public ObservableCollection<EnrollmentRequest> RejectedRequests { get; set; } = new();
        public bool HasRejected => RejectedRequests.Count > 0;
        public ICommand ViewRequestCommand { get; }

        public ReviewRequestsViewModel(IFirebaseService firebase) : base(firebase)
        {
            ViewRequestCommand = new Command<EnrollmentRequest>(OnViewRequest);
        }

        protected override async Task LoadDataAsync()
        {
            var all = await Firebase.GetAllAsync<EnrollmentRequest>("EnrollmentRequests");

            Requests.Clear();
            RejectedRequests.Clear();

            foreach (var r in all)
            {
                if (r.Status == RequestStatus.Pendiente || r.Status == RequestStatus.EnRevision)
                    Requests.Add(r);
                else if (r.Status == RequestStatus.Rechazada)
                    RejectedRequests.Add(r);
            }

            IsEmpty = Requests.Count == 0;
            OnPropertyChanged(nameof(HasRejected));
        }

        protected override bool ShouldReloadOnAppearing() => true;

        private async void OnViewRequest(EnrollmentRequest request)
        {
            if (request == null) return;
            await Shell.Current.GoToAsync(nameof(ReviewRequestDetailPage), true,
                new Dictionary<string, object> { { "Request", request } });
        }
    }
}
