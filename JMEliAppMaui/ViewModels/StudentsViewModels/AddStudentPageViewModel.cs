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
        private bool _cycleVisibility, _levelsVisibility, _gradesVisibility, _statusVisibility, _studentSummaryVisibility, _BackSubsVisibility,_imagevisibility;
        private string _fullname, _gradeSelected , _ImageUrl, _levelSelected , _statusSelected, _cyclce;

        #region objects and obseravables
        public bool CycleVisibility
        { get => _cycleVisibility; set { _cycleVisibility = value; OnPropertyChanged(); } }
        public bool LevelsVisibility
        { get => _levelsVisibility; set { _levelsVisibility = value; OnPropertyChanged(); } }
        public bool GradesVisibility
        { get => _gradesVisibility; set { _gradesVisibility = value; OnPropertyChanged(); } }
        public bool StatusVisibility
        { get => _statusVisibility; set { _statusVisibility = value; OnPropertyChanged(); } }
        public bool StudentSummaryVisibility
        { get => _studentSummaryVisibility; set { _studentSummaryVisibility = value; OnPropertyChanged(); } }
         public bool Imagevisibility
        { get => _imagevisibility; set { _imagevisibility = value; OnPropertyChanged(); } }
        public bool BackSubsVisibility
        { get => _BackSubsVisibility; set { _BackSubsVisibility = value; OnPropertyChanged(); } }

         public string ImageUrl
        { get => _ImageUrl; set { _ImageUrl = value; OnPropertyChanged(); } }
        public string FullName
        { get => _fullname; set { _fullname = value; OnPropertyChanged(); } }
        public string GradeSelected
        { get => _gradeSelected; set { _gradeSelected = value; OnPropertyChanged(); } }
        public string LevelSelected
        { get => _levelSelected; set { _levelSelected = value; OnPropertyChanged(); } }
        public string StatusSelected
        { get => _statusSelected; set { _statusSelected = value; OnPropertyChanged(); } }
        public string CyclceSelected
        { get => _cyclce; set { _cyclce = value; OnPropertyChanged(); } }
        private StudentModel _student;
        private ClientModel _clientUser;
        public ObservableCollection<CycleModel> Cycles { get; set; }
        public ObservableCollection<StudentLevelsModel> Levels { get; set; }
        public ObservableCollection<StatusModel> Status { get; set; }
        public ObservableCollection<StudentGradesModel> Grades { get; set; }

        public ClientModel Client
        { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }

        public StudentModel Student
        { get => _student; set { _student = value; OnPropertyChanged(); } }
        #endregion

        #region commands and implementeations
        public ICommand SelectCycleCommand { get; set; }
        public ICommand SelectLevelCommand { get; set; }
        public ICommand SelectStatusCommand { get; set; }
        public ICommand SelectGradesCommand { get; set; }
        public ICommand BackSubsCommnad { get; set; }
        public ICommand UploadStudentImageCommand { get;set;}
        public ICommand ConfirmCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        
        IFibStorageService _fibStorage;        
        IFibStatusService _fibStatusService;
        IFibLevelsService _fibLevelsService;
        IFibCyclesService _fibCycles;

        #endregion commands and implementeations
      

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
            SelectStatusCommand = new Command<StatusModel>(OnSelectStatusCommand);
            BackSubsCommnad = new Command(OnBackSubsCommnad);
            SelectCycleCommand = new Command<CycleModel>(OnSelectCycleCommand);
            SelectGradesCommand = new Command<StudentGradesModel>(OnSelectGradeCommand);
            SelectLevelCommand = new Command<StudentLevelsModel>(OnSelectLevelCommand);
             DeleteCommand = new Command(OnDeleteCommand);
            UploadStudentImageCommand = new Command(OnUploadStudentImageCommand);
            ConfirmCommand = new Command(OnConfirmCommand);
            ResetCommand = new Command(OnResetCommand); 
            AppearingCommand = new Command(OnAppearingCommand);
            StudentSummaryVisibility = false;
            BackSubsVisibility = false;
            ImageUrl = "user_icon.png";
            Imagevisibility = false;
            OnAppearingCommand();
            
        }

        private async void OnResetCommand()
        {
            IsLoadingRequierements = true;
            if (Student.Id != null)
            {
                await _fibAddGenericService.DeleteChild(Student.Id,"Students");
            }
            
            ResetFlas();
            IsLoadingRequierements = false;
        }

        private void OnConfirmCommand(object obj)
        {
            throw new NotImplementedException();
        }

        public async void OnAppearingCommand()
        {
            IsAdd = false;
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
                }
                Status.Clear();
                foreach (var item in status)
                {
                    Status.Add(item);
                }
                Levels.Clear();
                foreach (var item in levels)
                {
                    Levels.Add(item);
                }

                CycleVisibility = true;
                IsAdd = true;
            }
            //this could be subsctract in a service
            IsLoadingRequierements = false;
        }

        #region private methods

        private async void OnUploadStudentImageCommand()
        {
            
            IsLoadingRequierements = true;
            IsAdd = false;
            var picture = await MediaPicker.PickPhotoAsync();
            if (picture != null)
            {
                var stream = await picture.OpenReadAsync();
                try
                {
                    var img = await _fibStorage.AddImageFibStorge(Student.Id, "StudentImage", stream);
                    if (!string.IsNullOrEmpty(img))
                    {
                        ImageUrl = img;
                        Student.UrlImage = img;
                        await _fibAddGenericService.UpdateChild(Student, "Students", Student.Id.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                    await App.Current.MainPage.DisplayAlert("Error", $"{ex.Message}. \n Please try Later.", "ok");

                }
            }
            IsAdd = true;
            IsLoadingRequierements = false;
        }

        private async void OnSelectStatusCommand(StatusModel model)
        {
            if (string.IsNullOrEmpty(FullName))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "You are missing student's full name", "ok");
                return;
            }
            else
            {

                Student.Status = model.Name;
                StatusSelected = model.Name;
                CycleVisibility = false;
                LevelsVisibility = false;
                GradesVisibility = false;
                StatusVisibility = false;
                Student.FullName = FullName;
                Student.Grade = GradeSelected;
                Student.Status = StatusSelected;
                Student.Level = LevelSelected;
                Student.ActualCycle = CyclceSelected;
                Student.ClientId = Client.Id;
                BackSubsVisibility = false;
                IsLoadingRequierements = true;
                IsAdd = false;
                var id = await _fibAddGenericService.AddChild(Student, "Students");

                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    Student.Id = id.ToString();
                    await _fibAddGenericService.UpdateChild(Student, "Students", id.ToString());
                    
                }
                
                StudentSummaryVisibility = true;
                Imagevisibility = true;
                IsAdd = true;
                IsLoadingRequierements = false;

            }
        }

       
        private void OnBackSubsCommnad()
        {
            
        }

        private void ResetFlas()
        {
            IsAdd = true;
            FullName = "";
            Student = new StudentModel();
            CycleVisibility = false;
            LevelsVisibility = false;
            GradesVisibility = false;
            StatusVisibility = false;
        }

        private async void OnSelectGradeCommand(StudentGradesModel model)
        {
            Student.Grade = model.Name;
            GradeSelected = model.Name;
            if (!string.IsNullOrEmpty(Student.Grade))
            {
                CycleVisibility = false;
                LevelsVisibility = false;
                GradesVisibility = false;
                StatusVisibility = true;
            }
            else
            {
                await NavigateBack();
            }
           
        }

        private async void OnSelectLevelCommand(StudentLevelsModel model)
        {
            Student.Level = model.Name;
            LevelSelected = model.Name;
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

      
        private async void OnSelectCycleCommand(CycleModel model)
        {
            Student.ActualCycle = model.Name;
            CyclceSelected = model.Name;

            if (!string.IsNullOrEmpty(Student.ActualCycle))
            {
                BackSubsVisibility = true;
                CycleVisibility = false;
                LevelsVisibility = true;
                GradesVisibility = false;
                StatusVisibility = false;
            }
            else
            {
                await NavigateBack();
            }
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

        #endregion private methods
    }
}

