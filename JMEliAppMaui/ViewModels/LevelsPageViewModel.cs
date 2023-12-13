using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
 using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
 namespace JMEliAppMaui.ViewModels
{
    public class LevelsPageViewModel : BindableObject
    {
        private bool  _isLoading,_saveLevelGrades;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public bool SaveLevelGrades { get => _saveLevelGrades; set { _saveLevelGrades = value; OnPropertyChanged(); } }

        private string _gradeName, _levelName;
        private IFibAddGenericService<object> _fibAddGenericService;

        public ObservableCollection<StudentGradesModel> Grades {get;set;}
        public ObservableCollection<StudentLevelsModel> Levels { get; set; }
        public StudentLevelsModel SelectedLevel { get; set; }

        public string GradeNameEntry { get => _gradeName; set { _gradeName = value; OnPropertyChanged(); } }
        public string LevelName { get => _levelName; set { _levelName = value; OnPropertyChanged(); } }

        private bool _isEdit, _isShowing, _isAdding, _levelNameEnable, _ShowaddGrades;
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsShowing { get => _isShowing; set { _isShowing = value; OnPropertyChanged(); } }
        public bool IsAdding { get => _isAdding; set { _isAdding = value; OnPropertyChanged(); } }
        public bool LevelNameEnable { get => _levelNameEnable; set { _levelNameEnable = value; OnPropertyChanged(); } }
        public bool ShowAddGrades { get => _ShowaddGrades; set { _ShowaddGrades = value; OnPropertyChanged(); } }

        public ICommand DeleteGradeCommand { get; set; }
        public ICommand DeleteLevelCommand { get; set; }
        public ICommand AddLevelCommand { get; set; }
        public ICommand AddGradeCommand { get; set; }
        public ICommand SaveLevelGradesCommand { get; set; }
        public LevelsPageViewModel(IFibAddGenericService<object> fibAddGenericService)
		{
            this._fibAddGenericService = fibAddGenericService;
            Grades = new ObservableCollection<StudentGradesModel>();
            Levels = new ObservableCollection<StudentLevelsModel>();
            DeleteGradeCommand = new Command<StudentGradesModel>(OnDeleteGradeCommand);
            AddGradeCommand = new Command(OnAddGradeCommand);
            DeleteLevelCommand = new Command<StudentLevelsModel>(OnDeleteLevelCommand);
            AddLevelCommand = new Command(OnAddLevelCommand);
            SaveLevelGradesCommand = new Command(OnSaveLevelGradesCommand);
            SelectedLevel = new StudentLevelsModel();
            SaveLevelGrades = false;
            IsAdding = true;
            LevelNameEnable = true;
            GetChilds();
        }

        private async void GetChilds()
        {
            try
            {
               object fetchLevels = await _fibAddGenericService.GetChilds("Levels");

                var note = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StudentLevelsModel>>(fetchLevels.ToString());
                foreach (var item in note)
                {
                    Levels.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void OnSaveLevelGradesCommand(object obj)
        {
            SelectedLevel.Grades = new List<StudentGradesModel>();
            foreach (var item in Grades)
            {
                SelectedLevel?.Grades?.Add(item);
            } 

             if (!string.IsNullOrEmpty(SelectedLevel?.Id?.ToString()))
            {
            
                await _fibAddGenericService.UpdateChild(SelectedLevel, "Levels", SelectedLevel.Id.ToString());
            }

        }

        private void OnDeleteLevelCommand(StudentLevelsModel model)
        {

            Levels.Remove(model);
        }

        private async void OnAddLevelCommand()
        {
            IsLoading = true;

            if (!string.IsNullOrEmpty(LevelName))
            {
                var model = new StudentLevelsModel { Name = LevelName };

                var id = await _fibAddGenericService.AddChild(model, "Levels");

                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    SelectedLevel.Name = LevelName;
                    SelectedLevel.Id = id.ToString();
                    await _fibAddGenericService.UpdateChild(SelectedLevel, "Levels", id.ToString());
                }

                LevelNameEnable = false;
                ShowAddGrades = true;
                Levels.Add(model);
            }
            IsLoading = false;

        }

        private async void OnAddGradeCommand( )
        {
            IsLoading = true;
            if (!string.IsNullOrEmpty(GradeNameEntry))
            {
                var model = new StudentGradesModel { Name = GradeNameEntry };
                model.IdLevel = SelectedLevel.Id;


                SaveLevelGrades = true;
                Grades.Add(model);//GradeNameEntry
            }
            GradeNameEntry = string.Empty;
            IsLoading = false;

        }

        private void OnDeleteGradeCommand(StudentGradesModel obj)
        {
            Grades.Remove(obj);
        }
        async void x(object model)
        {

           
        }
    }
}

