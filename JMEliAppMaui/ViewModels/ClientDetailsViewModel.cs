using System;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using JMEliAppMaui.Views;

namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(Client), "Client")]
    public class ClientDetailsViewModel : BindableObject
    {
        #region props
        private ClientModel _clientUser;

        public ClientModel Client
        { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }

        private bool _isEdit, _isLoading, _IsLoadingRequierements;
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsLoadingRequierements { get => _IsLoadingRequierements; set { _IsLoadingRequierements = value; OnPropertyChanged(); } }


        
        private bool _isEditVisible;
        public bool IsEditVisible { get => _isEditVisible; set { _isEditVisible = value; OnPropertyChanged(); } }

        private bool _isPayment;
        public bool IsPayments { get => _isPayment; set { _isPayment = value; OnPropertyChanged(); } }

        private bool _isContract;
        public bool IsContract { get => _isContract; set { _isContract = value; OnPropertyChanged(); } }

        private bool _isAddStudent;
        public bool IsAddStudent { get => _isAddStudent; set { _isAddStudent = value; OnPropertyChanged(); } }
        private string _imageUrl;
        public string ImageUrl { get => _imageUrl; set { _imageUrl = value; OnPropertyChanged(); } }

        public ICommand EditCommand { get; private set; }
        public ICommand ContractCommand { get; private set; }
        public ICommand AddStudentCommand { get; private set; }
        public ICommand PaymentsCommand { get; private set; }

        public ICommand UpdateUserImageCommand { get; private set; }

        private IFibCRUDClients _fibCRUDClients;
        private IFibStorageService _fibStorage;
        private IFibStatusService _fibStatusService;
        IFibLevelsService _fibLevelsService;
        IFibCyclesService _fibCycles;
        #endregion

        public ClientDetailsViewModel(IFibCRUDClients fibCRUDClients,
            IFibStatusService fibStatusService,
            IFibLevelsService fibLevelsService,
            IFibCyclesService fibCycles,
            IFibStorageService fibStorageService)
        {
            this._fibCRUDClients = fibCRUDClients;
            this._fibStorage = fibStorageService;
            this._fibStatusService = fibStatusService;
            this._fibCycles = fibCycles;
            this._fibLevelsService = fibLevelsService;

            IsEdit = false;
            IsEditVisible = true;
            EditCommand = new Command(OnEditCommand);
            ContractCommand = new Command(OnContractCommand);
            AddStudentCommand = new Command(OnAddStudentCommand);
            PaymentsCommand = new Command(OnPaymentsCommand);
            UpdateUserImageCommand = new Command(OnUpdateUserImageCommand);
            ImageUrl = "user_icon.png";


            IsLoading = false;
        }

        public void SetClientOnVM(ClientModel client)
        {
            if (Client == null)
            {
                Client = new ClientModel();
            }
            Client = client;
        }

        void ResetFlags()
        {
            IsEditVisible = false;
            IsPayments = false;
            IsContract = false;
            IsAddStudent = false;
            IsEdit = false;

        }
        async void OnUpdateUserImageCommand()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;

            var picture = await MediaPicker.PickPhotoAsync();
            if (picture != null)
            {
                var stream = await picture.OpenReadAsync();
                try
                {
                    var img = await _fibStorage.AddImageFibStorge(Client.Id, "UserImage", stream);
                    if (!string.IsNullOrEmpty(img))
                    {
                        ImageUrl = img;
                        Client.UrlImage = img;
                        SetClientOnVM(Client);
                        await _fibCRUDClients.UpdateClient(Client);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                }
            }
            IsLoading = false;

        }

        private void OnPaymentsCommand()
        {
            ResetFlags();
            IsPayments = true;
        }

        private async void OnAddStudentCommand()
        {
            ResetFlags();
            IsLoading = true;
            IsAddStudent = true;
            IsLoadingRequierements = true;
            //this could be subsctract in a service
            var cycles = await _fibCycles.GetCycles();
            var levels = await _fibLevelsService.GetLevels();
            var status = await _fibStatusService.GetStatus();
            cycles.ToList();
            levels.ToList();
            status.ToList();

            if (cycles.Count == 0 || levels.Count == 0 || status.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "you are missing cycles , levels or status to subscribe a student to this user, status is also used for clients", "ok");
            }
            //this could be subsctract in a service
            else
            {

                var client = Client;
                IsEditVisible = true;
                IsLoading = false;
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "Client",client}
                };
                await AppShell.Current.GoToAsync(nameof(AddStudentPage), true, parameters);
               
            }

        }

        private void OnContractCommand()
        {
            ResetFlags();
            IsContract = true;
        }

        private void OnEditCommand()
        {
            IsLoading = true;
            if (IsEdit)
            {               
                _fibCRUDClients.UpdateClient(Client);
            }
            else
            {
                ResetFlags();
                App.Current.MainPage.DisplayAlert("Edit", "now u can edit user", "ok");
                IsEditVisible = true;
                IsEdit = true;
            }
            IsLoading = false;
        }
    }
}

