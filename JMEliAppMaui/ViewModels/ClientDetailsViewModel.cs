using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;
using JMEliAppMaui.Views;
 
namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(Client), "Client")]
    public class ClientDetailsViewModel : BindableObject
    {
        #region props
        public ObservableCollection<StudentModel> StudentList {get;set;}
        public ObservableCollection<ContractModel> Contracts { get; set; }

        private ClientModel _clientUser;

        public ClientModel Client
        { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }

        public string ClientStatusColor
        { 
          get => _ClientStatusColor; 
          set { 
                _ClientStatusColor = value;
                //If not handled windows crashes with message"No installed components were detected." sometimes
                //Exception thrown: 'System.Runtime.InteropServices.COMException' in WinRT.Runtime.dll
                //An exception of type 'System.Runtime.InteropServices.COMException' PENDING INVESTIGATION (seems it is bug in MAUI libraries)
                //Other option is to define property in constructor.
                try
                {
                    OnPropertyChanged();
                }
                catch(Exception ex)//handling exception for Windows crash
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public string ClientStatusMessage
        {
            get => _ClientStatusMessage; set
            {
                _ClientStatusMessage = value; OnPropertyChanged();
            }
        }
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }

        private bool _isEdit, _isLoading, _IsLoadingRequierements;
        public bool IsEdit { get => _isEdit; set { _isEdit = value; OnPropertyChanged(); } }
        public bool IsLoadingRequierements { get => _IsLoadingRequierements; set { _IsLoadingRequierements = value; OnPropertyChanged(); } }



        private string _ClientStatusMessage;
        private string _ClientStatusColor;
        private bool _isEditVisible, _ClientStatusVisibility;
        public bool IsEditVisible { get => _isEditVisible; set { _isEditVisible = value; OnPropertyChanged(); } }
        public bool ClientStatusVisibility { get => _ClientStatusVisibility; set { _ClientStatusVisibility = value; OnPropertyChanged(); } }
        private bool _isPayment;
        public bool IsPayments { get => _isPayment; set { _isPayment = value; OnPropertyChanged(); } }

        private bool _isContract;
        public bool IsContract { get => _isContract; set { _isContract = value; OnPropertyChanged(); } }

        private bool _isAddStudent;
        public bool IsAddStudent { get => _isAddStudent; set { _isAddStudent = value; OnPropertyChanged(); } }
        private string _imageUrl;
        public string ImageUrl { get => _imageUrl; set { _imageUrl = value; OnPropertyChanged(); } }

        public ICommand EditCommand { get; private set; }
        public ICommand ContractCommand { get; private set; }
        public ICommand PaymentsCommand { get; private set; }
        public ICommand AddStudentCommand { get; private set; }
        public ICommand GenerateContractCommand { get; private set; }
        public ICommand DetailsContractCommand { get; private set; }
        public ICommand OnAppearingCommand { get; set; }
        public ICommand UpdateUserImageCommand { get; private set; }
        public ICommand StudentDetailsCommand { get; private set; }
        

        private IFibCRUDClients _fibCRUDClients;
        private IFibStorageService _fibStorage;
        private IFibStatusService _fibStatusService;
        IFibLevelsService _fibLevelsService;
        IFibCyclesService _fibCycles;
        IFibContract _fibContract;
        IContractGeneratorService _contractGenerator;
        IFibAddGenericService _fibAddGenericService;

        #endregion

        public ClientDetailsViewModel(IFibCRUDClients fibCRUDClients,
            IFibStatusService fibStatusService,
            IFibLevelsService fibLevelsService,
            IFibCyclesService fibCycles,
            IFibStorageService fibStorageService, IFibContract fibContract,
            IContractGeneratorService contractGenerator,
            IFibAddGenericService fibAddGenericService)
        {
            this._contractGenerator = contractGenerator;
            this._fibAddGenericService = fibAddGenericService;
            this._fibContract = fibContract;
            this._fibCRUDClients = fibCRUDClients;
            this._fibStorage = fibStorageService;
            this._fibStatusService = fibStatusService;
            this._fibCycles = fibCycles;
            this._fibLevelsService = fibLevelsService;

            StudentList = new ObservableCollection<StudentModel>();
            Contracts = new ObservableCollection<ContractModel>();

            IsEdit = false;
            IsEditVisible = false;
            EditCommand = new Command(OnEditCommand);
            ContractCommand = new Command(OnContractCommand);
            AddStudentCommand = new Command(OnAddStudentCommand);
            PaymentsCommand = new Command(OnPaymentsCommand);
            UpdateUserImageCommand = new Command(OnUpdateUserImageCommand);
            GenerateContractCommand = new Command(OnGenerateContract);
            DetailsContractCommand = new Command<ContractModel>(OnDetailsContract);
            ImageUrl = "user_icon.png";
            OnAppearingCommand = new Command(async() => await OnOnAppearingCommand());
            StudentDetailsCommand = new Command<StudentModel>(OnStudentDetailsCommand);
            IsLoading = false;
            ClientStatusVisibility = false;
#if WINDOWS
            ClientStatusColor = "Green";
#endif
        }

        private async void OnStudentDetailsCommand(StudentModel model)
        {

            if (string.IsNullOrEmpty(model.UrlImage))
            {
                model.UrlImage = "user_icon.png";
            }
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Student",model}
            };
            await AppShell.Current.GoToAsync(nameof(StudentDetailsPage), true, parameters);

        }

        private async Task OnOnAppearingCommand()
        {
            ResetFlags(true);
            IsLoadingRequierements = true;
            IsLoading = true;
            await GetStudentsFronClient();
            IsLoading = false;
            IsLoadingRequierements = false;
        }

        async Task GetStudentsFronClient()
        {
            var students = await _fibCRUDClients.GetStudentsFromClient(Client.Id);
            if (students.Count() > 0)
            {
               
                int studentsBadStatus = 0;
                //StudentList.Clear();
                foreach (var item in students)
                {

                    if (item.Status != "Vigente")
                    {
                        studentsBadStatus++;
                    }
                    StudentList.Add(item);
                }
                if (studentsBadStatus > 0)
                {
                    ClientStatusColor = "Red";
                    ClientStatusMessage = "One or more students over due.";
                }
                else
                {
                    ClientStatusMessage = "Vigente";
                    ClientStatusColor = "Green";
                }
                IsAddStudent = true;
                ClientStatusVisibility = true;

                var contractsResult = await ((IFirebaseService)_fibAddGenericService).GetWhereAsync<ContractModel>(
                    "Contracts", c => c.ClientId == Client.Id);
                if (Contracts == null)
                    Contracts = new ObservableCollection<ContractModel>();
                Contracts.Clear();
                foreach (var contract in contractsResult)
                {
                    Contracts.Add(contract);
                }
            }
            else
            {
                IsEditVisible = true;
            }
        }

        public void SetClientOnVM(ClientModel client)
        {
            if (Client == null)
            {
                Client = new ClientModel();
            }
            Client = client;
        }

        void ResetFlags(bool isOnAppearing = false)
        {
            IsEditVisible = false;
            IsPayments = false;
            IsContract = false;
            IsAddStudent = false;
            IsEdit = false;
            if (isOnAppearing)
            {
                if (Contracts == null || StudentList == null)
                {
                    StudentList = new ObservableCollection<StudentModel>();
                    Contracts = new ObservableCollection<ContractModel>();
                }
                Contracts.Clear();
                StudentList.Clear();
            }
          
        }
        async void OnUpdateUserImageCommand()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;

            var picture = await MediaPicker.PickPhotoAsync();
            if (picture != null)
            {
                var stream = await picture.OpenReadAsync();
                try
                {
                    var img = await _fibStorage.AddImageFibStorge(Client.Id, "UserImage", stream);
                    if (!string.IsNullOrEmpty(img))
                    {
                        ImageUrl = img;
                        Client.UrlImage = img;
                        SetClientOnVM(Client);
                        await _fibCRUDClients.UpdateClient(Client);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {ex.Message} {ex.Data}");
                }
            }
            IsLoading = false;

        }

        private void OnPaymentsCommand()
        {
            ResetFlags();
            IsPayments = true;
        }

        private async void OnAddStudentCommand()
        {
            
            if (IsAddStudent)
            {
                ResetFlags();
                IsLoading = true;
                

                var client = Client;
                IsEditVisible = true;
                IsLoading = false;
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "Client",client}
                };
                await AppShell.Current.GoToAsync(nameof(AddStudentPage), true, parameters);
                IsAddStudent = false;
            }
            else if (!IsAddStudent && StudentList.Count > 0)
            {
                ResetFlags();
                IsAddStudent = true;

            }
            else if(!IsAddStudent)
            {
                ResetFlags();
                IsAddStudent = true;
            }
           
        }

        private void OnContractCommand()
        {
            ResetFlags();
            IsContract = true;
        }

        private async void OnDetailsContract(ContractModel contract)
        {
            if (contract == null) return;
            await Shell.Current.GoToAsync(nameof(ContractViewerPage), true,
                new Dictionary<string, object>
                {
                    { nameof(ContractModel), contract }
                });
        }

        private async void OnGenerateContract()
        {
            if (Client == null || StudentList == null || StudentList.Count == 0)
            {
                await Shell.Current.DisplayAlert("Info", "Primero inscribe un alumno para generar contrato", "OK");
                return;
            }

            // If multiple students, let user pick which one
            StudentModel student;
            if (StudentList.Count == 1)
            {
                student = StudentList.First();
            }
            else
            {
                var studentNames = StudentList.Select(s => s.FullName ?? "Sin nombre").ToArray();
                var selected = await Shell.Current.DisplayActionSheet(
                    "¿Para cuál alumno?", "Cancelar", null, studentNames);
                if (string.IsNullOrEmpty(selected) || selected == "Cancelar") return;
                student = StudentList.FirstOrDefault(s => s.FullName == selected) ?? StudentList.First();
            }

            // Pick contract type
            var templates = _contractGenerator.GetAvailableTemplates();
            var contractType = await Shell.Current.DisplayActionSheet(
                "Tipo de contrato", "Cancelar", null, templates.ToArray());
            if (string.IsNullOrEmpty(contractType) || contractType == "Cancelar") return;

            var cycle = new CycleModel { Name = student.ActualCycle ?? "Ciclo actual" };

            try
            {
                // Generate HTML
                var html = await _contractGenerator.GenerateContractHtmlAsync(student, Client, cycle, contractType);

                // Save to Firebase
                var contract = new ContractModel
                {
                    Type = contractType,
                    Status = "Generado",
                    ClientId = Client.Id,
                    StudentId = student.Id,
                    StudentName = student.FullName,
                    ClientName = Client.FullName,
                    CycleName = student.ActualCycle,
                    HtmlContent = html,
                    CreatedDate = DateTime.Now
                };

                var id = await _fibAddGenericService.AddChild(contract, "Contracts");
                contract.Id = id?.ToString();
                contract.Name = $"{contractType} - {student.FullName}";
                await _fibAddGenericService.UpdateChild(contract, "Contracts", contract.Id!);

                // Add to local list
                Contracts.Add(contract);

                await Shell.Current.DisplayAlert("✅ Contrato Generado",
                    $"Contrato de {contractType} para {student.FullName} guardado exitosamente.", "Ver Contrato");

                // Open in WebView — save HTML to temp file for WebView
                var filePath = Path.Combine(FileSystem.CacheDirectory, $"contract_{contract.Id}.html");
                await File.WriteAllTextAsync(filePath, html);

                await Shell.Current.GoToAsync(nameof(ContractViewerPage), true,
                    new Dictionary<string, object>
                    {
                        { nameof(ContractModel), new ContractModel { Url = filePath, Type = contractType, ClientId = Client.Id, StudentId = student.Id } }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo generar: {ex.Message}", "OK");
            }
        }

        private void OnEditCommand()
        {
            IsLoading = true;
            if (IsEdit)
            {               
                _fibCRUDClients.UpdateClient(Client);
            }
            else
            {
                ResetFlags();
                App.Current.MainPage.DisplayAlert("Edit", "now u can edit user", "ok");
                IsEditVisible = true;
                IsEdit = true;
            }
            IsLoading = false;
        }
    }
}

