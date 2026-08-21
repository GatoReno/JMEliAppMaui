using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels
{
	public class CyclesViewModel : BindableObject
    {

        #region props
        private bool _isLoading, _isEdit, _isAdd, _isShow, _AddVisibility, _backVisibility;
        private string _CycleName, _Title,_Id;
        public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        public string CycleName { get => _CycleName; set { _CycleName = value; OnPropertyChanged(); } }


        public bool BackVisibility
        {
            get => _backVisibility; set
            {
                _backVisibility = value;
            }
        }
        public bool AddVisibility
        {
            get => _AddVisibility; set
            {
                _AddVisibility = value;
            }
        }
        
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsAdd { get => _isAdd; set { _isAdd = value; OnPropertyChanged(); } }
        public bool IsShow { get => _isShow; set { _isShow = value; OnPropertyChanged(); } }

        DateTime _StartDate, _EndDate;
        public DateTime StartDate { get => _StartDate; set { _StartDate = value; OnPropertyChanged(); } }
        public DateTime EndDate { get => _EndDate; set { _EndDate = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand EditCycleCommand { get; private set; }
        public ICommand DeleteCycleCommand { get; private set; }
        public ICommand TerminateCycleCommand { get; private set; }

        public ObservableCollection<CycleModel> Cycles { get; set; }
        public CycleModel SelectedCycle { get; set; }

        private IFibAddGenericService _fibAddGenericService;
        IFibCyclesService _fibCyclesService1;
        #endregion

        public CyclesViewModel(IFibAddGenericService fibAddGenericService, IFibCyclesService fibCyclesService)
		{
            this._fibAddGenericService = fibAddGenericService;
            this._fibCyclesService1 = fibCyclesService;
            Cycles = new ObservableCollection<CycleModel>();
            OnBackCommand();
            SelectedCycle = new CycleModel();
            AddCommand = new Command(OnAddCommand);
            BackCommand = new Command(OnBackCommand);
            EditCycleCommand = new Command<CycleModel>(OnEditCycleCommand);
            DeleteCycleCommand = new Command(OnDeleteCycleCommand);
            TerminateCycleCommand = new Command<CycleModel>(OnTerminateCycle);
            BackVisibility = true;
            Title = "Cycles";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        private async void OnDeleteCycleCommand(object obj)
        {
            IsLoading = true;
            var deletePromot = await App.Current.MainPage.DisplayAlert("Warning", $"Are you sure about delete {SelectedCycle.Name} ? ", "Ok","Cancel");

            if (deletePromot)
            {
                await _fibAddGenericService.DeleteChild(SelectedCycle.Id, "Cycles");
                OnBackCommand();
            }

            IsLoading = false;
        }

        private async void OnEditCycleCommand(CycleModel model)
        {
            IsLoading = true;
            AddVisibility = false;
            if (IsEdit)
            {
                SelectedCycle.Name = CycleName;
                SelectedCycle.EndDate = EndDate.ToString("yyyy-MM-dd");
                SelectedCycle.Id = Id;
                SelectedCycle.StartDate = StartDate.ToString("yyyy-MM-dd");
                await _fibAddGenericService.UpdateChild(SelectedCycle, "Cycles",SelectedCycle.Id);
                OnBackCommand();
            }
            else
            {
                CycleName = model.Name;
                SelectedCycle.Name = CycleName;
                SelectedCycle.EndDate = EndDate.ToString("yyyy-MM-dd");
                SelectedCycle.Id = model.Id;
                SelectedCycle.StartDate = StartDate.ToString("yyyy-MM-dd");


                IsAdd = false;
                IsEdit = true;
                IsShow = false;
            }
            IsLoading = false;


        }

        private void OnBackCommand()
        {
            IsAdd = false;
            IsEdit = false;
            IsShow = true;
            AddVisibility = true;
            GetChilds();
        }

        private async void OnAddCommand()
        {

            
            if (IsEdit)
            {
                return;
            }
            if (IsAdd)
            {
                if (EndDate == DateTime.Today || string.IsNullOrEmpty(CycleName))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "A name and end date not to be today to add a Cycle;", "ok");
                }
                else
                {
                    IsLoading = true;
                    var model = new CycleModel { Name = CycleName };

                    var id = await _fibAddGenericService.AddChild(model, "Cycles");

                    if (!string.IsNullOrEmpty(id.ToString()))
                    {
                        SelectedCycle.Name = CycleName;
                        SelectedCycle.StartDate = StartDate.ToString("yyyy-MM-dd");
                        SelectedCycle.EndDate = EndDate.ToString("yyyy-MM-dd");
                        SelectedCycle.Id = Id = id.ToString();
                        await _fibAddGenericService.UpdateChild(SelectedCycle, "Cycles", id.ToString());
                    }


                    IsShow = true;
                    IsAdd = false;
                    Cycles.Add(model);
                    IsLoading = false;

                    // Graduate all active enrollments from previous cycles
                    await EgressPreviousCycleStudents(id.ToString());

                    OnBackCommand();
                }
            }
            else
            {
                IsAdd = true;
                IsEdit = false;
                IsShow = false;
                CycleName = "";
            }
           
        }

        private async  void GetChilds()
        {


            IsLoading = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "need internet to procede, check your conectivity", "ok");

                return;
            }
            Cycles.Clear();
            var clients = await _fibCyclesService1.GetCycles();
            foreach (var item in clients)
            {
                Cycles.Add(item);
            }

            IsLoading = false;

        }

        /// <summary>
        /// Terminate a cycle: only allowed 3 days before EndDate.
        /// Marks all enrollments as Egresado and creates AcademicHistory.
        /// </summary>
        private async void OnTerminateCycle(CycleModel cycle)
        {
            if (cycle == null) return;

            // Check if we're within 3 days of EndDate
            if (DateTime.TryParse(cycle.EndDate, out var endDate))
            {
                var daysUntilEnd = (endDate - DateTime.Today).TotalDays;
                if (daysUntilEnd > 3)
                {
                    await Shell.Current.DisplayAlert("⏳ No disponible",
                        $"Solo puedes cerrar el ciclo a partir de 3 días antes de la fecha de cierre ({cycle.EndDate}). Faltan {(int)daysUntilEnd} días.",
                        "OK");
                    return;
                }
            }

            var confirm = await Shell.Current.DisplayAlert("⚠️ Terminar Ciclo",
                $"¿Estás seguro de terminar el ciclo '{cycle.Name}'?\n\nTodos los alumnos activos serán marcados como Egresados.",
                "Sí, terminar", "Cancelar");

            if (!confirm) return;

            IsLoading = true;
            try
            {
                var firebase = (IFirebaseService)_fibAddGenericService;

                // Graduate all enrollments in this cycle
                var enrollments = await firebase.GetWhereAsync<EnrollmentModel>(
                    "Enrollments", e => e.CycleId == cycle.Id &&
                        (e.Status == StudentStatus.Activo || e.Status == StudentStatus.Inscrito));

                foreach (var enrollment in enrollments)
                {
                    var history = new AcademicHistoryModel
                    {
                        StudentId = enrollment.StudentId,
                        StudentName = enrollment.StudentName,
                        CycleId = cycle.Id,
                        CycleName = cycle.Name,
                        Level = enrollment.Level,
                        Grade = enrollment.Grade,
                        FinalStatus = StudentStatus.Egresado,
                        CompletedDate = DateTime.Now.ToString("yyyy-MM-dd")
                    };
                    var hId = await firebase.AddAsync(history, "AcademicHistory");
                    history.Id = hId;
                    await firebase.UpdateAsync(history, "AcademicHistory", hId);

                    enrollment.Status = StudentStatus.Egresado;
                    enrollment.StatusChangeDate = DateTime.Now.ToString("yyyy-MM-dd");
                    await firebase.UpdateAsync(enrollment, "Enrollments", enrollment.Id!);
                }

                // Mark cycle as terminated
                cycle.Name = $"{cycle.Name} (Terminado)";
                await firebase.UpdateAsync(cycle, "Cycles", cycle.Id!);

                await Shell.Current.DisplayAlert("✅ Ciclo Terminado",
                    $"{enrollments.Count} alumno(s) egresados. El ciclo '{cycle.Name}' ha sido cerrado.", "OK");

                OnBackCommand();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally { IsLoading = false; }
        }

        /// <summary>
        /// When a new cycle is created, all active/inscrito enrollments from OTHER cycles get graduated.
        /// Creates AcademicHistory records and updates enrollment status to Egresado.
        /// </summary>
        private async Task EgressPreviousCycleStudents(string newCycleId)
        {
            try
            {
                var firebase = (IFirebaseService)_fibAddGenericService;
                var enrollments = await firebase.GetWhereAsync<EnrollmentModel>(
                    "Enrollments", e => e.CycleId != newCycleId &&
                        (e.Status == StudentStatus.Activo || e.Status == StudentStatus.Inscrito));

                foreach (var enrollment in enrollments)
                {
                    // Create academic history record
                    var history = new AcademicHistoryModel
                    {
                        StudentId = enrollment.StudentId,
                        StudentName = enrollment.StudentName,
                        CycleId = enrollment.CycleId,
                        CycleName = enrollment.CycleName,
                        Level = enrollment.Level,
                        Grade = enrollment.Grade,
                        FinalStatus = StudentStatus.Egresado,
                        CompletedDate = DateTime.Now.ToString("yyyy-MM-dd")
                    };
                    var histId = await firebase.AddAsync(history, "AcademicHistory");
                    history.Id = histId;
                    await firebase.UpdateAsync(history, "AcademicHistory", histId);

                    // Update enrollment status
                    enrollment.Status = StudentStatus.Egresado;
                    enrollment.StatusChangeDate = DateTime.Now.ToString("yyyy-MM-dd");
                    await firebase.UpdateAsync(enrollment, "Enrollments", enrollment.Id!);
                }

                if (enrollments.Count > 0)
                {
                    await Shell.Current.DisplayAlert("Ciclo Nuevo",
                        $"{enrollments.Count} alumno(s) del ciclo anterior marcados como Egresados.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Egress error: {ex.Message}");
            }
        }
    }
}

