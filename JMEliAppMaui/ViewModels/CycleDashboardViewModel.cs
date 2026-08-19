using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using JMEliAppMaui.Views;

namespace JMEliAppMaui.ViewModels
{
    /// <summary>
    /// Dashboard principal centrado en el ciclo activo.
    /// Flujo guiado: Sin ciclo → Sin clientes → Sin alumnos → Dashboard completo.
    /// </summary>
    public class CycleDashboardViewModel : BindableObject
    {
        #region State enum

        public enum DashboardState
        {
            Loading,
            NoCycle,         // No hay ciclos → crear uno
            NoClients,       // Hay ciclo pero no clientes → agregar tutor
            NoStudents,      // Hay clientes pero no alumnos inscritos → inscribir
            Ready            // Todo listo → mostrar alumnos del ciclo
        }

        #endregion

        #region Properties

        private DashboardState _state = DashboardState.Loading;
        private string _activeCycleName = "";
        private string _cyclePeriod = "";
        private string _activeFilter = "Todos";
        private bool _isRefreshing;
        private int _totalStudents, _activeCount, _adeudoCount;

        public DashboardState State { get => _state; set { _state = value; OnPropertyChanged(); NotifyVisibility(); } }

        // Visibility helpers bound from XAML
        public bool IsLoading => State == DashboardState.Loading;
        public bool ShowNoCycle => State == DashboardState.NoCycle;
        public bool ShowNoClients => State == DashboardState.NoClients;
        public bool ShowNoStudents => State == DashboardState.NoStudents;
        public bool ShowReady => State == DashboardState.Ready;

