using System;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Client), "Client")]
    public class AddStudentPageViewModel : BaseViewModel
	{
        private ClientModel _clientUser;

        public ClientModel Client
        { get => _clientUser; set { _clientUser = value; OnPropertyChanged(); } }


        public AddStudentPageViewModel(IFibAddGenericService<object> fibAddGenericService)
		{
			this._fibAddGenericService = fibAddGenericService;
			AddCommand = new Command(OnAddCommand);
            DeleteCommand = new Command(OnDeleteCommand);
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

