using System;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(Client), "ClientModel")]
    public class ClientDetailsViewModel : BindableObject
    {
        #region props
        private ClientModel _clientUser;
        public ClientModel Client { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }

        private bool _isEdit;
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }

        private bool _isEditVisible;
        public bool IsEditVisible { get => _isEditVisible; set { _isEditVisible = value; OnPropertyChanged(); } }

        private bool _isPayment;
        public bool IsPayments { get => _isPayment; set { _isPayment = value; OnPropertyChanged(); } }

        private bool _isContract;
        public bool IsContract { get => _isContract; set { _isContract = value; OnPropertyChanged(); } }

        private bool _isAddStudent;
        public bool IsAddStudent { get => _isAddStudent; set { _isAddStudent = value; OnPropertyChanged(); } }

        public ICommand EditCommand { get; private set; }
        public ICommand ContractCommand { get; private set; }
        public ICommand AddStudentCommand { get; private set; }
        public ICommand PaymentsCommand { get; private set; }

        private IFibCRUDClients _fibCRUDClients;

        #endregion

        public ClientDetailsViewModel(IFibCRUDClients fibCRUDClients)
        {
            this._fibCRUDClients = fibCRUDClients;
            IsEdit = false;
            IsEditVisible = true;
            Client = new ClientModel();
            EditCommand = new Command(OnEditCommand);
            ContractCommand = new Command(OnContractCommand);
            AddStudentCommand = new Command(OnAddStudentCommand);
            PaymentsCommand = new Command(OnPaymentsCommand);
        }

        

        void ResetFlags()
        {
            IsEditVisible = false;
            IsPayments = false;
            IsContract = false;
            IsAddStudent = false;
            IsEdit = false;

        }

        private void OnPaymentsCommand(object obj)
        {
            ResetFlags();
            IsPayments = true;
        }

        private void OnAddStudentCommand(object obj)
        {
            ResetFlags();
            IsAddStudent = true;
        }

        private void OnContractCommand(object obj)
        {
            ResetFlags();
            IsContract = true;
        }

        private void OnEditCommand(object obj)
        {
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
        }
    }
}

