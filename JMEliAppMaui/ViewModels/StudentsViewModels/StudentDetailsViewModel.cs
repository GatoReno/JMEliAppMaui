using System;
using System.Windows.Input;
using JMEliAppMaui.Models;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Student), "Student")]
    public class StudentDetailsViewModel : StudentBaseViewModel
    {
        #region props
        private StudentModel _student;
        bool _MenuHolder, _paymentHolder,_editholder, _contractHolder, _StatusHolder;
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

        public StudentModel Student
        { get => _student; set { _student = value; OnPropertyChanged(); } }

        public string MenuString
        { get => _subMenuString; set { _subMenuString = value; OnPropertyChanged(); } }
        public ICommand StatusCommand { get; private set; }
        public ICommand ContractCommand { get; private set; }
        public ICommand PaymentsCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand BackMenuCommand { get; private set; }
        //
        #endregion

        public StudentDetailsViewModel()
        {
            StatusCommand = new Command(OnStatusCommand);
            ContractCommand = new Command(OnContractCommand);
            PaymentsCommand = new Command(OnPaymentsCommand);
            BackMenuCommand = new Command(OnBackMenuCommand);
            AppearingCommand = new Command(OnOnAppearingCommand);
            EditCommand = new Command(OnEditCommand);
            //AppearingCommand.Execute(null);

        }

        private void OnEditCommand(object obj)
        {
            MenuHolder = false;
            MenuString = "back";
            PaymentHolder = false;
            ContractHolder = false;
            StatusHolder = false;
            EditHolder = true;
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
        }

        private void OnContractCommand(object obj)
        {
            MenuHolder = false;
            MenuString = "back";
            PaymentHolder = false;
            ContractHolder = true;
            StatusHolder = false;
            EditHolder = false;
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

