using System;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Client), "Client")]
    public class AddStudentPageViewModel : BaseViewModel
	{
        private ClientModel _clientUser;

        public ClientModel Client
        { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }

        private IFibStorageService _fibStorage;
        private IFibStatusService _fibStatusService;
        IFibLevelsService _fibLevelsService;
        IFibCyclesService _fibCycles;

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
			AddCommand = new Command(OnAddCommand);
            DeleteCommand = new Command(OnDeleteCommand);
            AppearingCommand = new Command(OnAppearingCommand);
            OnAppearingCommand();
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
                await App.Current.MainPage.DisplayAlert("Alert", "you are missing cycles , levels or status to subscribe a student to this user, status is also used for clients, please make sure you have them to procede", "ok");

                await AppShell.Current.GoToAsync("/..");
            }
            else {
                IsAdd = true;
            }
            //this could be subsctract in a service
            IsLoadingRequierements = false;
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