        public string ActiveCycleName { get => _activeCycleName; set { _activeCycleName = value; OnPropertyChanged(); } }
        public string CyclePeriod { get => _cyclePeriod; set { _cyclePeriod = value; OnPropertyChanged(); } }
        public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(); } }
        public int TotalStudents { get => _totalStudents; set { _totalStudents = value; OnPropertyChanged(); } }
        public int ActiveCount { get => _activeCount; set { _activeCount = value; OnPropertyChanged(); } }
        public int AdeudoCount { get => _adeudoCount; set { _adeudoCount = value; OnPropertyChanged(); } }

        // Filter colors
        public Color AllFilterColor => _activeFilter == "Todos" ? Color.FromArgb("#4a148c") : Color.FromArgb("#9e9e9e");
        public Color InscritoFilterColor => _activeFilter == "Inscrito" ? Color.FromArgb("#1565c0") : Color.FromArgb("#9e9e9e");
        public Color ActivoFilterColor => _activeFilter == "Activo" ? Color.FromArgb("#2e7d32") : Color.FromArgb("#9e9e9e");
        public Color AdeudoFilterColor => _activeFilter == "Adeudo" ? Color.FromArgb("#f9a825") : Color.FromArgb("#9e9e9e");
        public Color BajaFilterColor => _activeFilter == "Baja" ? Color.FromArgb("#c62828") : Color.FromArgb("#9e9e9e");
        public Color SuspendidoFilterColor => _activeFilter == "Suspendido" ? Color.FromArgb("#e65100") : Color.FromArgb("#9e9e9e");
        public Color EgresadoFilterColor => _activeFilter == "Egresado" ? Color.FromArgb("#6a1b9a") : Color.FromArgb("#9e9e9e");

        public ObservableCollection<EnrollmentDisplayItem> AllEnrollments { get; set; } = new();
        public ObservableCollection<EnrollmentDisplayItem> FilteredEnrollments { get; set; } = new();

        #endregion

        #region Commands

        public ICommand FilterCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ViewStudentCommand { get; }
        public ICommand ChangeStatusCommand { get; }
        public ICommand AppearingCommand { get; }

        // Guided flow commands
        public ICommand CreateCycleCommand { get; }
        public ICommand AddClientCommand { get; }
        public ICommand AddStudentCommand { get; }
        public ICommand ManageCyclesCommand { get; }

        #endregion

        private readonly IFibCyclesService _cyclesService;
        private readonly IFirebaseService _firebase;
        private CycleModel? _activeCycle;

        public CycleDashboardViewModel(IFibCyclesService cyclesService, IFirebaseService firebase)
        {
            _cyclesService = cyclesService;
            _firebase = firebase;

            FilterCommand = new Command<string>(OnFilter);
            RefreshCommand = new Command(async () => await LoadDataAsync());
            ViewStudentCommand = new Command<EnrollmentDisplayItem>(OnViewStudent);
            ChangeStatusCommand = new Command<EnrollmentDisplayItem>(OnChangeStatus);
            AppearingCommand = new Command(async () => await LoadDataAsync());

            // Guided flow
            CreateCycleCommand = new Command(OnCreateCycle);
            AddClientCommand = new Command(OnAddClient);
            AddStudentCommand = new Command(OnAddStudent);
            ManageCyclesCommand = new Command(OnManageCycles);

            _ = LoadDataAsync();
        }

        #region Data Loading with Progressive State

        private async Task LoadDataAsync()
        {
            try
            {
                State = DashboardState.Loading;
                IsRefreshing = true;

                // Step 1: Check for cycles
                var cycles = await _cyclesService.GetCycles();
                var cycleList = cycles?.ToList() ?? new();

                if (cycleList.Count == 0)
                {
                    State = DashboardState.NoCycle;
                    return;
                }

                // We have a cycle - pick the latest
                _activeCycle = cycleList.LastOrDefault();
                ActiveCycleName = _activeCycle?.Name ?? "Sin nombre";
                CyclePeriod = $"{_activeCycle?.StartDate ?? "?"} — {_activeCycle?.EndDate ?? "?"}";

                // Step 2: Check for clients
                var clients = await _firebase.GetAllAsync<ClientModel>("Clients");
                if (clients.Count == 0)
                {
                    State = DashboardState.NoClients;
                    return;
                }

                // Step 3: Check for enrollments in this cycle
                var enrollments = await _firebase.GetWhereAsync<EnrollmentModel>(
                    "Enrollments", e => e.CycleId == _activeCycle!.Id);

                if (enrollments.Count == 0)
                {
                    State = DashboardState.NoStudents;
                    return;
                }

                // Step 4: We have everything - show the dashboard
                AllEnrollments.Clear();
                foreach (var enrollment in enrollments)
                {
                    AllEnrollments.Add(new EnrollmentDisplayItem
                    {
                        Id = enrollment.Id,
                        StudentId = enrollment.StudentId,
                        StudentName = enrollment.StudentName ?? "Sin nombre",
                        ClientName = enrollment.ClientName ?? "",
                        Level = enrollment.Level ?? "",
                        Grade = enrollment.Grade ?? "",
                        Status = enrollment.Status ?? StudentStatus.Inscrito,
                        StatusColor = Color.FromArgb(StudentStatus.GetColor(enrollment.Status ?? "")),
                        Tuition = enrollment.Tuition ?? ""
                    });
                }

                UpdateCounts();
                ApplyFilter();
                State = DashboardState.Ready;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CycleDashboard error: {ex.Message}");
                // If Firebase is unreachable, show NoCycle as fallback
                State = DashboardState.NoCycle;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        #endregion

        #region Guided Flow Navigation

        private async void OnCreateCycle()
        {
            await Shell.Current.GoToAsync($"//{nameof(CyclesPage)}");
        }

        private async void OnAddClient()
        {
            await Shell.Current.GoToAsync($"//{nameof(ClientsPage)}");
        }

        private async void OnAddStudent()
        {
            await Shell.Current.GoToAsync(nameof(SelectClientPage));
        }

        private async void OnManageCycles()
        {
            await Shell.Current.GoToAsync($"//{nameof(CyclesPage)}");
        }

        #endregion

        #region Filter & Actions

        private void UpdateCounts()
        {
            TotalStudents = AllEnrollments.Count;
            ActiveCount = AllEnrollments.Count(e => e.Status == StudentStatus.Activo);
            AdeudoCount = AllEnrollments.Count(e => e.Status == StudentStatus.Adeudo);
        }

        private void OnFilter(string filter)
        {
            _activeFilter = filter;
            ApplyFilter();
            OnPropertyChanged(nameof(AllFilterColor));
            OnPropertyChanged(nameof(InscritoFilterColor));
            OnPropertyChanged(nameof(ActivoFilterColor));
            OnPropertyChanged(nameof(AdeudoFilterColor));
            OnPropertyChanged(nameof(BajaFilterColor));
            OnPropertyChanged(nameof(SuspendidoFilterColor));
            OnPropertyChanged(nameof(EgresadoFilterColor));
        }

        private void ApplyFilter()
        {
            FilteredEnrollments.Clear();
            var source = _activeFilter == "Todos"
                ? AllEnrollments
                : new ObservableCollection<EnrollmentDisplayItem>(
                    AllEnrollments.Where(e => e.Status == _activeFilter));

            foreach (var item in source)
            {
                FilteredEnrollments.Add(item);
            }
        }

        private async void OnViewStudent(EnrollmentDisplayItem item)
        {
            if (item?.StudentId == null) return;

            try
            {
                // Load full student from Firebase
                var student = await _firebase.GetByIdAsync<StudentModel>("Students", item.StudentId);
                if (student == null)
                {
                    student = new StudentModel { Id = item.StudentId, FullName = item.StudentName };
                }
                student.Id = item.StudentId;

                await Shell.Current.GoToAsync(nameof(StudentDetailsPage), true,
                    new Dictionary<string, object>
                    {
                        { "Student", student }
                    });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ViewStudent error: {ex.Message}");
                // Fallback with partial data
                await Shell.Current.GoToAsync(nameof(StudentDetailsPage), true,
                    new Dictionary<string, object>
                    {
                        { "Student", new StudentModel { Id = item.StudentId, FullName = item.StudentName } }
                    });
            }
        }

        private async void OnChangeStatus(EnrollmentDisplayItem item)
        {
            if (item == null) return;

            var statusOptions = StudentStatus.All.Select(s => s.Name).ToArray();
            var result = await Shell.Current.DisplayActionSheet(
                $"Cambiar estado de {item.StudentName}",
                "Cancelar", null, statusOptions);

            if (!string.IsNullOrEmpty(result) && result != "Cancelar")
            {
                item.Status = result;
                item.StatusColor = Color.FromArgb(StudentStatus.GetColor(result));

                // Persist to Firebase
                var enrollment = new EnrollmentModel
                {
                    Id = item.Id,
                    StudentId = item.StudentId,
                    CycleId = _activeCycle?.Id,
                    Status = result,
                    StatusChangeDate = DateTime.Now.ToString("yyyy-MM-dd")
                };
                await _firebase.UpdateAsync(enrollment, "Enrollments", item.Id ?? "");

                UpdateCounts();
                ApplyFilter();
            }
        }

        #endregion

        private void NotifyVisibility()
        {
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(ShowNoCycle));
            OnPropertyChanged(nameof(ShowNoClients));
            OnPropertyChanged(nameof(ShowNoStudents));
            OnPropertyChanged(nameof(ShowReady));
        }
    }

    /// <summary>
    /// Display item for enrollment list.
    /// </summary>
    public class EnrollmentDisplayItem : BindableObject
    {
        private string? _status;
        private Color _statusColor = Colors.Grey;

        public string? Id { get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? ClientName { get; set; }
        public string? Level { get; set; }
        public string? Grade { get; set; }
        public string? Tuition { get; set; }

        public string? Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public Color StatusColor
        {
            get => _statusColor;
            set { _statusColor = value; OnPropertyChanged(); }
        }
    }
}
