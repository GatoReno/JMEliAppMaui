using System;
using System.Windows.Input;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
	public class StudentBaseViewModel : BaseViewModel
	{

        #region privs

        private string _state, _gender, _observations, _Description, _insurance, _clave, _precedes, _weight, _Tuition,
         _size, _bloodtype, _alergies,
         _phobias,_nickName,_trainedBath,_homeLanguge,_DevelopmentObservations,_SpecialWords,
         _BathHour,_SleepHour,_HomeLanguage,_AwakeHour,_NapHour,_BreakFastHour,_MealHour,_MealType,_MuscularControl;

        private string _fullname, _gradeSelected, _ImageUrl, _levelSelected, _status, _cyclce;
        private string _ActualCycle;
        private string _ClientId,_Id;
        private bool _Imagevisibility,_IsLateDevelopment;
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
        public string? BreakFastHour
        { get => _BreakFastHour; set { _BreakFastHour = value; OnPropertyChanged(); } }
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
         public string? HomeLanguge
        { get => _homeLanguge; set { _homeLanguge = value; OnPropertyChanged(); } }
         public string? DevelopmentObservations
        { get => _DevelopmentObservations; set { _DevelopmentObservations = value; OnPropertyChanged(); } }
         public string? SpecialWords
        { get => _SpecialWords; set { _SpecialWords = value; OnPropertyChanged(); } }

        //..

        public string? Tuition
        { get => _Tuition; set { _Tuition = value; OnPropertyChanged(); } }
        public string? ClientId
        { get => _ClientId; set { _ClientId = value; OnPropertyChanged(); } }
        public string? Id
        { get => _Id; set { _Id = value; OnPropertyChanged(); } }
        public string? LevelSelected
        { get => _levelSelected; set { _levelSelected = value; OnPropertyChanged(); } }
        public string? Cycle
        { get => _cyclce; set { _cyclce = value; OnPropertyChanged(); } }
        public string? Fullname
        { get => _fullname; set { _fullname = value; OnPropertyChanged(); } }
        public string? Status
        { get => _status; set { _status = value; OnPropertyChanged(); } }
        public string? Alergies
        { get => _alergies; set { _alergies = value; OnPropertyChanged(); } }
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
        public string? Precedes
        { get => _precedes; set { _precedes = value; OnPropertyChanged(); } }
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

        
        #endregion



    }
}

