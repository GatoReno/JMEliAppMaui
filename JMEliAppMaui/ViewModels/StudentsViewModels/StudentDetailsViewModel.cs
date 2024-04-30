using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Controls.UserDialogs.Maui;
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

        bool _MenuHolder, _paymentHolder,_editholder, _contractHolder, _contractDetailsHolder, _denyVisibility,_StatusHolder;
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
        public bool DenyVisibility
        { get => _denyVisibility; set { _denyVisibility = value; OnPropertyChanged(); } }
        

        public StudentModel Student
        { get => _student; set { _student = value; OnPropertyChanged(); } }

        public ContractModel SelectedContracted
        { get => _selectContract; set { _selectContract = value; OnPropertyChanged(); } }

        public string MenuString
        { get => _subMenuString; set { _subMenuString = value; OnPropertyChanged(); } }

        public string StatusTypeString
        { get => _StatusTypeString; set { _StatusTypeString = value; OnPropertyChanged(); } }

        public string DocumentMessage
        { get => _DocumentMessage; set { _DocumentMessage = value; OnPropertyChanged(); } }

        public ICommand StatusCommand { get; private set; }
        public ICommand ContractCommand { get; private set; }
        public ICommand PaymentsCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand BackMenuCommand { get; private set; }
        public ICommand DetailsContractCommand { get; private set; }
        public ICommand OpenContractCommand { get; private set; }
        public ICommand UpdateStudentDataCommand { get; private set; }
        public ICommand DenyDocumentCommand { get; private set; }


        public ObservableCollection<ContractModel> StudentContractsL { get; set; }


        private IFibAddGenericService _fibAddGenericService;
        IFibContract _fibContractService;
        private string _StatusTypeString;
        private string _DocumentMessage;

        //
        #endregion

        public StudentDetailsViewModel(IFibAddGenericService fibAddGenericService, IFibContract fibContractService)
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
            UpdateStudentDataCommand = new Command(OnUpdateStudentDataCommand);
            DenyDocumentCommand = new Command(OnDenyDocumentCommand);
            Imagevisibility = true;
            IsLoadingRequierements = false;
        }

        private async void OnDenyDocumentCommand(object obj)
        {
            await UserDialogs.Instance.AlertAsync("Please specify the rejection reason", "Info", "ok");
            ContractDetailsHolder = false;
            DenyVisibility = true;
        }

        private async  void OnUpdateStudentDataCommand()
        {
            if (!IsAdd)
            {
                IsAdd = true;
                return;
            }
            else
            {
                Student.State = State;
                Student.Precedes = Precedes;
                Student.Observations = Observations;
                Student.Insurance = Insurance;
                Student.Weight = Weight;
                Student.Gender = Gender;
                Student.Clave = Clave;
                Student.Size = Size;
                Student.BloodType = BloodType;
                Student.Alergies = Alergies;
                Student.FullName = Fullname;
                //
                IsLoadingRequierements = true;
                await Task.Delay(2000);
                await _fibAddGenericService.UpdateChild(Student, "Students", Student.Id.ToString());

                IsAdd = false;
                IsLoadingRequierements = false;
            }
          
        }

        private async void OnOpenContractCommand(object obj)
        {
             
            await  UserDialogs.Instance.AlertAsync("A  web browser will launch targeting your document, make sure store in download files in your device", "Info", "ok");
             
           
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
            StatusTypeString = $"Type: {SelectedContracted.Type} Status: {SelectedContracted.Status}";
            DocumentMessage = "";
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
            DenyVisibility = false;
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

