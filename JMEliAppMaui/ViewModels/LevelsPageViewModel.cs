using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Firebase.Database;
 using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using Newtonsoft.Json;

namespace JMEliAppMaui.ViewModels
{
    public class LevelsPageViewModel : BindableObject
    {
        private bool  _isLoading,_saveLevelGrades, _backVisibility;

        public bool BackVisibility { get => _backVisibility; set { _backVisibility = value; OnPropertyChanged(); } }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public bool SaveLevelGrades { get => _saveLevelGrades; set { _saveLevelGrades = value; OnPropertyChanged(); } }

        private string _gradeName, _levelName;
        private IFibAddGenericService<object> _fibAddGenericService;
        private IFibLevelsService _fibLevelsService;
        public ObservableCollection<StudentGradesModel> Grades {get;set;}
        public ObservableCollection<StudentLevelsModel> Levels { get; set; }
        public ObservableCollection<StudentGradesModel> SelectedGrades { get; set; }


        
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
        public ICommand BackCommand { get; set; }
        public ICommand EditLevelCommand { get; set; }
 
        

        public LevelsPageViewModel(IFibAddGenericService<object> fibAddGenericService, IFibLevelsService fibLevelsService)
		{
            this._fibAddGenericService = fibAddGenericService;
            this._fibLevelsService = fibLevelsService;
            Grades = new ObservableCollection<StudentGradesModel>();
            Levels = new ObservableCollection<StudentLevelsModel>();
            SelectedGrades = new ObservableCollection<StudentGradesModel>();
            DeleteGradeCommand = new Command<StudentGradesModel>(OnDeleteGradeCommand);
            AddGradeCommand = new Command(OnAddGradeCommand);
            DeleteLevelCommand = new Command(OnDeleteLevelCommand);
            AddLevelCommand = new Command(OnAddLevelCommand);
            SaveLevelGradesCommand = new Command(OnSaveLevelGradesCommand);
            BackCommand = new Command(OnBackCommand);
            EditLevelCommand = new Command<StudentLevelsModel>(OnEditLevelCommand);
            SelectedLevel = new StudentLevelsModel();
            SaveLevelGrades = false;
            LevelNameEnable = true;
            BackVisibility = false;
            GetChilds();
        }

        private void OnEditLevelCommand(StudentLevelsModel obj)
        {
            IsEdit = true;
            IsShowing = false;
            IsAdding = false;
            BackVisibility = true;

            SelectedLevel = obj;
            Grades.Clear();
            LevelName = obj.Name;
            SelectedGrades.Clear();
            if (SelectedLevel.Grades.Any())
            {
                foreach (var item in SelectedLevel.Grades)
                {
                    SelectedGrades.Add(item);
                }
            }
         

        }

        private void OnBackCommand()
        {
            IsAdding = false;
            IsEdit = false;
            IsShowing = true;
            GetChilds();
            BackVisibility = false;
            ShowAddGrades = false;
        }

        private async void GetChilds()
        {
            try
            {
                Levels.Clear();
                Grades.Clear();
                SelectedGrades.Clear();
                var _levels = await _fibLevelsService.GetLevels();
                foreach (var item in _levels)
                {
                    Levels.Add(item);
                }
                if (Levels.Count > 0)
                {
                    IsShowing = true;
                    IsEdit = false;
                    IsAdding = false;
                }
                else
                {
                    IsAdding = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($" {ex.Message} {ex.Data}");

            }
        }

        private async void OnSaveLevelGradesCommand(object obj)
        {
            IsLoading = true;
            BackVisibility = true;
            SelectedLevel.Grades = new List<StudentGradesModel>();
            foreach (var item in Grades)
            {
                SelectedLevel?.Grades?.Add(item);
            } 

             if (!string.IsNullOrEmpty(SelectedLevel?.Id?.ToString()))
            {
            
                await _fibAddGenericService.UpdateChild(SelectedLevel, "Levels", SelectedLevel.Id.ToString());
            }
            SelectedLevel.Grades.Clear();
            Grades.Clear();
            OnBackCommand();
            IsLoading = false;

        }

        private async void OnDeleteLevelCommand()
        {
            IsLoading = true;

            var request = await App.Current.MainPage.DisplayAlert("","Are you sure you want to delete this Level","ok","cancel");
            if (request)
            {
                await _fibAddGenericService.DeleteChild(SelectedLevel.Id, "Levels");
            }
            OnBackCommand();
            IsLoading = false;

            // Levels.Remove(model);
        }

        private async void OnAddLevelCommand()
        {
            IsLoading = true;
            if (IsAdding)
            {
                BackVisibility = true;
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
                Grades.Clear();

            }
            else { IsAdding = true; IsShowing = false; BackVisibility = true; }
            IsLoading = false;

        }

        private  void OnAddGradeCommand()
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
         
    }
}

