using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Client), "Client")]
    public class AddStudentPageViewModel : BaseViewModel
	{
         private bool _cycleVisibility, _levelsVisibility, _gradesVisibility, _statusVisibility;

        public bool CycleVisibility
        { get => _cycleVisibility; set { _cycleVisibility = value; OnPropertyChanged(); } }
        public bool LevelsVisibility
        { get => _levelsVisibility; set { _levelsVisibility = value; OnPropertyChanged(); } }
        public bool GradesVisibility
        { get => _gradesVisibility; set { _gradesVisibility = value; OnPropertyChanged(); } }
        public bool StatusVisibility
        { get => _statusVisibility; set { _statusVisibility = value; OnPropertyChanged(); } }



        private ClientModel _clientUser;
        public ObservableCollection<CycleModel> Cycles { get; set; }
        public ObservableCollection<StudentLevelsModel> Levels { get; set; }
        public ObservableCollection<StatusModel> Status { get; set; }
        public ObservableCollection<StudentGradesModel> Grades { get; set; }

        public ClientModel Client
        { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }

        public StudentModel Student
        { get => _student; set { _student = value; OnPropertyChanged(); } }


        public ICommand SelectCycleCommand { get; set; }
        public ICommand SelectLevelCommand { get; set; }
        public ICommand SelectGradeCommand { get; set; }

        
        private IFibStorageService _fibStorage;
        
        private IFibStatusService _fibStatusService;
        IFibLevelsService _fibLevelsService;
        IFibCyclesService _fibCycles;
         private StudentModel _student;

        public AddStudentPageViewModel(IFibAddGenericService<object> fibAddGenericService ,
            IFibStatusService fibStatusService,
            IFibLevelsService fibLevelsService,
            IFibCyclesService fibCycles,
            IFibStorageService fibStorageService)
        {
            
            this._fibStorage = fibStorageService;
            this._fibStatusService = fibStatusService;
            this._fibCycles = fibCycles;
            this._fibLevelsService = fibLevelsService;  
            this._fibAddGenericService = fibAddGenericService;
          
            Cycles = new ObservableCollection<CycleModel>();
            Status = new ObservableCollection<StatusModel>();

            Grades = new ObservableCollection<StudentGradesModel>();
            Levels = new ObservableCollection<StudentLevelsModel>();
            AddCommand = new Command(OnAddCommand);
            Student = new StudentModel();
            SelectCycleCommand = new Command<CycleModel>(OnSelectCycleCommand);
            SelectGradeCommand = new Command<StudentGradesModel>(OnSelectGradeCommand);
            SelectLevelCommand = new Command<StudentLevelsModel>(OnSelectLevelCommand);
            DeleteCommand = new Command(OnDeleteCommand);
            AppearingCommand = new Command(OnAppearingCommand);
            OnAppearingCommand();
            ResetFlas();
        }

        private void OnSelectGradeCommand(StudentGradesModel model)
        {
            Student.Grade = model.Name;

            CycleVisibility = false;
            LevelsVisibility = false;
            GradesVisibility = false;
            StatusVisibility = true;
        }

        private async void OnSelectLevelCommand(StudentLevelsModel model)
        {
            Student.Level = model.Name;
            var grades = model.Grades;
            grades.ToList();
            if (grades.Count > 0)
            {
                
                foreach (var item in grades)
                {
                    Grades.Add(item);

                }
                CycleVisibility = false;
                LevelsVisibility = false;
                GradesVisibility = true;
                StatusVisibility = false;
            }
            else
            {
              await NavigateBack();
            }
        }

        private void ResetFlas()
        {
            IsAdd = true;
            CycleVisibility = true;
            LevelsVisibility = false;
            GradesVisibility = false;
            StatusVisibility = false;
        }

        private void OnSelectCycleCommand(CycleModel model)
        {
            Student.ActualCycle = model.Name;
            CycleVisibility = false;
            LevelsVisibility = true;
            GradesVisibility = false;
            StatusVisibility = false;
        }

        private async void OnAppearingCommand()
        {
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
                await NavigateBack();
            }
            else
            {
                Cycles.Clear();
                foreach (var item in cycles)
                {
                    Cycles.Add(item);
                    //CycleList.Add(item.Name);
                }
                //CycleList
                Status.Clear();
                foreach (var item in status)
                {
                    //StatusList.Add(item.Name);
                    Status.Add(item);
                }
                Levels.Clear();
                foreach (var item in levels)
                {
                    //LevelList.Add(item.Name);
                    Levels.Add(item);
                }


                IsAdd = true;
            }
            //this could be subsctract in a service
            IsLoadingRequierements = false;
        }

        private async Task NavigateBack()
        {
            await App.Current.MainPage.DisplayAlert("Alert", "you are missing cycles , levels or status to subscribe a student to this user, status is also used for clients, please make sure you have them to procede", "ok");

            await AppShell.Current.GoToAsync("/..");
        }

        private void OnDeleteCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void OnAddCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

