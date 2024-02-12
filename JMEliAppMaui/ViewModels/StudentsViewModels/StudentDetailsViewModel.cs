using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Student), "Student")]
    public class StudentDetailsViewModel : StudentBaseViewModel
    {
        #region props
        private StudentModel _student;
        private ContractModel _selectContract;

        bool _MenuHolder, _paymentHolder,_editholder, _contractHolder, _contractDetailsHolder, _StatusHolder;
        private string _subMenuString;

        public bool MenuHolder
        { get => _MenuHolder; set { _MenuHolder = value; OnPropertyChanged(); } }
        public bool PaymentHolder
        { get => _paymentHolder; set { _paymentHolder = value; OnPropertyChanged(); } }
        public bool ContractHolder
        { get => _contractHolder; set { _contractHolder = value; OnPropertyChanged(); } }
        public bool StatusHolder
        { get => _StatusHolder; set { _StatusHolder = value; OnPropertyChanged(); } }
        public bool EditHolder
        { get => _editholder; set { _editholder = value; OnPropertyChanged(); } }
        public bool ContractDetailsHolder
        { get => _contractDetailsHolder; set { _contractDetailsHolder = value; OnPropertyChanged(); } }

        

        public StudentModel Student
        { get => _student; set { _student = value; OnPropertyChanged(); } }

        public ContractModel SelectedContracted
        { get => _selectContract; set { _selectContract = value; OnPropertyChanged(); } }

        public string MenuString
        { get => _subMenuString; set { _subMenuString = value; OnPropertyChanged(); } }
        public ICommand StatusCommand { get; private set; }
        public ICommand ContractCommand { get; private set; }
        public ICommand PaymentsCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand BackMenuCommand { get; private set; }
        public ICommand DetailsContractCommand { get; private set; }
        public ICommand OpenContractCommand { get; private set; }


        public ObservableCollection<ContractModel> StudentContractsL { get; set; }


        private IFibAddGenericService<object> _fibAddGenericService;
        IFibContract _fibContractService;

        //
        #endregion

        public StudentDetailsViewModel(IFibAddGenericService<object> fibAddGenericService, IFibContract fibContractService)
        {
            this._fibAddGenericService = fibAddGenericService;
            this._fibContractService = fibContractService;
            StatusCommand = new Command(OnStatusCommand);
            ContractCommand = new Command(OnContractCommand);
            PaymentsCommand = new Command(OnPaymentsCommand);
            BackMenuCommand = new Command(OnBackMenuCommand);
            AppearingCommand = new Command(OnOnAppearingCommand);
            EditCommand = new Command(OnEditCommand);
            DetailsContractCommand = new Command(OnDetailsContractCommand);
            OpenContractCommand = new Command(OnOpenContractCommand);
            StudentContractsL = new ObservableCollection<ContractModel>();
            //AppearingCommand.Execute(null);

        }

        private async void OnOpenContractCommand(object obj)
        {
            await Launcher.OpenAsync(SelectedContracted.Url);
        }

        private void OnDetailsContractCommand(object obj)
        {
            ContractDetailsHolder = true;
            MenuHolder = false;
            MenuString = "back";
            PaymentHolder = false;
            ContractHolder = false;            
            StatusHolder = false;
            EditHolder = false;

            SelectedContracted = (ContractModel)obj;

        }

        private void OnEditCommand(object obj)
        {
            MenuHolder = false;
            MenuString = "back";
            PaymentHolder = false;
            ContractHolder = false;
            StatusHolder = false;
            EditHolder = true;
            ContractDetailsHolder = false;
            // missing update Fib info implementation 
        }

        private void OnBackMenuCommand(object obj)
        {
            ResetFlags();
        }

        private void OnStatusCommand(object obj)
        {
            MenuHolder = false;
            MenuString = "back";
            PaymentHolder = false;
            ContractHolder = false;
            StatusHolder = true;
            EditHolder = false;
            ContractDetailsHolder = false;
        }

        private async void OnContractCommand(object obj)
        {
            MenuHolder = false;
            MenuString = "back";
            PaymentHolder = false;
            ContractHolder = true;
            StatusHolder = false;
            ContractDetailsHolder = false;
            EditHolder = false;
            StudentContractsL.Clear();

            var contracts = await _fibContractService.GetContractsStudent(Student.Id);
            if (contracts.Count() > 0)
            {
                foreach (var item in contracts)
                {
                    StudentContractsL.Add(item);
                }
            }

        }

        private void OnPaymentsCommand(object obj)
        {
            MenuHolder = false;
            MenuString = "back";
            PaymentHolder = true;
            ContractHolder = false;
            StatusHolder = false;
            EditHolder = false;
        }

        void ResetFlags(bool isOnAppearing = false)
        {
            MenuHolder = true;
            MenuString = "menu";
            PaymentHolder = false;
            ContractHolder = false;
            StatusHolder = false;
            EditHolder = false;
            ContractDetailsHolder = false;

        }
        private void OnOnAppearingCommand()
        {
            Fullname = Student.FullName;
            Alergies = Student.Alergies;
            BloodType = Student.BloodType;
            Clave = Student.Clave;
            LevelSelected = Student.Level;
            Grade = Student.Grade;
            Gender = Student.Gender;
            Observations = Student.Observations;
            Tuition = Student.Tuition;
            State = Student.State;
            Precedes = Student.Precedes;
            Status = Student.Status;
            Weight = Student.Weight;
            Size = Student.Size;
            Insurance = Student.Insurance;
            ActualCycle = Student.ActualCycle;
            ImageUrl = Student.UrlImage;
            ClientId = Student.ClientId;
            Id = Student.Id;
            ResetFlags(true);
        }
    }
}

