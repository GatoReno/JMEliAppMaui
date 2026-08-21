using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Client), "Client")]
    public class AddStudentPageViewModel : StudentBaseViewModel
    {
        #region privaates
        private bool _cycleVisibility, _levelsVisibility, _gradesVisibility, _statusVisibility, _studentSummaryVisibility, _BackSubsVisibility, _imagevisibility, _IsContract;
        private string _fullname, _gradeSelected, _ImageUrl, _levelSelected, _statusSelected, _cyclce ;
        private string _Tuition;
        private string _inscriptionFee;
        #endregion

        #region objects and obseravables

        public bool IsContract
        { get => _IsContract; set { _IsContract = value; OnPropertyChanged(); } }
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
        public string Tuition { get => _Tuition; set { _Tuition = value; OnPropertyChanged(); } }
        public string InscriptionFee { get => _inscriptionFee; set { _inscriptionFee = value; OnPropertyChanged(); } }

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
        public ICommand UpdateStudentCommand { get; set; }
        public ICommand ContractPickerCommand { get; set; }


        IFibStorageService _fibStorage;        
        IFibStatusService _fibStatusService;
        IFibLevelsService _fibLevelsService;
        IFibCyclesService _fibCycles;
        private string? _selectedCycleId;
        private string? _currentEnrollmentId;
        #endregion commands and implementeations


        public AddStudentPageViewModel(IFibAddGenericService fibAddGenericService ,
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
            UpdateStudentCommand = new Command(OnUpdateStudentCommand);
            ContractPickerCommand = new Command(OnContractPickerCommand);

            StudentSummaryVisibility = false;
            BackSubsVisibility = false;
            ImageUrl = "user_icon.png";
            Imagevisibility = false;
            IsContract = false;
            OnAppearingCommand();
            
        }

       

        public async void OnAppearingCommand()
        {
            IsAdd = false;
            IsLoadingRequierements = true;

            try
            {
                var cycles = await _fibCycles.GetCycles();
                var levels = await _fibLevelsService.GetLevels();

                // Check prerequisites and guide user to fix what's missing
                var missing = new List<SchoolPrerequisite>();
                if (cycles.Count == 0) missing.Add(SchoolPrerequisite.Cycles);
                if (levels.Count == 0) missing.Add(SchoolPrerequisite.Levels);

                if (missing.Count > 0)
                {
                    var first = missing.First();
                    var info = PrerequisiteInfo.All[first];

                    var go = await Shell.Current.DisplayAlert(
                        $"{info.Emoji} Falta: {info.Title}",
                        info.Description,
                        info.ActionLabel,
                        "Cancelar");

                    if (go)
                    {
                        await Shell.Current.GoToAsync($"//{info.RouteName}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("..");
                    }

                    IsLoadingRequierements = false;
                    return;
                }

                // All prerequisites met — load data
                Cycles.Clear();
                foreach (var item in cycles)
                    Cycles.Add(item);

                // Status now comes from code constants, not Firebase
                Status.Clear();
                foreach (var statusInfo in StudentStatus.All)
                {
                    Status.Add(new StatusModel
                    {
                        Name = statusInfo.Name,
                        Description = statusInfo.Description,
                        Color = statusInfo.Color
                    });
                }

                Levels.Clear();
                foreach (var item in levels)
                    Levels.Add(item);

                CycleVisibility = true;
                IsAdd = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddStudent OnAppearing error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"No se pudieron cargar los datos: {ex.Message}", "OK");
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsLoadingRequierements = false;
            }
        }

        #region private methods

        private async void OnContractPickerCommand(object obj)
        {
            IsLoading = true;
            var contratFile = await FilePicker.PickAsync();
            if (contratFile != null)
            {
                var stream = await contratFile.OpenReadAsync();
                try
                {
                    var file = await _fibStorage.AddPdfFibStorge(Student.Id, "StudentContrat", stream);
                    if (!string.IsNullOrEmpty(file))
                    {
                        // ImageUrl = file;
                        ContractModel contract = new ContractModel { ClientId = Client.Id, StudentId = Student.Id, Type = "Inscripcion" , Url = file, Status = "New" }; 
                       var id =  await _fibAddGenericService.AddChild(contract,"Contract");
                        if (!string.IsNullOrEmpty(id.ToString()))
                        {
                            contract.Id = id.ToString();
                            await _fibAddGenericService.UpdateChild(contract, "Contract", id.ToString());
                            await App.Current.MainPage.DisplayAlert("Success", $"Contract updated, please confirm signed conftract.", "ok"); 
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error", $"Somethign got wrong , Please try Later.", "ok"); 
                        }

                     }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                    await App.Current.MainPage.DisplayAlert("Error", $"{ex.Message}. \n Please try Later.", "ok");

                }
            }
            IsLoading = false;
        }

        private async void OnUpdateStudentCommand(object obj)
        {
            Student.State = State;
            Student.MedicalHistory = MedicalHistory;
            Student.Observations = Observations;
            Student.Insurance = Insurance;
            Student.Weight = Weight;
            Student.Gender = Gender;
            Student.Clave = Clave;
            Student.Size = Size;
            Student.BloodType = BloodType;
            Student.Allergies = Allergies;
            Student.FullName = FullName;
            //IsAdd = false;
            IsLoadingRequierements = true;
            await _fibAddGenericService.UpdateChild(Student, "Students", Student.Id.ToString());
            IsLoadingRequierements = false;
            DataFormVisibility = true;
            //StudentSummaryVisibility = false;
            IsContract = true;

        }

        private async void OnResetCommand()
        {
            var prompt = await App.Current.MainPage.DisplayAlert("Alert", "This action will reset all information taken for this student and start from the begin in, Are you sure to do so?", "ok", "cancel");

            if (prompt)
            {
                IsLoadingRequierements = true;
                if (Student.Id != null)
                {
                    await _fibAddGenericService.DeleteChild(Student.Id, "Students");
                }
                Student = new StudentModel();
                ResetFlags();
                IsLoadingRequierements = false;
            }

        }

        private async void OnConfirmCommand(object obj)
        {
            if (string.IsNullOrEmpty(Tuition))
            {
                await Shell.Current.DisplayAlert("Colegiatura", "Ingresa el monto de colegiatura mensual", "OK");
                return;
            }

            Student.Tuition = Tuition;
            IsAdd = false;
            IsLoadingRequierements = true;

            try
            {
                // Save student with tuition
                await _fibAddGenericService.UpdateChild(Student, "Students", Student.Id.ToString());

                // Update enrollment with tuition + inscription fee
                if (!string.IsNullOrEmpty(_currentEnrollmentId))
                {
                    var enrollUpdate = new EnrollmentModel
                    {
                        Id = _currentEnrollmentId,
                        StudentId = Student.Id,
                        CycleId = _selectedCycleId,
                        ClientId = Client.Id,
                        Status = Models.StudentStatus.Inscrito,
                        EnrollmentDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        StudentName = Student.FullName,
                        ClientName = Client.FullName,
                        Level = Student.Level,
                        Grade = Student.Grade,
                        CycleName = CyclceSelected,
                        Tuition = Tuition,
                        InscriptionFee = InscriptionFee ?? "0"
                    };
                    await _fibAddGenericService.UpdateChild(enrollUpdate, "Enrollments", _currentEnrollmentId);
                }

                // Generate BOTH contracts automatically: Ficha de Inscripcion + Contrato Escolar
                var cycle = new CycleModel { Name = CyclceSelected ?? "Ciclo actual" };
                var contractGen = new Services.Implementations.ContractGeneratorService();

                // 1. Ficha de Inscripción
                var fichaHtml = await contractGen.GenerateContractHtmlAsync(Student, Client, cycle, "Inscripcion");
                var ficha = new ContractModel
                {
                    Type = "Inscripcion", Status = "Pendiente Firma",
                    ClientId = Client.Id, StudentId = Student.Id,
                    StudentName = Student.FullName, ClientName = Client.FullName,
                    CycleName = CyclceSelected, HtmlContent = fichaHtml,
                    CreatedDate = DateTime.Now, Name = $"Ficha Inscripción - {Student.FullName}"
                };
                var fichaId = await _fibAddGenericService.AddChild(ficha, "Contracts");
                if (!string.IsNullOrEmpty(fichaId?.ToString()))
                { ficha.Id = fichaId.ToString(); await _fibAddGenericService.UpdateChild(ficha, "Contracts", ficha.Id!); }

                // 2. Contrato de Servicios Escolares
                var contratoHtml = await contractGen.GenerateContractHtmlAsync(Student, Client, cycle, "ContratoEscolar");
                var contrato = new ContractModel
                {
                    Type = "ContratoEscolar", Status = "Pendiente Firma",
                    ClientId = Client.Id, StudentId = Student.Id,
                    StudentName = Student.FullName, ClientName = Client.FullName,
                    CycleName = CyclceSelected, HtmlContent = contratoHtml,
                    CreatedDate = DateTime.Now, Name = $"Contrato Escolar - {Student.FullName}"
                };
                var contratoId = await _fibAddGenericService.AddChild(contrato, "Contracts");
                if (!string.IsNullOrEmpty(contratoId?.ToString()))
                { contrato.Id = contratoId.ToString(); await _fibAddGenericService.UpdateChild(contrato, "Contracts", contrato.Id!); }

                await Shell.Current.DisplayAlert("✅ Inscripción Exitosa",
                    $"{Student.FullName} inscrito correctamente.\nContrato de inscripción generado.", "OK");

                // Navigate back to CycleDashboard
                await Shell.Current.GoToAsync("//CycleDashboardPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo completar: {ex.Message}", "OK");
            }
            finally
            {
                IsLoadingRequierements = false;
            }
        }

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

        #region subs flow commands

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
                Student.CURP = StudentCURP;
                Student.Address = StudentAddress;
                Student.Phone = StudentPhone;
                Student.Religion = StudentReligion;
                Student.PreviousSchool = StudentPreviousSchool;
                BackSubsVisibility = false;
                IsLoadingRequierements = true;
                IsAdd = false;
                var id = await _fibAddGenericService.AddChild(Student, "Students");

                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    Student.Id = id.ToString();
                    await _fibAddGenericService.UpdateChild(Student, "Students", id.ToString());

                    // Create Enrollment linking student to active cycle
                    var enrollment = new EnrollmentModel
                    {
                        StudentId = Student.Id,
                        CycleId = _selectedCycleId,
                        ClientId = Client.Id,
                        Status = Models.StudentStatus.Inscrito,
                        EnrollmentDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        StudentName = Student.FullName,
                        ClientName = Client.FullName,
                        Level = Student.Level,
                        Grade = Student.Grade,
                        CycleName = CyclceSelected,
                        Tuition = Student.Tuition,
                        InscriptionFee = InscriptionFee ?? "0"
                    };
                    var enrollId = await _fibAddGenericService.AddChild(enrollment, "Enrollments");
                    if (!string.IsNullOrEmpty(enrollId?.ToString()))
                    {
                        enrollment.Id = enrollId.ToString();
                        _currentEnrollmentId = enrollment.Id;
                        await _fibAddGenericService.UpdateChild(enrollment, "Enrollments", enrollment.Id!);
                    }
                }
                
                StudentSummaryVisibility = true;
                Imagevisibility = true;
                IsAdd = true;
                IsLoadingRequierements = false;

            }
        }

       
        private void OnBackSubsCommnad()
        {
            OnResetCommand();
        }

        private async void ResetFlags()
        {
           
            IsAdd = true;
            FullName = "";
            Student = new StudentModel();
            CycleVisibility = true;
            LevelsVisibility = false;
            Imagevisibility = false;
            GradesVisibility = false;
            StatusVisibility = false;
            IsContract = false;
            StudentSummaryVisibility = false;

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
            var grades = model.Grades ?? new List<StudentGradesModel>();

            // Fallback: if no grades from Firebase, use predefined PrimaryLevels
            if (grades.Count == 0)
            {
                grades = PrimaryLevels.Grades.Select(g => new StudentGradesModel
                {
                    Name = $"{g.Grade} ({g.Phase})",
                    LevelId = model.Id
                }).ToList();
            }

            if (grades.Count > 0)
            {
                Grades.Clear();
                foreach (var item in grades)
                    Grades.Add(item);
                CycleVisibility = false;
                LevelsVisibility = false;
                GradesVisibility = true;
                StatusVisibility = false;
            }
            else
            {
                GradeSelected = "N/A";
                CycleVisibility = false;
                LevelsVisibility = false;
                GradesVisibility = false;
                StatusVisibility = true;
            }
        }

      
        private async void OnSelectCycleCommand(CycleModel model)
        {
            Student.ActualCycle = model.Name;
            CyclceSelected = model.Name;
            _selectedCycleId = model.Id;

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

        #endregion

        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void OnDeleteCommand(object obj)
        {
            //throw new NotImplementedException();
        }

        private void OnAddCommand(object obj)
        {
           // throw new NotImplementedException();
        }

        #endregion private methods
    }
}

