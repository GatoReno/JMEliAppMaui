using System;
using System.Windows.Input;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
	public class StudentBaseViewModel : BaseViewModel
	{

        #region privs

        private string _state, _gender, _observations, _Description, _insurance, _clave, _precedes, _weight, _Tuition, _size, _bloodtype, _alergies;

        private string _fullname, _gradeSelected, _ImageUrl, _levelSelected, _status, _cyclce;
        private string _ActualCycle;
        private string _ClientId,_Id;

         #endregion

        #region student


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


        #endregion


       
    }
}

