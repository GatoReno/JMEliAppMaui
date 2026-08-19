using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Views;

namespace JMEliAppMaui.ViewModels
{
    public class ClientsViewModel : DataViewModel
    {
        #region Properties

        private bool _isSearch = true;
        private bool _orPlus;
        private string? _searchText;
        private string? _fullname, _scholarship, _occupation, _email, _phone, _office, _relationship, _work, _address;

        public bool IsSearch { get => _isSearch; set { _isSearch = value; OnPropertyChanged(); } }
        public bool OrPlus { get => _orPlus; set { _orPlus = value; OnPropertyChanged(); } }

        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(_searchText))
                    ApplySearch();
                else
                    RestoreFullList();
            }
        }

        public string? FullName { get => _fullname; set { _fullname = value; OnPropertyChanged(); } }
        public string? Scholarship { get => _scholarship; set { _scholarship = value; OnPropertyChanged(); } }
        public string? Occupation { get => _occupation; set { _occupation = value; OnPropertyChanged(); } }
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string? Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        public string? Office { get => _office; set { _office = value; OnPropertyChanged(); } }
        public string? Relationship { get => _relationship; set { _relationship = value; OnPropertyChanged(); } }
        public string? Work { get => _work; set { _work = value; OnPropertyChanged(); } }
        public string? Address { get => _address; set { _address = value; OnPropertyChanged(); } }

        public ObservableCollection<ClientModel> ClientList { get; set; } = new();

        #endregion

        #region Commands

        public ICommand AddCommand { get; }
        public ICommand AddBackCommand { get; }
        public ICommand DetailsClientCommand { get; }
        public ICommand SearchCommand { get; }

        #endregion

        // Backing list for search (so we don't lose the full list)
        private List<ClientModel> _allClients = new();

        public ClientsViewModel(IFirebaseService firebase) : base(firebase)
        {
            AddCommand = new Command(OnAddCommand);
            AddBackCommand = new Command(OnAddBackCommand);
            SearchCommand = new Command(ApplySearch);
            DetailsClientCommand = new Command<ClientModel>(OnDetailsClientCommand);
        }

        /// <summary>
        /// Loads all clients from Firebase. Called automatically by DataViewModel on OnAppearing.
        /// </summary>
        protected override async Task LoadDataAsync()
        {
            var clients = await Firebase.GetAllAsync<ClientModel>("Clients");
            _allClients = clients.ToList();

            ClientList.Clear();
            foreach (var client in _allClients)
            {
                ClientList.Add(client);
            }

            IsEmpty = _allClients.Count == 0;
        }

        /// <summary>Always reload on appearing — user may have added a client from another screen.</summary>
        protected override bool ShouldReloadOnAppearing() => true;

        #region Search

        private void ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var found = _allClients.Where(c =>
                (c.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();

            ClientList.Clear();
            foreach (var client in found)
                ClientList.Add(client);
        }

        private void RestoreFullList()
        {
            ClientList.Clear();
            foreach (var client in _allClients)
                ClientList.Add(client);
        }

        #endregion

        #region Navigation & Add

        private async void OnDetailsClientCommand(ClientModel client)
        {
            if (string.IsNullOrEmpty(client.UrlImage))
                client.UrlImage = "user_icon.png";

            await AppShell.Current.GoToAsync(nameof(ClientDetailsPage), true,
                new Dictionary<string, object> { { "Client", client } });
        }

        private void OnAddBackCommand()
        {
            IsSearch = true;
            OrPlus = false;
        }

        private async void OnAddCommand()
        {
            IsSearch = false;

            if (OrPlus)
            {
                if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Email))
                {
                    await Shell.Current.DisplayAlert("Error", "Nombre y email son obligatorios", "OK");
                    return;
                }

                var newClient = new ClientModel
                {
                    FullName = FullName,
                    Email = Email,
                    Occupation = Occupation,
                    Phone = Phone,
                    Scholarship = Scholarship,
                    Status = "alta",
                    State = "",
                    Work = Work,
                    Relationship = Relationship,
                    Address = Address,
                    Office = Office
                };

                try
                {
                    var id = await Firebase.AddAsync(newClient, "Clients");
                    newClient.Id = id;
                    await Firebase.UpdateAsync(newClient, "Clients", id);

                    _allClients.Add(newClient);
                    ClientList.Add(newClient);

                    await Shell.Current.DisplayAlert("Éxito", $"Cliente {FullName} agregado", "OK");

                    // Clear form
                    FullName = null; Email = null; Occupation = null;
                    Phone = null; Scholarship = null; Work = null;
                    Relationship = null; Address = null; Office = null;
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
                }

                IsSearch = true;
                OrPlus = false;
                IsEmpty = false;
                return;
            }

            OrPlus = true;
        }

        #endregion
    }
}
