#if IOS

#endif

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Controls.UserDialogs.Maui;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using JMEliAppMaui.Views;
using Microsoft.Maui.Animations;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Student), "Student")]
    public class StudentDetailsViewModel : StudentBaseViewModel
    {
        #region props
 
        private StudentModel _student;
        private ContractModel _selectContract;

        bool _IsLateDevelopment,_MenuHolder, _paymentHolder,_editholder, _contractHolder, _contractDetailsHolder, _denyVisibility,_StatusHolder;
        private string _subMenuString;

        public bool IsLateDevelopment
        { 
            get => _IsLateDevelopment; 
            set 
            { 
                _IsLateDevelopment = value; 
                OnPropertyChanged(); 
            } 
        }
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

        public ICommand EditDataCommand { get; private set; }
        public ObservableCollection<ContractModel> StudentContractsL { get; set; }


        private readonly IAlertService _alertService;
        private readonly IFileService _fileService;
        private IFibAddGenericService _fibAddGenericService;
        IFibContract _fibContractService;
        private readonly IGetAsyncFileService _asyncGetFileService;
        private string _StatusTypeString;
        private string _DocumentMessage;

        //
        #endregion

        public StudentDetailsViewModel(IFibAddGenericService fibAddGenericService, 
                                       IFibContract fibContractService,
                                       IAlertService alertService,
                                       IFileService fileService,
                                       IGetAsyncFileService asyncGetFileService
                                       )
        {
            this._asyncGetFileService =  asyncGetFileService;
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
            EditDataCommand = new Command(OnEditDataCommand);
            Imagevisibility = true;
            IsLoadingRequierements = false;
            _alertService = alertService;
            _fileService = fileService;  
        }

        void OnEditDataCommand()
        {

            IsAdd = !IsAdd;
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
                OnEditDataCommand();
                return;
            }
            else
            {
                SetStudentValues();
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
            // If called from the list tap, obj is the contract
            var contract = obj as ContractModel ?? SelectedContracted;
            if (contract == null) return;

            await Shell.Current.GoToAsync(nameof(ContractViewerPage), true,
                new Dictionary<string, object>
                {
                    { nameof(ContractModel), contract }
                });
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

            try
            {
                var contracts = await ((IFirebaseService)_fibAddGenericService).GetWhereAsync<ContractModel>(
                    "Contracts", c => c.StudentId == Student.Id);
                foreach (var contract in contracts)
                {
                    StudentContractsL.Add(contract);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Load contracts error: {ex.Message}");
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
            GetStudentValues();  
            ResetFlags(true);
        }

 void SetStudentValues()
        {
            Student.FullName = Fullname;
            Student.Allergies = Allergies;
            Student.BloodType = BloodType;
            Student.Clave = Clave;
            Student.Level = LevelSelected;
            Student.Grade = Grade;
            Student.Gender = Gender;
            Student.Observations = Observations;
            Student.Tuition = Tuition;
            Student.State = State;
            Student.MedicalHistory = MedicalHistory;
            Student.Status = Status;
            Student.Weight = Weight;
            Student.Size = Size;
            Student.Insurance = Insurance;
            Student.ActualCycle = ActualCycle;
            Student.UrlImage = ImageUrl;
            Student.ClientId = ClientId;
            Student.Id = Id;
            Student.NapHour = NapHour;
            Student.BreakfastHour = BreakfastHour;
            Student.MealHour = MealHour;
            Student.MealType = MealType;
            Student.MuscularControl = MuscularControl;
            Student.Phobias = Phobias;
            Student.NickName = NickName;
            Student.TrainedBath = TrainedBath;
            Student.HomeLanguage = HomeLanguage;
            Student.IsLateDevelopment = IsLateDevelopment;
            Student.SpecialWords = SpecialWords;
            Student.BathHour = BathHour;
            Student.SleepHour = SleepHour;
            Student.AwakeHour = AwakeHour;
            if (IsLateDevelopment)
            {
                Student.DevelopmentObservations = DevelopmentObservations;
            }
            else
            {
                DevelopmentObservations = string.Empty;
            }
        }

        void GetStudentValues()
        {
            Fullname = Student.FullName;
            Allergies = Student.Allergies;
            BloodType = Student.BloodType;
            Clave = Student.Clave;
            LevelSelected = Student.Level;
            Grade = Student.Grade;
            Gender = Student.Gender;
            Observations = Student.Observations;
            Tuition = Student.Tuition;
            State = Student.State;
            MedicalHistory = Student.MedicalHistory;
            Status = Student.Status;
            Weight = Student.Weight;
            Size = Student.Size;
            Insurance = Student.Insurance;
            ActualCycle = Student.ActualCycle;
            ImageUrl = Student.UrlImage;
            ClientId = Student.ClientId;
            Id = Student.Id;
            NapHour = Student.NapHour;
            BreakfastHour = Student.BreakfastHour;
            MealHour = Student.MealHour;
            MealType = Student.MealType;
            MuscularControl = Student.MuscularControl;
            Phobias = Student.Phobias;
            NickName = Student.NickName;
            TrainedBath = Student.TrainedBath;
            HomeLanguage = Student.HomeLanguage;
            IsLateDevelopment = Student.IsLateDevelopment;
            SpecialWords = Student.SpecialWords;
            BathHour = Student.BathHour;
            SleepHour = Student.SleepHour;
            AwakeHour = Student.AwakeHour;
            if (IsLateDevelopment)
            {
                DevelopmentObservations = Student.DevelopmentObservations;
            }
            else
            {
                DevelopmentObservations = string.Empty;
            }
        }
   
   
    }
}

