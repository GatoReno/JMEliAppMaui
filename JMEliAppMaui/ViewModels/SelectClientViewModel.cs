using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Views;

namespace JMEliAppMaui.ViewModels
{
    /// <summary>
    /// Pantalla de selección de cliente para el flujo de inscripción.
    /// Al tocar un cliente → navega a AddStudentPage con ese cliente.
    /// </summary>
    public class SelectClientViewModel : DataViewModel
    {
        private string? _searchText;
        private List<ClientModel> _allClients = new();

        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public ObservableCollection<ClientModel> FilteredClients { get; set; } = new();

        public ICommand SelectClientCommand { get; }
        public ICommand GoToAddClientCommand { get; }

        public SelectClientViewModel(IFirebaseService firebase) : base(firebase)
        {
            SelectClientCommand = new Command<ClientModel>(OnSelectClient);
            GoToAddClientCommand = new Command(OnGoToAddClient);
        }

        protected override async Task LoadDataAsync()
        {
            var clients = await Firebase.GetAllAsync<ClientModel>("Clients");
            _allClients = clients.ToList();
            ApplyFilter();
        }

        protected override bool ShouldReloadOnAppearing() => true;

        private void ApplyFilter()
        {
            FilteredClients.Clear();

            var source = string.IsNullOrWhiteSpace(SearchText)
                ? _allClients
                : _allClients.Where(c =>
                    (c.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

            foreach (var client in source)
                FilteredClients.Add(client);
        }

        private async void OnSelectClient(ClientModel client)
        {
            if (client == null) return;

            // Navigate to AddStudentPage with this client
            await Shell.Current.GoToAsync(nameof(AddStudentPage), true,
                new Dictionary<string, object>
                {
                    { "Client", client }
                });
        }

        private async void OnGoToAddClient()
        {
            await Shell.Current.GoToAsync($"//{nameof(ClientsPage)}");
        }
    }
}
