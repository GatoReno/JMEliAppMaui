using System;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

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

        private bool _isEdit, _isLoading;
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }

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
        
        #endregion

        public ClientDetailsViewModel(IFibCRUDClients fibCRUDClients, IFibStorageService fibStorageService)
        {
            this._fibCRUDClients = fibCRUDClients;
            this._fibStorage = fibStorageService;
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
                    var img = await _fibStorage.AddImageFibStorge(Client.Id,"UserImage",stream);
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

        private void OnAddStudentCommand()
        {
            ResetFlags();
            IsAddStudent = true;
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

