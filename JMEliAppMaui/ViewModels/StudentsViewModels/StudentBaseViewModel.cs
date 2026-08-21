using System;
using System.Windows.Input;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    public class StudentBaseViewModel : BaseViewModel
    {
        #region privs

        private string _state, _gender, _observations, _Description, _insurance, _clave, _medicalHistory, _weight, _Tuition,
         _size, _bloodtype, _allergies,
         _phobias, _nickName, _trainedBath, _DevelopmentObservations, _SpecialWords,
         _BathHour, _SleepHour, _HomeLanguage, _AwakeHour, _NapHour, _BreakfastHour, _MealHour, _MealType, _MuscularControl;

        private string _fullname, _gradeSelected, _ImageUrl, _levelSelected, _status, _cyclce;
        private string _ActualCycle;
        private string _ClientId, _Id;
        private string? _studentCURP, _studentAddress, _studentPhone, _studentReligion, _studentPreviousSchool;
        private bool _Imagevisibility, _IsLateDevelopment;
        #endregion

        #region student

        public string? HomeLanguage
        { get => _HomeLanguage; set { _HomeLanguage = value; OnPropertyChanged(); } }
        public string? BathHour
        { get => _BathHour; set { _BathHour = value; OnPropertyChanged(); } }
        public string? SleepHour
        { get => _SleepHour; set { _SleepHour = value; OnPropertyChanged(); } }
        public string? AwakeHour
        { get => _AwakeHour; set { _AwakeHour = value; OnPropertyChanged(); } }
        public string? NapHour
        { get => _NapHour; set { _NapHour = value; OnPropertyChanged(); } }
        public string? BreakfastHour
        { get => _BreakfastHour; set { _BreakfastHour = value; OnPropertyChanged(); } }
        public string? MealHour
        { get => _MealHour; set { _MealHour = value; OnPropertyChanged(); } }
        public string? MealType
        { get => _MealType; set { _MealType = value; OnPropertyChanged(); } }
        public string? MuscularControl
        { get => _MuscularControl; set { _MuscularControl = value; OnPropertyChanged(); } }
        public string? Phobias
        { get => _phobias; set { _phobias = value; OnPropertyChanged(); } }
        public string? NickName
        { get => _nickName; set { _nickName = value; OnPropertyChanged(); } }
        public string? TrainedBath
        { get => _trainedBath; set { _trainedBath = value; OnPropertyChanged(); } }
        public string? DevelopmentObservations
        { get => _DevelopmentObservations; set { _DevelopmentObservations = value; OnPropertyChanged(); } }
        public string? SpecialWords
        { get => _SpecialWords; set { _SpecialWords = value; OnPropertyChanged(); } }

        public string? Tuition
        { get => _Tuition; set { _Tuition = value; OnPropertyChanged(); } }
        public string? ClientId
        { get => _ClientId; set { _ClientId = value; OnPropertyChanged(); } }
        public new string? Id
        { get => _Id; set { _Id = value; OnPropertyChanged(); } }
        public string? LevelSelected
        { get => _levelSelected; set { _levelSelected = value; OnPropertyChanged(); } }
        public string? Cycle
        { get => _cyclce; set { _cyclce = value; OnPropertyChanged(); } }
        public string? Fullname
        { get => _fullname; set { _fullname = value; OnPropertyChanged(); } }
        public string? Status
        { get => _status; set { _status = value; OnPropertyChanged(); } }
        public string? Allergies
        { get => _allergies; set { _allergies = value; OnPropertyChanged(); } }
        public string? Grade
        { get => _gradeSelected; set { _gradeSelected = value; OnPropertyChanged(); } }
        public string? Description
        { get => _Description; set { _Description = value; OnPropertyChanged(); } }
        public string? State
        { get => _state; set { _state = value; OnPropertyChanged(); } }
        public string? Gender
        { get => _gender; set { _gender = value; OnPropertyChanged(); } }
        public string? Observations
        { get => _observations; set { _observations = value; OnPropertyChanged(); } }
        public string? Insurance
        { get => _insurance; set { _insurance = value; OnPropertyChanged(); } }
        public string? Clave
        { get => _clave; set { _clave = value; OnPropertyChanged(); } }
        public string? MedicalHistory
        { get => _medicalHistory; set { _medicalHistory = value; OnPropertyChanged(); } }
        public string? Weight
        { get => _weight; set { _weight = value; OnPropertyChanged(); } }
        public string? Size
        { get => _size; set { _size = value; OnPropertyChanged(); } }
        public string? BloodType
        { get => _bloodtype; set { _bloodtype = value; OnPropertyChanged(); } }
        public string? ImageUrl
        { get => _ImageUrl; set { _ImageUrl = value; OnPropertyChanged(); } }
        public string? ActualCycle
        { get => _ActualCycle; set { _ActualCycle = value; OnPropertyChanged(); } }
        public bool Imagevisibility
        { get => _Imagevisibility; set { _Imagevisibility = value; OnPropertyChanged(); } }
        public bool IsLateDevelopment
        { get => _IsLateDevelopment; set { _IsLateDevelopment = value; OnPropertyChanged(); } }

        // Contract-required fields
        public string? StudentCURP
        { get => _studentCURP; set { _studentCURP = value; OnPropertyChanged(); } }
        public string? StudentAddress
        { get => _studentAddress; set { _studentAddress = value; OnPropertyChanged(); } }
        public string? StudentPhone
        { get => _studentPhone; set { _studentPhone = value; OnPropertyChanged(); } }
        public string? StudentReligion
        { get => _studentReligion; set { _studentReligion = value; OnPropertyChanged(); } }
        public string? StudentPreviousSchool
        { get => _studentPreviousSchool; set { _studentPreviousSchool = value; OnPropertyChanged(); } }

        #endregion
    }
}

