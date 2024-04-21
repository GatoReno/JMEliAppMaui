using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Views;

namespace JMEliAppMaui.ViewModels
{
	public class ClientsViewModel : BindableObject
    {
        #region props
        private bool _searchOrPlus, _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }

        public bool IsSearch { get => _searchOrPlus; set { _searchOrPlus = value; OnPropertyChanged(); }  }
        private bool _orPlus;
        public bool OrPlus { get => _orPlus; set { _orPlus = value; OnPropertyChanged(); } }
        public ObservableCollection<ClientModel> ClientList { get; set; }

        public ICommand AddCommand { get; private set; }
        public ICommand AddBackCommand { get; private set; }
        public ICommand DetailsClientCommand { get; private set; }
        public ICommand AppearingCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        #endregion

        #region add client props
        private string _fullname,_searchText, _scholarity,_ocupation,_email,_phone,_office,_relationship,_work,_address,
            _imageUrl;

        public string SearchText
        {
            get => _searchText;
            set {
                _searchText = value;
                OnPropertyChanged();
                if (_searchText.Length > 0)
                {
                    OnSearchCommand();
                }
                else {
                    GetClients();
                }
            }

        }
        public string FullName { get => _fullname; set { _fullname = value; OnPropertyChanged(); } }
        
        public string Scholarity { get => _scholarity; set { _scholarity= value; OnPropertyChanged(); } }
        public string Ocupation { get => _ocupation; set { _ocupation = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email= value; OnPropertyChanged(); } }
        public string Phone { get => _phone; set { _phone= value; OnPropertyChanged(); } }
        public string Office { get => _office; set { _office= value; OnPropertyChanged(); } }
        public string Relationship { get => _relationship; set {_relationship= value; OnPropertyChanged(); } }
        public string Work { get => _work; set { _work = value; OnPropertyChanged(); } }
        public string Address { get => _address; set { _address= value; OnPropertyChanged(); } }
        #endregion

        private IFibCRUDClients _fibCRUDClients;

        public ClientsViewModel(IFibCRUDClients fibCRUDClients)
		{
			this._fibCRUDClients = fibCRUDClients;
            IsSearch = true;
            OrPlus = !IsSearch;
            AddCommand = new Command(OnAddCommand);
            AddBackCommand = new Command(OnAddBackCommand);
            SearchCommand = new Command(OnSearchCommand);
            DetailsClientCommand = new Command<ClientModel>(OnDetailsClientCommand);
            ClientList = new ObservableCollection<ClientModel>();
            AppearingCommand = new Command(OnAppearingCommand);
            AppearingCommand.Execute(null);
            IsLoading = true;
        }

        private void OnSearchCommand()
        {
            var foundClients = ClientList.Where(found =>
            found.Email.Contains(SearchText) ||
            found.FullName.Contains(SearchText)
            ).ToList();

            ClientList.Clear();
            foreach (var client in foundClients)
            {
                ClientList.Add(client);
            }
        }

        private  void OnAppearingCommand( )
        {
            GetClients();
        }

        async void GetClients()
        {
            IsLoading = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "need internet to procede, check your conectivity", "ok");

                return;
            }
            ClientList.Clear();
           
            var clients = await _fibCRUDClients.GetClients();
            foreach (var item in clients)
            {
                ClientList.Add(item);
            }
            IsLoading = false;
        }

        private async void OnDetailsClientCommand(ClientModel client)
        {
            if (string.IsNullOrEmpty(client.UrlImage))
            {
                client.UrlImage = "user_icon.png";
            }
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Client",client}
            };
            await AppShell.Current.GoToAsync(nameof(ClientDetailsPage),true,parameters);
        }

        private void OnAddBackCommand(object obj)
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
                    await App.Current.MainPage.DisplayAlert("Error", "need full name and email to procede", "ok");
                    return;
                }
                else { 

                ClientModel newClient = new ClientModel()
                {
                    FullName = FullName,
                    Email =Email,
                    Ocupation = Ocupation,
                    Phone=Phone,
                    Scholarity = Scholarity,
                    Status = "alta",
                    State = "",
                    Work = Work,
                    Relationship =Relationship,
                    Address = Address,
                    Office = Office
                };

                   var id = await _fibCRUDClients.AddClient(newClient);
                    if (!string.IsNullOrEmpty(id))
                    {
                        await App.Current.MainPage.DisplayAlert("Success", $"client with {Email} and full name : {FullName} added", "ok");
                        newClient.Id = id;
                        ClientList.Add(newClient);
                    }
                    IsSearch = true;
                    OrPlus = false;
                    return;
                }
            }

            OrPlus = true;
        }
    }
}

